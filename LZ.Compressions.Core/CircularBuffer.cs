using System.Collections.Generic;

namespace LZ.Compressions.Core
{
    public class CircularBuffer
    {
        int capacity;
        List<char> items;

        public int Length => items.Count;

        public char this[int key] => items[key];

        public CircularBuffer(int length)
        {
            items = new List<char>();
            capacity = length;
        }

        public CircularBuffer(int length, string value)
        {
            items = new List<char>();
            capacity = length;
            Add(value);
        }

        public void Add(char ch)
        {
            if (items.Count == capacity)
                items.RemoveAt(0);
            items.Add(ch);
        }

        public void Add(string value)
        {
            foreach (var ch in value)
                Add(ch);
        }

        public void Remove(int count)
        {
            items.RemoveRange(0, count);
        }

        public override string ToString() =>
            string.Join(string.Empty, items);
    }
}
