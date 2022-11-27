using Microsoft.Extensions.Logging;

namespace Midicontrol.Midi
{
    public interface IMidiListenerStore
    {
        IMidiDeviceListener GetListener(string deviceName);
        bool TryAdd(IMidiDeviceListener listener);
        IEnumerable<IMidiDeviceListener> GetActiveListeners();
    }

    public class MidiListenerStore : IMidiListenerStore
    {
        private readonly ILogger _logger;
        private IDictionary<string, IMidiDeviceListener> _listeners;

        public MidiListenerStore(ILogger<MidiListenerStore> logger)
        {
            _logger = logger;
            _listeners = new Dictionary<string, IMidiDeviceListener>();
        }

        public IEnumerable<IMidiDeviceListener> GetActiveListeners()
        {
            return _listeners.Values.Where(l => l.IsListening);
        }

        public IMidiDeviceListener GetListener(string deviceName)
        {
            if (_listeners.TryGetValue(deviceName, out IMidiDeviceListener listener))
            {
                return listener;
            }
            else
            {
                throw new Exception("No associated devices");
            }
        }

        public bool TryAdd(IMidiDeviceListener listener)
        {
            return _listeners.TryAdd(listener.DeviceName, listener);
        }
    }
}