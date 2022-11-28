using Microsoft.Extensions.Logging;

namespace Midicontrol.Midi.NativeSinks
{
    internal class VolumeControlSink : IMidiMessageSink
    {
        private readonly ILogger _logger;

        public string Name => "VolumeControl";

        public VolumeControlSink(ILogger<VolumeControlSink> logger)
        {
            _logger = logger;
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public Task ProcessMessageAsync(IEnumerable<IMidiMessageSinkArgs> args)
        {
            throw new NotImplementedException();
        }
    }

    interface IMidiDevice {
        MidiDeviceCapability Capability { get; } // if both : TwoWay binding, else OneWay
    }

    internal enum MidiDeviceCapability
    {
        Write,
        Read
    }

    // MS / PA same contract interface
    interface IAudioDriver
    {
        Task InitializeAsync();
        Task<IEnumerable<IAudioStream>> GetStreams(string destination);
    }

    interface IAudioStream
    {
        Task<uint> GetVolumeAsync();
        Task SetVolumeAsync(uint value);
        Task ToggleMuteAsync(bool value);
        Task ToggleSoloAsync(bool value);
    }
}