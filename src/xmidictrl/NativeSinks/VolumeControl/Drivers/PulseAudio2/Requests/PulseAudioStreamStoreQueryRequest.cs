using MediatR;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal class PulseAudioStreamStoreQueryRequest : IRequest<IQueryable<IPulseAudioStream>>
    {

        public PulseAudioStreamStoreQueryRequest()
        {

        }
    }
}