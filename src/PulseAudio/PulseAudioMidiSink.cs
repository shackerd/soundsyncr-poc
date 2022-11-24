using Microsoft.Extensions.Logging;
using Midicontrol.Infrastructure.Bindings;
using Midicontrol.Midi;

namespace Midicontrol.PulseAudio
{
    internal class PulseAudioMidiSink : IMidiMessageSink
    {
        private readonly PulseAudioClient _client;
        private readonly ILogger _logger;
        private const string _playbackStreamVolumeAction = "PlaybackStream.Volume";
        private const string _recordStreamVolumeAction = "RecordStream.Volume";

        public string Name => "Pulse Audio";

        public PulseAudioMidiSink(PulseAudioClient client, ILogger<PulseAudioMidiSink> logger)
        {
            _client = client;
            _logger = logger;
        }

        private IEnumerable<PulseAudioStream> GetStreams(string destination)
        {
            
            return _client.StreamStore.PlaybackStreams.Where(s => s.Binary.Equals(destination, StringComparison.InvariantCultureIgnoreCase));            
        }

        public async Task InitializeAsync()
        {
            if (!_client.Initialized)
            {
                await _client.ConnectAsync();
            }            
        }

        public async Task ProcessMessageAsync(IEnumerable<IMidiMessageSinkArgs> args)
        {
            foreach (var arg in args)
            {
                uint value = PulseAudioMidiValueConverter.FromCCValueToVolume((uint)arg.Value);

                if(!_client.StreamStore.PlaybackStreams.Any())
                {
                    return;
                }

                IEnumerable<PulseAudioStream> streams = 
                    GetStreams(arg.Destination);
                    
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
        }
    }
}