namespace Midicontrol.Infrastructure.Bindings
{
    public class MidiSinkMap
    {
        public string Sink { get; set; }
        public List<MidiBinding> Bindings { get; set; }
    }
}