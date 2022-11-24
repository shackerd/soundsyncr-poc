using Midicontrol.Infrastructure.Bindings;

namespace Midicontrol.Midi
{
    public interface IMidiMessageSink
    {
        Task InitializeAsync(IEnumerable<MidiBinding> bindings);
        Task ProcessMessageAsync(MidiMessage message);
        string Name { get; }
    }
}