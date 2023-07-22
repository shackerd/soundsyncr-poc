using MediatR;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal class PulseAudioStreamStoreQueryRequest : IRequest<IEnumerable<IPulseAudioStream>>
    {

        public PulseAudioStreamStoreQueryRequest()
        {

        }
    }
}