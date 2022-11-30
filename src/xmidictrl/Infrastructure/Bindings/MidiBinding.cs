namespace Midicontrol.Infrastructure.Bindings
{
    public class MidiBinding
    {
        public int Controller { get; set; }
        public List<MidiSinkParam> Params { get; set; }
    }
}