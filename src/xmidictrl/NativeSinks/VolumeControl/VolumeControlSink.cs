using Microsoft.Extensions.Logging;

namespace Midicontrol.Midi.NativeSinks
{
    internal class VolumeControlSink : IMidiMessageSink
    {
        private readonly ILogger _logger;
        private readonly IAudioDriver _driver;

        public string Name => "VolumeControl";

        public VolumeControlSink(ILogger<VolumeControlSink> logger, IAudioDriver driver)
        {
            _logger = logger;
            _driver = driver;
        }

        public Task InitializeAsync()
        {
            // if IMidiDevice.Capability == Both (Twoway) -> Write to mididevice volume of streams
            throw new NotImplementedException();
        }

        public async Task ProcessMessageAsync(IEnumerable<IMidiMessageSinkArgs> args)
        {
            foreach (IMidiMessageSinkArgs arg in args)
            {
                VolumeControlAction action = arg.Action;

                IEnumerable<IAudioStream> streams =
                    _driver
                        .GetStreamsAsync(arg.Destination)
                        .Result
                        .Where(s => s.Scope == action.Scope && s.Type == action.StreamType);

                switch (action.ActionType)
                {
                    case ActionType.Bind:
                        await Task.WhenAll(streams.Select(s => s.SetVolumeAsync((uint)arg.Value)));
                        break;
                    case ActionType.Mute:
                        await Task.WhenAll(streams.Select(s => s.ToggleMuteAsync(arg.Value != 0)));
                        break;
                    case ActionType.Solo:
                        await _driver.ToggleSoloAsync(streams.Single(), arg.Value != 0);
                        break;
                }
            }
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
}