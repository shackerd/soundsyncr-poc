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
                MidiSinkMap sinkMap = _maps.FirstOrDefault(s => s.Name == sink.Name);

                await sink.InitializeAsync().ConfigureAwait(false);
            }
        }

        public Task BroadcastAsync(MidiMessage message)
        {
            return BroadcastAsyncInternal(message);
        }

        private async Task BroadcastAsyncInternal(MidiMessage message)
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
        }

        private IEnumerable<IMidiMessageSinkArgs> BuildSinkArgs(MidiMessage message, IMidiMessageSink sink)  // TryBuildSinkArgs -> bool + out param
        {
            MidiSinkMap map = _maps.FirstOrDefault(m => m.Name.Equals(sink.Name));

            if(map == null) 
            {
                return Enumerable.Empty<IMidiMessageSinkArgs>();
            }

            MidiBinding binding = 
                map.Bindings.FirstOrDefault(b => b.Controller == (int)message.Controller);
            
            if(binding == null)
            {
                return Enumerable.Empty<IMidiMessageSinkArgs>();
            }

            return binding.Params.Select(p => new MidiMessageSinkArgs(p.Action, p.Destination, (int)message.Value));
        }
    }

      
}