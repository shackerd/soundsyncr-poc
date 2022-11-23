using Microsoft.Extensions.Logging;
using Midicontrol.Midi;

namespace Midicontrol.PulseAudio
{
    internal class PulseAudioMidiSink : IMidiMessageSink
    {
        private readonly PulseAudioClient _client;
        private readonly Dictionary<int, string> _ctrl;
        private readonly ILogger _logger;

        public PulseAudioMidiSink(PulseAudioClient client, ILogger<PulseAudioMidiSink> logger)
        {
            _client = client;
            _logger = logger;

            _ctrl = new Dictionary<int, string>();
            _ctrl.Add(2, "chrome");
            _ctrl.Add(1, "teams");
            _ctrl.Add(3, "firefox");   
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

            if (_ctrl.ContainsKey((int)message.Controller))
            {
                string binary = _ctrl[(int)message.Controller];

                var stream = 
                    _client
                        .StreamStore
                        .PlaybackStreams
                        .FirstOrDefault(_ => _.Binary.StartsWith(binary));
                
                if (stream != null)
                {
                    await stream
                        .SetVolumeAsync(value);

                    uint percentage = (uint)((double)((double)value / (double)0xFFFF) * 100);
                    _logger.LogDebug($"{stream.Binary} volume {percentage}%");                            
                }
                
            }    
        }
    }
}