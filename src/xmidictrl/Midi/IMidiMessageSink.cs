using Midicontrol.Infrastructure.Bindings;

namespace Midicontrol.Midi
{
    public interface IMidiMessageSink
    {
        Task InitializeAsync();
        Task ProcessMessageAsync(IEnumerable<IMidiMessageSinkArgs> args);
        string Name { get; }
    }
}