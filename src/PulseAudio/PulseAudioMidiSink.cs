using Microsoft.Extensions.Logging;
using Midicontrol.Infrastructure.Bindings;
using Midicontrol.Midi;

namespace Midicontrol.PulseAudio
{
    internal class PulseAudioMidiSink : IMidiMessageSink
    {
        private readonly PulseAudioClient _client;
        private readonly ILogger _logger;

        private IEnumerable<MidiBinding> _bindings;

        public string Name => "Pulse Audio";

        public PulseAudioMidiSink(PulseAudioClient client, ILogger<PulseAudioMidiSink> logger)
        {
            _client = client;
            _logger = logger;
        }

        public Task ProcessMessageAsync(MidiMessage message){

            return ProcessMessageAsyncInternal(message);
        }

        private async Task ProcessMessageAsyncInternal(MidiMessage message)
        {
            uint value = PulseAudioMidiValueConverter.FromCCValueToVolume((uint)message.Value);

            if(!_client.StreamStore.PlaybackStreams.Any())
            {
                return;
            }

            IEnumerable<PulseAudioStream> streams = 
                GetStreams((int)message.Controller);
                
            if (streams != null)
            {
                foreach (PulseAudioStream stream in streams)
                {
                    await stream
                        .SetVolumeAsync(value);

                    uint percentage = (uint)((double)((double)value / (double)0xFFFF) * 100);
                    _logger.LogDebug($"Pulse Audio : {stream.Binary} volume {percentage}%");                               
                }                    
            }  
        }

        private IEnumerable<PulseAudioStream> GetStreams(int controller)
        {
            var binding = 
                _bindings
                    .FirstOrDefault(b => b.Controller == controller);

            if(binding == null){
                return Enumerable.Empty<PulseAudioStream>();
            }
                
            return _client
                .StreamStore
                .PlaybackStreams
                .Where(s => binding.Params.Any(p => p.Destination.Equals(s.Binary, StringComparison.InvariantCultureIgnoreCase)));
        }

        public async Task InitializeAsync(IEnumerable<MidiBinding> bindings)
        {
            _bindings = bindings;

            if (!_client.Initialized)
            {
                await _client.ConnectAsync();
            }            
        }
    }
}