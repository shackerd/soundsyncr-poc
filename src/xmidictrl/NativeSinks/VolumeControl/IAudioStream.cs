namespace Midicontrol.Midi.NativeSinks.VolumeControl
{
    internal interface IAudioStream : IDisposable
    {
        IAudioStream? Root { get; }
        Scope Scope { get; }
        StreamType Type { get; }
        string? Identifier { get; }
        Task<uint> GetVolumeAsync();
        Task SetVolumeAsync(uint value);
        Task ToggleMuteAsync(bool value);
    }
}