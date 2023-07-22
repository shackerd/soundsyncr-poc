namespace Midicontrol.Midi.NativeSinks.VolumeControl
{
    internal class VolumeEvent
    {
        DateTime On { get; }
        bool Managed { get; }
        uint Value { get; }

        public VolumeEvent(bool managed, uint value)
        {
            On = DateTime.UtcNow;
            Managed = managed;
            Value = value;
        }
    }
    

    internal class VolumeCommand
    {

    }
}