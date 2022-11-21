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

        private readonly List<PulseAudioStream> _streams = new List<PulseAudioStream>();        

        public IEnumerable<PulseAudioStream> RecordStreams => _streams.Where(s => s.Type == PulseAudioStreamType.RecordStream);
        public IEnumerable<PulseAudioStream> PlaybackStreams => _streams.Where(s => s.Type == PulseAudioStreamType.PlaybackStream);
        public IEnumerable<PulseAudioStream> AllStreams => _streams.AsEnumerable();


        private async Task<string> ServerLookupAsync(){
            
            Connection sessionConnection = Connection.Session;

            IServerLookupProxy proxy = 
                sessionConnection.CreateProxy<IServerLookupProxy>(
                    _serverLookupServiceName, 
                    _serverLookupObjectPath
                );

            ServerLookupProperties res = await proxy.GetAllAsync();                             
            
            return res.Address;
        }

        public async Task ConnectAsync(){

            string address = await ServerLookupAsync();
            
            Connection connection = new Connection(address);            

            connection.StateChanged += new EventHandler<ConnectionStateChangedEventArgs>(
                (s, e) => {
                    Console.WriteLine(e.State);
                }
            );

            ConnectionInfo inf = await connection.ConnectAsync();
                        
            ICoreProxy props = connection.CreateProxy<ICoreProxy>(_coreSeviceName,_core1ObjectPath);
            
            CoreProperties core = await props.GetAllAsync();
        
            PulseAudioStream[] ps = await GatherStreamObjectsAsync(connection, core.PlaybackStreams, PulseAudioStreamType.PlaybackStream);
            PulseAudioStream[] rs = await GatherStreamObjectsAsync(connection, core.RecordStreams, PulseAudioStreamType.RecordStream);

            _streams.AddRange(ps);
            _streams.AddRange(rs);
        }

        private async Task<PulseAudioStream[]> GatherStreamObjectsAsync(Connection connection, ObjectPath[] objectPaths, PulseAudioStreamType type) 
        {
            PulseAudioStream[] streams = new PulseAudioStream[objectPaths.Length];

            for (int i = 0; i < objectPaths.Length; i++)
            {
                ObjectPath path = objectPaths[i];
                IStreamProxy proxy = connection.CreateProxy<IStreamProxy>(_streamServiceName, path);
                PulseAudioStream stream = await PulseAudioStream.CreateAsync(proxy, type);
                streams[i] = stream;
            }            

            return streams;
        }
    }    
}