using Microsoft.Extensions.Logging;
using PortMidi;

namespace Midicontrol.Midi
{
    public interface IMidiDeviceListenerFactory
    {
        IMidiDeviceListener Create(MidiDeviceInfo device);
    }

    internal class MidiDeviceListenerFactory : IMidiDeviceListenerFactory
    {
        private readonly SynchronizationContext _synchronizationContext;
        private readonly IEnumerable<IMidiMessageSink> _sinks;
        private readonly ILogger<IMidiDeviceListener> _logger;

        public MidiDeviceListenerFactory(
            SynchronizationContext synchronizationContext,
            IEnumerable<IMidiMessageSink> sinks,
            ILogger<IMidiDeviceListener> logger
        )
        {
            _synchronizationContext = synchronizationContext;
            _sinks = sinks;
            _logger = logger;
        }

        public IMidiDeviceListener Create(PortMidi.MidiDeviceInfo device)
        {
            return new MidiDeviceListener(device, _synchronizationContext, _sinks, _logger);
        }
    }
}