using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoMM.ClientClasses;

namespace Homm.Client
{
    class GraphRouteExtentions
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
            var nodePathInfo = GraphPathInfo[node];
            foreach (var neighbour in node.IncidentNodes.Where(n => !IsObstacle(n)))
            {
                var newTravelTime = nodePathInfo.TravelTime + neighbour.Weight;
                if (!GraphPathInfo.ContainsKey(neighbour) || GraphPathInfo[neighbour].TravelTime > newTravelTime)
                {
                    var newPath = new LinkedSequence<Node>((LinkedSequence<Node>)nodePathInfo.Path);
                    newPath.Add(neighbour);
                    GraphPathInfo[neighbour] = new NodePathInfo(newPath, newTravelTime);
                    RegisterAllNeighbours(neighbour, GraphPathInfo, IsObstacle);
                }
            }
        }

        public static IEnumerable<Node> FindPathToClosest(Node lastNode, Dictionary<Node, NodePathInfo> graphPathInfo, Func<Node, bool> isTarget)
        {
            NodePathInfo minNodePathInfo = null;
            foreach (var nodePathInfo in graphPathInfo.Where(p => isTarget(p.Key)).Select(p => p.Value))
            {
                if (nodePathInfo.TravelTime != 0
                    && (minNodePathInfo == null
                    || nodePathInfo.TravelTime < minNodePathInfo.TravelTime))
                {
                    minNodePathInfo = nodePathInfo;
                }
            }
            return minNodePathInfo == null ? null : minNodePathInfo.Path;
        }
        /*public static IEnumerable<Node> FindSafeNode(Node start)
        {
            var visited = new HashSet<Node>();
            var queue = new Queue<Node>();
            queue.Enqueue(start);
            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                if (node.MapObjectData.NeutralArmy != null)
                {
                    visited.Add(node);
                }
                if (visited.Contains(node))
                {
                    continue;
                }
                visited.Add(node);
                if (node.MapObjectData.ResourcePile != null
                    || node.MapObjectData.Dwelling != null && node.MapObjectData.Dwelling.Owner != "Left"
                    || node.MapObjectData.Mine != null && node.MapObjectData.Mine.Owner != "Left")
                {
                    yield return node;
                }
                foreach (var incidentNode in node.IncidentNodes)
                    queue.Enqueue(incidentNode);
            }
        }*/
    }
}
