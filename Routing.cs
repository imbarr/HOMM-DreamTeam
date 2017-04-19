using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoMM.ClientClasses;

namespace Homm.Client
{
    static class Routing
    {
        public static Dictionary<Node, NodePathInfo> GetGraphPathInfo(Node start, Func<Node, bool> IsObstacle)
        {
            var result = new Dictionary<Node, NodePathInfo>
            {
                {start, new NodePathInfo(new LinkedSequence<Node>(start), 0.0)}
            };
            RegisterAllNeighbours(start, result, IsObstacle);
            return result;
        }

        private static void RegisterAllNeighbours(Node node, Dictionary<Node, NodePathInfo> GraphPathInfo, Func<Node, bool> IsObstacle)
        {
            if (IsObstacle(node))
                return;
            var nodePathInfo = GraphPathInfo[node];
            foreach (var neighbour in node.IncidentNodes)
            {
                var newTravelTime = nodePathInfo.TravelTime + neighbour.Weight;
                if (!GraphPathInfo.ContainsKey(neighbour) || GraphPathInfo[neighbour].TravelTime > newTravelTime)
                {
                    var newPath = new LinkedSequence<Node>((LinkedSequence<Node>)nodePathInfo.Path);
                    newPath.AddFirst(neighbour);
                    GraphPathInfo[neighbour] = new NodePathInfo(newPath, newTravelTime);
                    RegisterAllNeighbours(neighbour, GraphPathInfo, IsObstacle);
                }
            }
        }

        public static IEnumerable<Node> FindPathToClosest(Node lastNode, Dictionary<Node, NodePathInfo> graphPathInfo, Func<Node, bool> isTarget)
        {
            NodePathInfo minNodePathInfo = null;
            Node minNode = null;
            foreach (var pair in graphPathInfo.Where(p => isTarget(p.Key)))
            {
                if (minNodePathInfo == null || pair.Value.TravelTime < minNodePathInfo.TravelTime)
                {
                    minNodePathInfo = pair.Value;
                    minNode = pair.Key;
                }
            }
            return minNodePathInfo == null ? null : minNodePathInfo.Path;
        }
    }
}
