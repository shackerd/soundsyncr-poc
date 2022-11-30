namespace Midicontrol.Midi.NativeSinks
{
    // MS / PA same contract interface
    interface IAudioDriver
    {
        Task InitializeAsync();
        Task<IEnumerable<IAudioStream>> GetStreams(string destination);
        Task ToggleSoloAsync(IAudioStream stream, bool solo);
    }
}