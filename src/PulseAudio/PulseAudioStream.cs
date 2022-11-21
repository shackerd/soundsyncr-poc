using Midicontrol.PulseAudio.DBus;

namespace Midicontrol.PulseAudio
{
    public class PulseAudioStream
    {
        public IStreamProxy Proxy { get; }        
        public string Binary { get; }
        public string Pid { get; }
        public PulseAudioStreamType Type { get; }
        
        private const string _applicationProcessBinary = "application.process.binary";
        private const string _applicationProcessId = "application.process.id";

        internal PulseAudioStream(IStreamProxy proxy, string appBinary, string pid, PulseAudioStreamType type)
        {
            Proxy = proxy;
            Binary = appBinary;
            Pid = pid;
            Type = type;
        }

        public static async Task<PulseAudioStream> CreateAsync(IStreamProxy proxy, PulseAudioStreamType type)
        {
            var props = await proxy.GetAllAsync();
            var app = System.Text.Encoding.Default.GetString(props.PropertyList[_applicationProcessBinary]);
            var pid = System.Text.Encoding.Default.GetString(props.PropertyList[_applicationProcessId]);
            return new PulseAudioStream(proxy, app, pid, type);
        }
    }
}