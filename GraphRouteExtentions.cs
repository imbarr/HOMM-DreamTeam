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
        public static Dictionary<Node, NodePathInfo> GetGraphPathInfo(Node start)
        {
            var result = new Dictionary<Node, NodePathInfo>();
            var currentPath = new LinkedList<Node>();
            currentPath.AddLast(start);
            result.Add(start, new NodePathInfo(currentPath, 0.0));
            RegisterAllNeighbours(start, currentPath, result);
            return result;
        }

        private static void RegisterAllNeighbours(Node node, LinkedList<Node> path,
            Dictionary<Node, NodePathInfo> GraphPathInfo)
        {
            foreach (var neighbour in node.incidentNodes)
            {
                var newTravelTime = GraphPathInfo[node].TravelTime + neighbour.weight;
                if (!GraphPathInfo.ContainsKey(neighbour) || GraphPathInfo[neighbour].TravelTime > newTravelTime)
                {
                    var newPath = new LinkedList<Node>(GraphPathInfo[node].Path);
                    newPath.AddLast(neighbour);
                    GraphPathInfo[neighbour] = new NodePathInfo(newPath, newTravelTime);
                    RegisterAllNeighbours(neighbour, path, GraphPathInfo);
                }
            }
        }

        public static LinkedList<Node> FindPathToClosest(Dictionary<Node, NodePathInfo> graphPathInfo, Func<Node, bool> isValidTarget)
        {
            NodePathInfo minNodePathInfo = null;
            foreach (var nodePathInfo in graphPathInfo.Where(p => isValidTarget(p.Key)))
            {
                if (nodePathInfo.Value.TravelTime != 0 && (minNodePathInfo == null || nodePathInfo.Value.TravelTime < minNodePathInfo.TravelTime))
                    minNodePathInfo = nodePathInfo.Value;
            }
            return minNodePathInfo.Path;
        }
    }
}
