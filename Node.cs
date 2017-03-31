using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoMM.ClientClasses;

namespace Homm.Client
{
    class Node
    {
        private MapObjectData mapObjectData;
        private List<Node> incidentNodes;
        private double weight = 0.0;
        //TODO: расчет веса
        public Node(MapObjectData mapObject)
        {
            incidentNodes = new List<Node>();
            mapObjectData = mapObject;
        }

        public void Connect(Node other)
        {
            incidentNodes.Add(other);
        }
    }
}
