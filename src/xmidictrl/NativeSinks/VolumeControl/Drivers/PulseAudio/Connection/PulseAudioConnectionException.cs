namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    [System.Serializable]
    public class PulseAudioConnectionException : System.Exception
    {
        public PulseAudioConnectionException() { }
        public PulseAudioConnectionException(string message) : base(message) { }
        public PulseAudioConnectionException(string message, System.Exception inner) : base(message, inner) { }
        protected PulseAudioConnectionException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}