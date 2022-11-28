using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    public interface IPulseAudioPropertyReader
    {
        IReadOnlyDictionary<string, string> ReadAll(IDictionary<string, byte[]> properties);
        string ReadProperty(IDictionary<string, byte[]> properties, string propName);
    }

    internal class PulseAudioPropertyReader : IPulseAudioPropertyReader
    {
        private readonly ILogger _logger;
        public PulseAudioPropertyReader(ILogger<IPulseAudioPropertyReader> logger)
        {            
            _logger =  logger;
        }
        
        public IReadOnlyDictionary<string, string> ReadAll(IDictionary<string, byte[]> properties)
        {
            Dictionary<string, string> converted = new();

            foreach (string propName in properties.Keys)
            {
                converted.Add(propName, ReadProperty(properties, propName));
            }

            return new ReadOnlyDictionary<string, string>(converted);
        }

        public string ReadProperty(IDictionary<string, byte[]> properties, string propName)
        {
            byte[] rawValue = properties[propName];

            // skip last nullbyte
            byte[] value = rawValue.Take(rawValue.Length - 1).ToArray();

            return Encoding.Default.GetString(value);
        }
    }
}