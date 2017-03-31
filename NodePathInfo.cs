using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Interfaces;

namespace Homm.Client
{
    class NodePathInfo
    {
        public readonly LinkedList<Node> Path;
        public readonly double TravelTime;

        public NodePathInfo(LinkedList<Node> path, double travelTime)
        {
            Path = path;
            TravelTime = travelTime;
        }
    }
}
