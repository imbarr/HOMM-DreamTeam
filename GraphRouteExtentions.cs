using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoMM.ClientClasses;

namespace Homm.Client
{
    class GraphRouteExtentions
    {
        public static List<Node> FindPath(Node start, Node finish)
        {
            //TODO: реализовать поиск пути
            throw new NotImplementedException();
        }

        public static List<Node> FindPathToClosest(Graph graph, Node start, Func<MapObjectData, bool> isValidTarget)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
