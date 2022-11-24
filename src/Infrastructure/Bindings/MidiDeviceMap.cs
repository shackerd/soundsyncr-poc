namespace Midicontrol.Infrastructure.Bindings
{
    public class MidiDeviceMap
    {
        public string DeviceName { get; set; }
        public List<MidiSinkMap> Sinks { get; set; }
    }
}