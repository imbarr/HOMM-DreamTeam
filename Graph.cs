using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoMM;
using HoMM.ClientClasses;

namespace Homm.Client
{
    class Graph
    {
        private readonly Node[,] graph;
        public Graph(MapData map)
        {
            graph = new Node[map.Width, map.Height];
            foreach (var mapObject in map.Objects)
            {
                var x = mapObject.Location.X;
                var y = mapObject.Location.Y;
                if (mapObject.Wall == null)
                {
                    graph[x, y] = new Node(mapObject);
                    for (var dx = -1; dx <= 1; dx++)
                    for (var dy = -1; dy <= 1; dy++)
                        if (dy != dx && x + dx >= 0 && x + dx < map.Width
                            && y + dy >= 0 && y + dy < map.Height && graph[x + dx, y + dy] != null)
                                graph[x, y].Connect(graph[x + dx, y + dy]);
                }
            }
        }
    }
}
