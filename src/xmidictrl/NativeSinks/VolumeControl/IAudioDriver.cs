namespace Midicontrol.Midi.NativeSinks.VolumeControl
{
    // MS / PA same contract interface
    interface IAudioDriver
    {
        Task InitializeAsync();
        Task<IEnumerable<IAudioStream>> GetStreamsAsync(string destination);
        Task ToggleSoloAsync(IAudioStream stream, StreamType type, bool solo);
    }
}