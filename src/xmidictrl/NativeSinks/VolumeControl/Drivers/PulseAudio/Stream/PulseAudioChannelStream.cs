using Microsoft.Extensions.Logging;
using Midicontrol.Midi.NativeSinks.PulseAudio.DBus;
using Midicontrol.Midi.NativeSinks.VolumeControl;
using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal sealed class PulseAudioChannelStream : IPulseAudioStream
    {

        // private bool disposedValue;
        private readonly ILogger<PulseAudioChannelStream> _logger;
        private readonly IAudioStream _rootStream;
        private readonly IStreamProxy _proxy;

        public Scope Scope => Scope.Channel;

        public StreamType Type { get; }

        public string Identifier { get; }
        public ObjectPath ObjectPath { get; }

        public IAudioStream? Root => _rootStream;

        public PulseAudioChannelStream(
            IAudioStream rootStream,
            string identifier,
            ObjectPath objectPath,
            StreamType type,
            IStreamProxy proxy,
            ILogger<PulseAudioChannelStream> logger
        )
        {
            _rootStream = rootStream;
            Identifier = identifier;
            ObjectPath = objectPath;
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

            if (Type == StreamType.Record)
            {
                // Record stream does not have mute method
                return Task.CompletedTask;
            }

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