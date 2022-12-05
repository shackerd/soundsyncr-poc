namespace Midicontrol.Midi.NativeSinks
{
    internal interface IAudioStream : IDisposable
    {
        // IAudioStreamRef Root { get; }
        Scope Scope { get; }
        StreamType Type { get; }
        string? Identifier { get; }
        Task<uint> GetVolumeAsync();
        Task SetVolumeAsync(uint value);
        Task ToggleMuteAsync(bool value);
    }

    internal interface IAudioStreamRef { }

    // pulseaudio

    internal class PulseAudioStreamRef : IAudioStreamRef
    {

        private readonly Tmds.DBus.ObjectPath _path;
        public PulseAudioStreamRef(Tmds.DBus.ObjectPath path)
        {
            _path = path;
        }

        public static implicit operator PulseAudioStreamRef(Tmds.DBus.ObjectPath path){
            return new PulseAudioStreamRef(path);
        }

        public static implicit operator Tmds.DBus.ObjectPath(PulseAudioStreamRef streamRef){
            return streamRef._path;
        }
    }
}