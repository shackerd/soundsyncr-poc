namespace Midicontrol.Midi.NativeSinks
{
    internal interface IAudioStream : IDisposable
    {
        Scope Scope { get; }
        StreamType Type { get; }
        string Identifier { get; }
        Task<uint> GetVolumeAsync();
        Task SetVolumeAsync(uint value);
        Task ToggleMuteAsync(bool value);
    }
}