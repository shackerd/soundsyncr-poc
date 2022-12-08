using Microsoft.Extensions.Logging;
using Midicontrol.Midi.NativeSinks.PulseAudio.DBus;
using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal sealed class PulseAudioDeviceStream : IPulseAudioStream
    {
        private readonly IDevice _proxy;
        private readonly ILogger<PulseAudioDeviceStream> _logger;
        private bool disposedValue;

        public Scope Scope => Scope.Device;

        public StreamType Type { get; }

        public string? Identifier { get; }
        public ObjectPath ObjectPath { get; }

        public IAudioStream? Root => null;

        public PulseAudioDeviceStream(
            StreamType type,
            string? identifier,
            ObjectPath objectPath,
            IDevice proxy,
            ILogger<PulseAudioDeviceStream> logger
        )
        {
            Type = type;
            Identifier = identifier;
            ObjectPath = objectPath;
            _proxy = proxy;
            _logger = logger;
        }

        private void AssertProxyIsAlive()
        {
            if(_proxy == null)
            {
                throw new InvalidOperationException("Proxy is not initialized");
            }
        }

        public Task<uint> GetVolumeAsync()
        {
            AssertProxyIsAlive();
            return GetVolumeAsyncInternal();
        }

        internal async Task<uint> GetVolumeAsyncInternal()
        {
            uint[] volume = await _proxy.GetVolumeAsync();
            return volume.First();
        }

        public Task SetVolumeAsync(uint value)
        {
            AssertProxyIsAlive();
            return _proxy.SetVolumeAsync(new uint[1] { value.ToRatioUInt16() });
        }

        public Task ToggleMuteAsync(bool value)
        {
            AssertProxyIsAlive();
            return _proxy.SetMuteAsync(value);
        }

        private void Dispose(bool disposing)
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