
using MediatR;
using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    public class PulseAudioConnectionNotification : INotification
    {
        public bool Connected { get; }
        public Connection? Connection { get; }

        public PulseAudioConnectionNotification(bool connected, Connection? connection)
        {
            Connected = connected;
            Connection = connection;
        }
    }
}