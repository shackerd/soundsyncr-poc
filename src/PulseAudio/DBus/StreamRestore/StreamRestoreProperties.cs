using Tmds.DBus;

namespace Midicontrol.PulseAudio.DBus
{
    [Dictionary]
    class StreamRestoreProperties
    {
        private uint _InterfaceRevision = default(uint);
        public uint InterfaceRevision
        {
            get
            {
                return _InterfaceRevision;
            }

            set
            {
                _InterfaceRevision = (value);
            }
        }

        private ObjectPath[] _Entries = default(ObjectPath[]);
        public ObjectPath[] Entries
        {
            get
            {
                return _Entries;
            }

            set
            {
                _Entries = (value);
            }
        }
    }
}