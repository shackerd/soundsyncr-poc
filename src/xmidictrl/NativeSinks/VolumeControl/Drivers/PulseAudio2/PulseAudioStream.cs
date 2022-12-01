using Microsoft.Extensions.Logging;
using Midicontrol.PulseAudio.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal sealed class PulseAudioStream : IAudioStream
    {

        // private bool disposedValue;
        private readonly ILogger<PulseAudioStream> _logger;
        private readonly IStreamProxy _proxy;

        public Scope Scope { get; }

        public StreamType Type { get; }

        public string Identifier { get; }


        public PulseAudioStream(
            string identifier,
            Scope scope,
            StreamType type,
            IStreamProxy proxy,
            ILogger<PulseAudioStream> logger
        )
        {
            Identifier = identifier;
            Scope = scope;
            Type = type;
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

        private async Task<uint> GetVolumeAsyncInternal()
        {
            uint[] volume = await _proxy.GetVolumeAsync();
            return volume.First();
        }

        public Task SetVolumeAsync(uint value)
        {
            AssertProxyIsAlive();
            return SetVolumeAsyncInternal(value);
        }

        private async Task SetVolumeAsyncInternal(uint value)
        {
            await _proxy.SetVolumeAsync(new uint[1] { value.ToRatioUInt16() });
        }

        public Task ToggleMuteAsync(bool value)
        {
            AssertProxyIsAlive();
            return ToggleMuteAsyncInternal(value);
        }

        private async Task ToggleMuteAsyncInternal(bool value)
        {
            await _proxy.SetMuteAsync(value);
        }

        // private void Dispose(bool disposing)
        // {
        //     if (!disposedValue)
        //     {
        //         if (disposing)
        //         {

        //         }

        //         // TODO: free unmanaged resources (unmanaged objects) and override finalizer
        //         // TODO: set large fields to null
        //         disposedValue = true;
        //     }
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            // Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}