namespace Midicontrol.Midi
{
    public interface IMidiMessageSinkArgs
    {
        string Action { get; }
        string Destination { get; }
        int Value { get; }
    }

    internal class MidiMessageSinkArgs : IMidiMessageSinkArgs
    {
        public string Action { get; }
        public string Destination { get; }
        public int Value { get; }
        public MidiMessageSinkArgs(string action, string destination, int value)
        {
            Action = action;
            Destination = destination;
            Value = value;
        }
    }
}