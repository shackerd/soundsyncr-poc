namespace Midicontrol.Midi.NativeSinks.VolumeControl
{
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