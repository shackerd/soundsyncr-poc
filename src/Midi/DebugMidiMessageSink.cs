using Microsoft.Extensions.Logging;
using Midicontrol.Infrastructure.Bindings;

namespace Midicontrol.Midi
{
    public class DebugMidiMessageSink : IMidiMessageSink
    {
        private readonly ILogger<DebugMidiMessageSink> _logger;
        private IEnumerable<MidiBinding> _bindings;

        public DebugMidiMessageSink(ILogger<DebugMidiMessageSink> logger)
        {
            _logger = logger;            
        }

        public string Name => "Debug Sink";

        public Task InitializeAsync(IEnumerable<MidiBinding> bindings)
        {
            _bindings = bindings;

            return Task.CompletedTask;
        }

        public Task ProcessMessageAsync(MidiMessage message)
        {
            _logger.LogDebug(message.ToString());
            return Task.CompletedTask;
        }
    }
}