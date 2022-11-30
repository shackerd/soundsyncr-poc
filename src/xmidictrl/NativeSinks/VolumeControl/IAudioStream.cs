namespace Midicontrol.Midi.NativeSinks
{
    interface IAudioStream
    {
        Scope Scope { get; }
        StreamType Type { get; }
        string Identifier { get; }
        Task<uint> GetVolumeAsync();
        Task SetVolumeAsync(uint value);
        Task ToggleMuteAsync(bool value);
    }
}