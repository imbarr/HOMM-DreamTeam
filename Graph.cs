using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HoMM;
using HoMM.ClientClasses;

namespace Homm.Client
{
    class Graph
    {
        private readonly Node[,] graph;
        public readonly int Width;
        public readonly int Height;

        public Graph(MapData map)
        {
            Width = map.Width;
            Height = map.Height;
            graph = new Node[Width, Height];

            foreach (var mapObject in map.Objects)
            {
                var x = mapObject.Location.X;
                var y = mapObject.Location.Y;
                if (mapObject.Wall != null)
                    continue;

                graph[x, y] = new Node(mapObject);
                for (var dx = -1; dx <= 1; dx++)
                    for (var dy = -1; dy <= 1; dy++)
                        if (Convertation.ToDirection(new Point(dx, dy), x) != null
                            && InBounds(x + dx, y + dy)
                            && graph[x + dx, y + dy] != null)
                        {
                            graph[x, y].Connect(graph[x + dx, y + dy]);
                        }
            }
        }

        public bool InBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public Node this[int x, int y]
        {
            get
            {
                if (InBounds(x, y))
                    return graph[x, y];
                throw new IndexOutOfRangeException();
            }
        }
    }
}
