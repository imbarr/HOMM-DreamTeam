using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using HoMM;

namespace Homm.Client
{
    static class Convertation
    {
        private static readonly Tuple<Direction, Point>[] conformity =
        { 
            Tuple.Create(Direction.LeftDown, new Point(-1, 1)),
            Tuple.Create(Direction.LeftUp, new Point(-1, 0)),
            Tuple.Create(Direction.Up, new Point(0, -1)),
            Tuple.Create(Direction.RightUp, new Point(1, 0)),
            Tuple.Create(Direction.RightDown, new Point(1, 1)),
            Tuple.Create(Direction.Down, new Point(0, 1))
        };

        public static Point ToDelta(this Direction direction)
        {
            return conformity.First(t => t.Item1 == direction).Item2;
        }

        public static Direction ToDirection(this Point delta)
        {
            var tuple = conformity.First(t => t.Item2 == delta);
            if (tuple != null)
                return tuple.Item1;
            throw new ArgumentException();
        }
    }
}
