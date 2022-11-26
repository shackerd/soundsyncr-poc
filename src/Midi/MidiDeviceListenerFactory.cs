using Microsoft.Extensions.Logging;
using Midicontrol.Infrastructure.Bindings;
using PortMidi;

namespace Midicontrol.Midi
{
    public interface IMidiDeviceListenerFactory
    {
        IMidiDeviceListener Create(MidiDeviceInfo device);
        IMidiDeviceListener CreateDebug(PortMidi.MidiDeviceInfo device);
    }

    // missing map device -> sinks
    internal class MidiDeviceListenerFactory : IMidiDeviceListenerFactory
    {
        private readonly SynchronizationContext _synchronizationContext;
        private readonly IEnumerable<IMidiMessageSink> _sinks;
        private readonly ILogger<IMidiDeviceListener> _listenerLogger;
        private readonly ILogger<IMidiMessageDispatcher> _dispatcherLogger;
        private readonly IMidiListenerStore _store;
        private readonly ConfigMap _bindingMap;

        public MidiDeviceListenerFactory(
            SynchronizationContext synchronizationContext,
            IEnumerable<IMidiMessageSink> sinks,
            ILogger<IMidiDeviceListener> listenerLogger,
            ILogger<IMidiMessageDispatcher> dispatcherLogger,
            IMidiListenerStore store,
            ConfigMap bindingMap
        )
        {
            _synchronizationContext = synchronizationContext;
            _sinks = sinks;
            _listenerLogger = listenerLogger;
            _dispatcherLogger = dispatcherLogger;
            _store = store;
            _bindingMap = bindingMap;
        }

        public IMidiDeviceListener Create(PortMidi.MidiDeviceInfo device)
        {

            var map = _bindingMap.DevicesMap.FirstOrDefault(d => d.DeviceName.Equals(device.Name));
            var sinks = _sinks.Where(s => map.Sinks.Any(m => m.Name.Equals(s.Name)));

            IMidiMessageDispatcher dispatcher =
                new MidiMessageDispatcher(_dispatcherLogger, sinks, map.Sinks, _synchronizationContext);

            IMidiDeviceListener listener = new MidiDeviceListener(device, _listenerLogger, dispatcher);
            
            if(!_store.TryAdd(listener)){
                throw new Exception("Cannot register listener");
            }

            return listener;
        }

        public IMidiDeviceListener CreateDebug(PortMidi.MidiDeviceInfo device)
        {
            IMidiMessageDispatcher dispatcher =
                new DebugMidiMessageDispatcher(_dispatcherLogger);

            IMidiDeviceListener listener = new MidiDeviceListener(device, _listenerLogger, dispatcher);
            
            if(!_store.TryAdd(listener)){
                throw new Exception("Cannot register listener");
            }

            return listener;
        }
    }
}