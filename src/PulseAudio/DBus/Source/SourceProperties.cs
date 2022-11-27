using Tmds.DBus;

namespace Midicontrol.PulseAudio.DBus
{
    [Dictionary]
    internal class SourceProperties
    {
        private ObjectPath _MonitorOfSink = default(ObjectPath);
        public ObjectPath MonitorOfSink
        {
            get
            {
                return _MonitorOfSink;
            }

            set
            {
                _MonitorOfSink = (value);
            }
        }
    }
}