using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homm.Client
{
    class LinkedSequence<T> : IEnumerable<T>
    {
        private LinkedElement<T> First;
        public LinkedSequence(T value)
        {
            First = new LinkedElement<T>(value);
        }

        public LinkedSequence(LinkedSequence<T> other)
        {
            First = other.First;
        }

        public void Add(T value)
        {
            var newElement = new LinkedElement<T>(value);
            newElement.Next = First;
            First = newElement;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var current = First;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }
    }
}
