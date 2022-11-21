using Midicontrol.PulseAudio.DBus;

namespace Midicontrol.PulseAudio
{
    public class PulseAudioStream
    {
        public IStreamProxy Proxy { get; }        
        public string Binary { get; }
        public string Pid { get; }
        public PulseAudioStreamType Type { get; }
        public StreamProperties Properties { get; }
        private const string _applicationProcessBinary = "application.process.binary";
        private const string _applicationProcessId = "application.process.id";

        internal PulseAudioStream(IStreamProxy proxy, StreamProperties properties, string appBinary, string pid, PulseAudioStreamType type)
        {
            Proxy = proxy;
            Binary = appBinary;
            Pid = pid;
            Type = type;
            Properties = properties;
        }

        public static async Task<PulseAudioStream> CreateAsync(IStreamProxy proxy, PulseAudioStreamType type)
        {            
            StreamProperties props = await proxy.GetAllAsync();
            var app = System.Text.Encoding.Default.GetString(GetPropertyValue(props, _applicationProcessBinary));
            var pid = System.Text.Encoding.Default.GetString(GetPropertyValue(props, _applicationProcessId));            
            return new PulseAudioStream(proxy, props, app, pid, type);
        }

        private static byte[] GetPropertyValue(StreamProperties properties, string propName) 
        {
            byte[] rawValue = properties.PropertyList[propName];

            byte[] value = new byte[rawValue.Length - 1];

            for (int i = 0; i < rawValue.Length - 1; i++)
            {
                value[i] = rawValue[i];
            }

            return value;
        }
    }
}