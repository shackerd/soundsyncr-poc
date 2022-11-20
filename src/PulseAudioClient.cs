using Tmds.DBus;
using Core1.DBus;
using System.Collections.Generic;

namespace Midicontrol
{  
    public class PulseAudioClient
    {
        private const string _coreSeviceName = "org.PulseAudio.Core1";
        private const string _core1ObjectPath = "/org/pulseaudio/core1";
        private const string _streamServiceName = "org.PulseAudio.Core1.Stream";     

        private readonly List<PulseAudioStream> _recordStreams = new List<PulseAudioStream>();
        private readonly List<PulseAudioStream> _playbackStreams = new List<PulseAudioStream>();

        public List<PulseAudioStream> RecordStreams => _recordStreams;
        public List<PulseAudioStream> PlaybackStreams => _playbackStreams;

        private async Task<string> ServerLookupAsync(){
            
            var sessionConnection = Connection.Session;

            var proxy = 
                sessionConnection.CreateProxy<PulseAudio1.DBus.IServerLookup1>(
                    "org.PulseAudio1", 
                    "/org/pulseaudio/server_lookup1"
                );

            var res = await proxy.GetAllAsync();                             
            
            return res.Address;
        }

        public async Task ConnectAsync(){

            string address = await ServerLookupAsync();
            
            Connection cnn = new Connection(address);            

            cnn.StateChanged += new EventHandler<ConnectionStateChangedEventArgs>(
                (s, e) => {
                    Console.WriteLine(e.State);
                }
            );

            ConnectionInfo inf = await cnn.ConnectAsync();
                        
            ICore1 props = cnn.CreateProxy<ICore1>(_coreSeviceName,_core1ObjectPath);
            
            Core1Properties core = await props.GetAllAsync();
        
            for (int i = 0; i < core.RecordStreams.Count(); i++)
            {
                var objectPath = core.RecordStreams[i];
                var proxy = cnn.CreateProxy<IStream>(_streamServiceName, objectPath);
                var stream = await PulseAudioStream.CreateAsync(proxy);
                _recordStreams.Add(stream);
            }
            
            
            for (int i = 0; i < core.PlaybackStreams.Count(); i++)
            {               
                var objectPath = core.PlaybackStreams[i];
                var proxy = cnn.CreateProxy<IStream>(_streamServiceName, objectPath);
                var stream = await PulseAudioStream.CreateAsync(proxy);
                _playbackStreams.Add(stream);
            }
            
        }
    }

    public class PulseAudioStream
    {
        public IStream Proxy { get; }        
        public string Binary { get; }
        public string Pid { get; }

        internal PulseAudioStream(IStream proxy, string appBinary, string pid)
        {
            Proxy = proxy;
            Binary = appBinary;
            Pid = pid;
        }

        public static async Task<PulseAudioStream> CreateAsync(IStream proxy)
        {
            var props = await proxy.GetAllAsync();
            var app = System.Text.Encoding.Default.GetString(props.PropertyList["application.process.binary"]);
            var pid = System.Text.Encoding.Default.GetString(props.PropertyList["application.process.id"]);
            return new PulseAudioStream(proxy, app, pid);
        }
    }
}