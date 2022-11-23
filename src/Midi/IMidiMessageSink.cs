namespace Midicontrol.Midi
{
    public interface IMidiMessageSink
    {
        Task ProcessMessageAsync(MidiMessage message);
    }
}