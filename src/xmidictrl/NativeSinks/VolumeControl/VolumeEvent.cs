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

    internal class DropOutStack<T>
    {
        private T[] _items;
        private readonly uint _size;
        private uint _cursor = 0;
        private readonly object _state = new Object();

        public DropOutStack(uint capacity)
        {
            _size = capacity;
            _items = new T[_size];
        }

        public DropOutStack(IEnumerable<T> collection) : this((uint)collection.Count())
        {
            Array.Copy(collection.ToArray(), _items, _size);
        }

        public void Push(T item)
        {
            lock (_state)
            {
                _items[_cursor] = item;
                _cursor = ++_cursor % _size;
            }
        }

        public T Pop()
        {
            lock (_state)
            {
                _cursor = (_size + --_cursor) % _size;
                return _items[_cursor];
            }
        }

        public T Peek()
        {
            lock (_state)
            {
                uint cursor = (_size + _cursor - 1) % _size; // does not affect stack's cursor
                return _items[cursor];
            }
        }

        public static DropOutStack<T> operator +(DropOutStack<T> stack, T item)
        {
            stack.Push(item);
            return stack;
        }
    }
}