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
            var result = new Dictionary<Node, NodePathInfo>
            {
                {start, new NodePathInfo(new LinkedSequence<Node>(start), 0.0)}
            };
            RegisterAllNeighbours(start, result);
            return result;
        }

        private static void RegisterAllNeighbours(Node node, Dictionary<Node, NodePathInfo> GraphPathInfo)
        {
            var nodePathInfo = GraphPathInfo[node];
            foreach (var neighbour in node.IncidentNodes)
            {
                var newTravelTime = nodePathInfo.TravelTime + neighbour.Weight;
                if (!GraphPathInfo.ContainsKey(neighbour) || GraphPathInfo[neighbour].TravelTime > newTravelTime)
                {
                    var newPath = new LinkedSequence<Node>((LinkedSequence<Node>)nodePathInfo.Path);
                    newPath.Add(neighbour);
                    GraphPathInfo[neighbour] = new NodePathInfo(newPath, newTravelTime);
                    RegisterAllNeighbours(neighbour, GraphPathInfo);
                }
            }
        }

        public static IEnumerable<Node> FindPathToClosest(Dictionary<Node, NodePathInfo> graphPathInfo, Func<Node, bool> isTarget)
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
            return minNodePathInfo.Path;
        }
    }
}
