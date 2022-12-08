using Microsoft.Extensions.Logging;
using Midicontrol.Infrastructure.Bindings;

namespace Midicontrol.Midi
{
    public interface IMidiMessageDispatcher
    {
        IEnumerable<IMidiMessageSink> Sinks { get; }
        Task BroadcastAsync(MidiMessage message);
        Task InitializeAsync();
    }

    class MidiMessageDispatcher : IMidiMessageDispatcher
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<IMidiMessageSink> _sinks;
        private readonly IEnumerable<MidiSinkMap> _maps;
        private readonly SynchronizationContext _context;
        public IEnumerable<IMidiMessageSink> Sinks => _sinks;

        public MidiMessageDispatcher(
            ILogger<IMidiMessageDispatcher> logger,
            IEnumerable<IMidiMessageSink> sinks,
            IEnumerable<MidiSinkMap> maps,
            SynchronizationContext context
        )
        {
            _logger = logger;
            _sinks = sinks;
            _maps = maps;
            _context = context;
        }

        public Task InitializeAsync()
        {
            return InitializeAsyncInternal();
        }

        private async Task InitializeAsyncInternal()
        {
            foreach (IMidiMessageSink sink in _sinks)
            {
                // MidiSinkMap? sinkMap = _maps.FirstOrDefault(s => sink.Name.Equals(s.Name));

                await sink.InitializeAsync().ConfigureAwait(false);
            }
        }

        public Task BroadcastAsync(MidiMessage message)
        {
            return BroadcastAsyncInternal(message);
        }

        private Task BroadcastAsyncInternal(MidiMessage message)
        {
            _context.Post(
                async (_) =>
                {
                    foreach (IMidiMessageSink sink in _sinks)
                    {
                        IEnumerable<IMidiMessageSinkArgs> args =
                            BuildSinkArgs(message, sink);

                        if(!args.Any()){
                            continue;
                        }

                        await sink
                            .ProcessMessageAsync(args)
                            .ConfigureAwait(false);
                    }
                },
                null
            );

            return Task.CompletedTask;
        }

        private IEnumerable<IMidiMessageSinkArgs> BuildSinkArgs(MidiMessage message, IMidiMessageSink sink)  // TryBuildSinkArgs -> bool + out param
        {
            MidiSinkMap? map = _maps.FirstOrDefault(m => sink.Name.Equals(m.Name));

            if(map == null)
            {
                return Enumerable.Empty<IMidiMessageSinkArgs>();
            }

            MidiBinding? binding =
                map.Bindings?.FirstOrDefault(b => b.Controller == (int)message.Controller);

            if(binding == null || binding.Params == null)
            {
                return Enumerable.Empty<IMidiMessageSinkArgs>();
            }

            return binding
                .Params
                .Where(p => p.Action != null && p.Destination != null)
                .Select(p => new MidiMessageSinkArgs(p.Action!, p.Destination!, (int)message.Value))
                ?? Enumerable.Empty<IMidiMessageSinkArgs>();
        }
    }

    internal class DebugMidiMessageDispatcher : IMidiMessageDispatcher
    {
        private readonly ILogger _logger;

        public IEnumerable<IMidiMessageSink> Sinks =>  Enumerable.Empty<IMidiMessageSink>();

        public DebugMidiMessageDispatcher(ILogger<IMidiMessageDispatcher> logger)
        {
            _logger = logger;
        }

        public Task BroadcastAsync(MidiMessage message)
        {
            _logger.LogInformation(message.ToString());
            return Task.CompletedTask;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}