using Microsoft.Extensions.Logging;
using Midicontrol.PulseAudio.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal sealed class PulseAudioDeviceStream : IAudioStream
    {
        private readonly IDevice _proxy;
        private readonly ILogger<PulseAudioDeviceStream> _logger;
        private bool disposedValue;

        public Scope Scope => Scope.Device;

        public StreamType Type { get; }

        public string Identifier { get; }

        public PulseAudioDeviceStream(
            StreamType type,
            string identifier,
            IDevice proxy,
            ILogger<PulseAudioDeviceStream> logger
        )
        {
            Type = type;
            Identifier = identifier;
            _proxy = proxy;
            _logger = logger;
        }

        public async Task<uint> GetVolumeAsync()
        {
            uint[] volume = await _proxy.GetVolumeAsync();
            return volume.First();
        }

        public Task SetVolumeAsync(uint value)
        {
            return _proxy.SetVolumeAsync(new uint[1] { value.ToRatioUInt16() });
        }

        public Task ToggleMuteAsync(bool value)
        {
            return _proxy.SetMuteAsync(value);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}