using Tmds.DBus;
using Midicontrol.PulseAudio.DBus;
using System.Collections.Generic;

namespace Midicontrol.PulseAudio
{  
    public class PulseAudioClient
    {
        private const string _serverLookupServiceName = "org.PulseAudio1";
        private const string _serverLookupObjectPath = "/org/pulseaudio/server_lookup1";                
        private Connection _connection;

        private readonly SynchronizationContext _synCtx;

        private IPulseAudioStreamStore _streamStore;

        public IPulseAudioStreamStore StreamStore => _streamStore;

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

            options.SynchronizationContext = _synCtx;
            
            _connection = new Connection(options);

            _connection.StateChanged += new EventHandler<ConnectionStateChangedEventArgs>(OnConnectionEvent);

            ConnectionInfo inf = await _connection.ConnectAsync().ConfigureAwait(false);

            _streamStore = new PulseAudioStreamStore(_connection);

            await _streamStore.InitializeAsync().ConfigureAwait(false);
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
    }        
}