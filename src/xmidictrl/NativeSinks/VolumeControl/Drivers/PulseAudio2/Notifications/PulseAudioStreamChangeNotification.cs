using MediatR;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal class PulseAudioStreamChangeNotification : INotification
    {
        public IPulseAudioStream Stream { get; }
        public PulseAudioStreamChangeNotificationType Type { get; }

        public PulseAudioStreamChangeNotification(IPulseAudioStream stream, PulseAudioStreamChangeNotificationType type)
        {
            Stream = stream;
            Type = type;
        }
    }
}