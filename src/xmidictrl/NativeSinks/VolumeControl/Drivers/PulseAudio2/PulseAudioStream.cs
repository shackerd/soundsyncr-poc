using Microsoft.Extensions.Logging;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal sealed class PulseAudioStream : IAudioStream
    {
        private readonly ILogger<PulseAudioStream> _logger;

        public Scope Scope => throw new NotImplementedException();

        public StreamType Type => throw new NotImplementedException();

        private uint _volume;

        public string Identifier { get; }


        public PulseAudioStream(string identifier, ILogger<PulseAudioStream> logger)
        {
            Identifier = identifier;
            _logger = logger;
        }

        public Task<uint> GetVolumeAsync()
        {
            return Task.FromResult(_volume);
        }

        public Task SetVolumeAsync(uint value)
        {
            throw new NotImplementedException();
        }

        public Task ToggleMuteAsync(bool value)
        {
            _volume = value ? (uint)0 : (uint)127;

            return Task.CompletedTask;
        }
    }
}