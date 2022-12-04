using MediatR;
using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal class PulseAudioStreamGetRequest : IRequest<IPulseAudioStream>
    {
        public ObjectPath Path { get; }
        public Scope Scope { get; }
        public StreamType Type { get; }

        public PulseAudioStreamGetRequest(ObjectPath path, Scope scope, StreamType type)
        {
            Path = path;
            Scope = scope;
            Type = type;
        }

    }
}