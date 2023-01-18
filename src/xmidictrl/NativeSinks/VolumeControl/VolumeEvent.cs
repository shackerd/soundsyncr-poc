namespace Midicontrol.Midi.NativeSinks.VolumeControl
{
    internal abstract class VolumeEvent
    {
        DateTime On { get; }
    }

    internal class ManagedVolumeEvent : VolumeEvent
    {

    }

    internal class UnManagedVolumeEvent : VolumeEvent
    {

    }

    internal class VolumeCommand
    {

    }

    internal class VolumeEventSource
    {
        private readonly Dictionary<int, DropOutStack<VolumeEvent>> _source =
            new Dictionary<int, DropOutStack<VolumeEvent>>();

        private readonly object _state = new Object();
        private const uint _maxStackSize = 32;

        public VolumeEventSource() { }

        public DropOutStack<VolumeEvent> GetEvents(int hashcode)
        {
            if(!_source.TryGetValue(hashcode, out var stack)){
                DropOutStack<VolumeEvent> newStack = new(_maxStackSize);
                _source.Add(hashcode, newStack);
                stack = newStack;
            }

            return stack;
        }

        public void Publish(int hashcode, VolumeEvent @event)
        {
            DropOutStack<VolumeEvent> stack = GetEvents(hashcode);

            stack += @event;
        }

        public void RemoveSource(int hashcode)
        {
            if(!_source.ContainsKey(hashcode)) return;

            _source.Remove(hashcode);
        }
    }
}