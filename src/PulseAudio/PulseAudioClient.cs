using Tmds.DBus;
using Midicontrol.PulseAudio.DBus;
using System.Collections.Generic;

namespace Midicontrol.PulseAudio
{  
    public class PulseAudioClient
    {
        private const string _serverLookupServiceName = "org.PulseAudio1";
        private const string _serverLookupObjectPath = "/org/pulseaudio/server_lookup1";
        private const string _coreSeviceName = "org.PulseAudio.Core1";
        private const string _core1ObjectPath = "/org/pulseaudio/core1";
        private const string _streamServiceName = "org.PulseAudio.Core1.Stream";     
        private Connection _connection;

        private readonly List<PulseAudioStream> _streams = new List<PulseAudioStream>();        

        public IEnumerable<PulseAudioStream> RecordStreams => _streams.Where(s => s.Type == PulseAudioStreamType.RecordStream);
        public IEnumerable<PulseAudioStream> PlaybackStreams => _streams.Where(s => s.Type == PulseAudioStreamType.PlaybackStream);
        public IEnumerable<PulseAudioStream> AllStreams => _streams.AsEnumerable();

        private readonly SynchronizationContext _synCtx;

        public PulseAudioClient(SynchronizationContext synCtx)
        {
            _synCtx = synCtx;
        }

        private async Task<string> ServerLookupAsync(){
            
            Connection sessionConnection = Connection.Session;

            IServerLookupProxy proxy = 
                sessionConnection.CreateProxy<IServerLookupProxy>(
                    _serverLookupServiceName, 
                    _serverLookupObjectPath
                );

            ServerLookupProperties res = await proxy.GetAllAsync().ConfigureAwait(false);                             
            
            return res.Address;
        }

        public Task ConnectAsync() {
            return Task.Factory.StartNew(() => ConnectAsyncInternal());
        }

        private async Task ConnectAsyncInternal(){
                        
            string address = await ServerLookupAsync().ConfigureAwait(false);            
            
            ClientConnectionOptions options = new ClientConnectionOptions(address);
            // options.RunContinuationsAsynchronously = false;
            options.SynchronizationContext = _synCtx;
            
            _connection = new Connection(options);

            _connection.StateChanged += new EventHandler<ConnectionStateChangedEventArgs>(OnConnectionEvent);

            ConnectionInfo inf = await _connection.ConnectAsync().ConfigureAwait(false);
                        
            ICoreProxy proxy = _connection.CreateProxy<ICoreProxy>(_coreSeviceName,_core1ObjectPath);
            
            CoreProperties core = await proxy.GetAllAsync().ConfigureAwait(false);

            await GatherStreamsAsync(core).ConfigureAwait(false);
            
            await ListenToStreamsChangesAsync(proxy).ConfigureAwait(false);                
        }        

        private void OnConnectionEvent(object _, ConnectionStateChangedEventArgs eventArgs) 
        {
            switch (eventArgs.State)
            {
                case ConnectionState.Disconnecting:
                case ConnectionState.Disconnected:
                    throw new Exception("Connection close");                    
                default:
                    Console.WriteLine($"PulseAudio-Connection: {eventArgs.State}");
                    break;
            }
        }

        private async Task GatherStreamsAsync(CoreProperties core)
        {
            foreach (ObjectPath path in core.PlaybackStreams)
            {                
                PulseAudioStream stream = await GetStreamObjectAsync(_connection, path, PulseAudioStreamType.PlaybackStream).ConfigureAwait(false);
                _streams.Add(stream);
            }

            foreach (ObjectPath path in core.RecordStreams)
            {                
                PulseAudioStream stream = await GetStreamObjectAsync(_connection, path, PulseAudioStreamType.RecordStream).ConfigureAwait(false);
                _streams.Add(stream);
            }            
        }

        private async Task OnStreamCreated(ObjectPath path, PulseAudioStreamType type) 
        {
            // todo lock (concurrent)
            PulseAudioStream stream = 
                await GetStreamObjectAsync(_connection, path, type).ConfigureAwait(false); 

            Console.WriteLine($"Created stream: {stream.Binary}:{stream.Pid}");

            _streams.Add(stream);
        }

        private void OnStreamDeleted(ObjectPath path) 
        {
            // todo lock (concurrent)
            PulseAudioStream? stream = _streams.FirstOrDefault(s => s.Proxy.ObjectPath == path);

            if(stream == null) {
                return;
            }

            Console.WriteLine($"Removed stream: {stream.Binary}:{stream.Pid}");

            _streams.Remove(stream);
        }

        private async Task<PulseAudioStream> GetStreamObjectAsync(Connection connection, ObjectPath path, PulseAudioStreamType type) 
        {            
            IStreamProxy proxy = connection.CreateProxy<IStreamProxy>(_streamServiceName, path);
            PulseAudioStream stream = await PulseAudioStream.CreateAsync(proxy, type).ConfigureAwait(false);
            return stream;
        }

        private IDisposable _newPlaybackStreamSubscription;
        private IDisposable _newRecordStreamSubscription;
        private IDisposable _playbackStreamRemovedSubscription;
        private IDisposable _recordStreamRemovedSubscription;
        
        private async Task ListenToStreamsChangesAsync(ICoreProxy proxy) 
        {
            _newPlaybackStreamSubscription = await proxy.WatchNewPlaybackStreamAsync(
                async (path) => { 
                    await OnStreamCreated(path, PulseAudioStreamType.PlaybackStream)
                        .ConfigureAwait(false);
                }, 
                (ex) => { Console.WriteLine(ex.ToString()); }
            ).ConfigureAwait(false);

            _newRecordStreamSubscription = await proxy.WatchNewRecordStreamAsync(
                async (path) => {  
                    await OnStreamCreated(path, PulseAudioStreamType.RecordStream)
                        .ConfigureAwait(false);
                }, 
                (ex) => { 
                    Console.WriteLine(ex.ToString()); 
                }
            ).ConfigureAwait(false);

            _playbackStreamRemovedSubscription = await proxy.WatchPlaybackStreamRemovedAsync(
                (path) => { 
                    OnStreamDeleted(path); 
                }, 
                (ex) => { Console.WriteLine(ex.ToString()); }
            ).ConfigureAwait(false);

            _recordStreamRemovedSubscription = await proxy.WatchRecordStreamRemovedAsync(
                (path) => { 
                    OnStreamDeleted(path); 
                }, 
                (ex) => { 
                    Console.WriteLine(ex.ToString()); 
                }
            ).ConfigureAwait(false);                        

            ObjectPath[] array = new List<ObjectPath>().ToArray();
            
            // Notify PulseAudio we are listening to the following events
            await proxy.ListenForSignalAsync($"{_coreSeviceName}.NewPlaybackStream", array).ConfigureAwait(false);            
            await proxy.ListenForSignalAsync($"{_coreSeviceName}.PlaybackStreamRemoved", array).ConfigureAwait(false);            
            await proxy.ListenForSignalAsync($"{_coreSeviceName}.NewRecordStream", array).ConfigureAwait(false);            
            await proxy.ListenForSignalAsync($"{_coreSeviceName}.RecordStreamRemoved", array).ConfigureAwait(false);
            
        }
    }     
}