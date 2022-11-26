namespace Midicontrol.Infrastructure.Bindings
{
    public class ConfigMap
    {
        public List<string> Modules { get; set; }
        public List<MidiDeviceMap> DevicesMap { get; set; }
    }
}