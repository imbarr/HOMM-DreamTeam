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
        private static readonly Tuple<bool, Direction, Point>[] conformity =
        { 
            Tuple.Create(true, Direction.LeftDown, new Point(-1, 0)),
            Tuple.Create(true, Direction.LeftUp, new Point(-1, -1)),
            Tuple.Create(true, Direction.Up, new Point(0, -1)),
            Tuple.Create(true, Direction.RightUp, new Point(1, -1)),
            Tuple.Create(true, Direction.RightDown, new Point(1, 0)),
            Tuple.Create(true, Direction.Down, new Point(0, 1)),
            Tuple.Create(false, Direction.LeftDown, new Point(-1, 1)),
            Tuple.Create(false, Direction.LeftUp, new Point(-1, 0)),
            Tuple.Create(false, Direction.Up, new Point(0, -1)),
            Tuple.Create(false, Direction.RightUp, new Point(1, 0)),
            Tuple.Create(false, Direction.RightDown, new Point(1, 1)),
            Tuple.Create(false, Direction.Down, new Point(0, 1))
        };

        public static Point ToDelta(this Direction direction, int x)
        {
            return conformity.First(t => t.Item1 == (x % 2 == 0) && t.Item2 == direction).Item3;
        }

        public static Direction? ToDirection(Point delta, int x)
        {
            var tuple = conformity.FirstOrDefault(t => 
                t.Item1 == (x % 2 == 0) && t.Item3 == delta);
            return tuple == null ? null : (Direction?)tuple.Item2;
        }

        public static List<Direction> ToDirectionList(IEnumerable<Node> path)
        {
            var copy = path.ToList();
            var result = new List<Direction>();
            var previous = copy.First();
            foreach (var node in copy.Skip(1))
            {
                var dx = node.Coords.X - previous.Coords.X;
                var dy = node.Coords.Y - previous.Coords.Y;
                var direction = ToDirection(new Point(dx, dy), previous.Coords.X);
                if(direction == null)
                    throw new ArgumentException();
                result.Add((Direction)direction);
                previous = node;
            }
            return result;
        }
    }
}
