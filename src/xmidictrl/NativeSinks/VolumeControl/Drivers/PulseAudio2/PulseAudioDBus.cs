namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    public static class PulseAudioDBus
    {
        public const string ServerLookupServiceName = "org.PulseAudio1";
        public const string ServerLookupObjectPath = "/org/pulseaudio/server_lookup1";
        public const string CoreSeviceName = "org.PulseAudio.Core1";
        public const string CoreObjectPath = "/org/pulseaudio/core1";
        public const string StreamServiceName = "org.PulseAudio.Core1.Stream";
        public const string DeviceServiceName = "org.PulseAudio.Core1.Device";
    }
}