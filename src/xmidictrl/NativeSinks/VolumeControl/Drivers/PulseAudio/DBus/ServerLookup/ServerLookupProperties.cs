using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio.DBus
{
    [Dictionary]
    class ServerLookupProperties
    {
        private string? _Address = default(string);
        public string? Address
        {
            get
            {
                return _Address;
            }

            set
            {
                _Address = (value);
            }
        }
    }
}