using Midicontrol.Midi.NativeSinks.VolumeControl;

namespace Midicontrol.Midi.NativeSinks.MicrosoftAudio
{
    internal class MicrosoftAudioDriver : IAudioDriver
    {
        public Task<IEnumerable<IAudioStream>> GetStreamsAsync(string destination)
        {
            throw new NotImplementedException();
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public Task ToggleSoloAsync(IAudioStream stream, StreamType type, bool solo)
        {
            throw new NotImplementedException();
        }
    }
}