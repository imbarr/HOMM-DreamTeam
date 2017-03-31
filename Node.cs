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
        public readonly MapObjectData mapObjectData;
        public readonly List<Node> incidentNodes;
        public readonly double weight;

        public Node(MapObjectData mapObjectData)
        {
            incidentNodes = new List<Node>();
            this.mapObjectData = mapObjectData;
            weight = HommRules.Current.MovementDuration *
                     AdditionalConstants.MovementResistanceMultiplier[mapObjectData.Terrain];
        }

        public void Connect(Node other)
        {
            incidentNodes.Add(other);
            other.incidentNodes.Add(this);
        }
    }
}
