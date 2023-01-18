namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal class PulseAudioStreamStore
    {
        private readonly List<IPulseAudioStream> _streams = new List<IPulseAudioStream>();

        public List<IPulseAudioStream> Streams => _streams; // implement methods later

        public PulseAudioStreamStore()
        {

        }
    }
}