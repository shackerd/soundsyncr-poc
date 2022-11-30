using Microsoft.Extensions.Logging;
using Midicontrol.Infrastructure.Bindings;

namespace Midicontrol.Midi
{
    public class DebugMidiMessageSink : IMidiMessageSink
    {
        private readonly ILogger<DebugMidiMessageSink> _logger;

        public DebugMidiMessageSink(ILogger<DebugMidiMessageSink> logger)
        {
            _logger = logger;            
        }

        public string Name => "Debug";

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public Task ProcessMessageAsync(IEnumerable<IMidiMessageSinkArgs> args)
        {
            foreach (var arg in args)
            {
                _logger.LogDebug($"{arg.Action}->{arg.Destination}:{arg.Value}");
            }
            return Task.CompletedTask;
        }
    }    
}