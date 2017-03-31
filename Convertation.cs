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
        private static readonly Tuple<Direction, Point>[] conformityEven =
        { 
            Tuple.Create(Direction.LeftDown, new Point(-1, 0)),
            Tuple.Create(Direction.LeftUp, new Point(-1, -1)),
            Tuple.Create(Direction.Up, new Point(0, -1)),
            Tuple.Create(Direction.RightUp, new Point(1, -1)),
            Tuple.Create(Direction.RightDown, new Point(1, 0)),
            Tuple.Create(Direction.Down, new Point(0, 1))
        };

        private static readonly Tuple<Direction, Point>[] conformityOdd =
        {
            Tuple.Create(Direction.LeftDown, new Point(-1, 1)),
            Tuple.Create(Direction.LeftUp, new Point(-1, 0)),
            Tuple.Create(Direction.Up, new Point(0, -1)),
            Tuple.Create(Direction.RightUp, new Point(1, 0)),
            Tuple.Create(Direction.RightDown, new Point(1, 1)),
            Tuple.Create(Direction.Down, new Point(0, 1))
        };

        public static Point ToDelta(this Direction direction, int x)
        {
            throw new NotImplementedException();
        }

        public static Direction? ToDirection(Point delta, int x)
        {
            if (x % 2 == 0)
            {
                var tuple = conformityEven.FirstOrDefault(t => t.Item2 == delta);
                if (tuple != null)
                    return tuple.Item1;
                return null;
            }
            else
            {
                var tuple = conformityOdd.FirstOrDefault(t => t.Item2 == delta);
                if (tuple != null)
                    return tuple.Item1;
                return null;
            }
        }

        public static List<Direction> ToDirectionList(IEnumerable<Node> path)
        {
            var result = new List<Direction>();
            var previous = path.First();
            foreach (var node in path.Skip(1))
            {
                var dx = node.mapObjectData.Location.X - previous.mapObjectData.Location.X;
                var dy = node.mapObjectData.Location.Y - previous.mapObjectData.Location.Y;
                result.Add((Direction)ToDirection(new Point(dx, dy), previous.mapObjectData.Location.X));
                previous = node;
            }
            return result;
        }
    }
}
