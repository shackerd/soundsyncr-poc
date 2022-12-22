namespace Midicontrol.Midi.NativeSinks.VolumeControl
{
    internal interface IVolumeEventSource
    {
        DropOutStack<VolumeEvent> GetEvents(int hashcode);
        void Publish(int hashcode, VolumeEvent @event);
        void RemoveSource(int hashcode);
    }

    internal class VolumeEventSource : IVolumeEventSource
    {
        private readonly Dictionary<int, DropOutStack<VolumeEvent>> _source =
            new Dictionary<int, DropOutStack<VolumeEvent>>();

        private readonly object _state = new Object();
        private const uint _stackSize = 0x80; // 0x7F (127) << 1

        public VolumeEventSource() { }

        public DropOutStack<VolumeEvent> GetEvents(int hashcode)
        {
            if (!_source.TryGetValue(hashcode, out var stack))
            {
                DropOutStack<VolumeEvent> newStack = new(_stackSize);
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
            if (!_source.ContainsKey(hashcode)) return;

            _source.Remove(hashcode);
        }
    }
}