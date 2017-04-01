using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homm.Client
{
    class LinkedElement<T>
    {
        public readonly T Value;
        public LinkedElement<T> Next;

        public LinkedElement(T value)
        {
            Value = value;
        }
    }
}
