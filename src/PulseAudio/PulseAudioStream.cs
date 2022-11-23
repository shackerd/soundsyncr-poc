using System.Collections.ObjectModel;
using Midicontrol.PulseAudio.DBus;

namespace Midicontrol.PulseAudio
{
    public class PulseAudioStream
    {        
        private StreamProperties _properties;
        private const string _applicationProcessBinary = "application.process.binary";
        private const string _applicationProcessId = "application.process.id";
        private const string _applicationProcessName = "application.process.Name";
        internal readonly IReadOnlyDictionary<string, string> _readProperties;    
        
        public IStreamProxy Proxy { get; }        
        public string Binary => _readProperties[_applicationProcessBinary];
        public string Pid => _readProperties[_applicationProcessId];
        public string Name => _readProperties[_applicationProcessName];
        public PulseAudioStreamType Type { get; }
        public IReadOnlyDictionary<string, string> Properties => _readProperties;    

        internal PulseAudioStream(IStreamProxy proxy, StreamProperties properties, PulseAudioStreamType type, Dictionary<string, string> readProperties)
        {
            Proxy = proxy;            
            Type = type;
            _properties = properties;
            _readProperties = new ReadOnlyDictionary<string, string>(readProperties);            
        }        

        public static async Task<PulseAudioStream> CreateAsync(IStreamProxy proxy, PulseAudioStreamType type)
        {            
            StreamProperties props = await proxy.GetAllAsync();

            Dictionary<string, string> kvs = new Dictionary<string, string>();

            foreach (string key in props.PropertyList.Keys)
            {
                kvs.Add(key, System.Text.Encoding.UTF8.GetString(GetPropertyValue(props, key)));
            }

            return new PulseAudioStream(proxy, props, type, kvs);
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