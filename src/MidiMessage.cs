namespace Midicontrol
{
    public class MidiMessage
    {
        public DateTime PublishedOn { get; }
        public long Controller { get; }
        public long Value { get; }
        public MidiMessageType Type { get; }

        private MidiMessage(long controller, long value, long status)
        {
            Controller = controller;
            Value = value;
            Type = (MidiMessageType)status;
            PublishedOn = DateTime.Now;
        }

        public static explicit operator MidiMessage(PortMidi.Event @event){
            return new MidiMessage(@event.Date1, @event.Data2, @event.Status);
        }

        public override string ToString()
        {
            return $"{PublishedOn.ToString("yyyy-MM-dd HH:mm:ss")}, {Type}, {Controller}, {Value}";
        }
    }
}