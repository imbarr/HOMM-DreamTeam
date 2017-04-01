﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoMM;
using HoMM.ClientClasses; 

namespace Homm.Client
{
    class Node
    {
        public readonly MapObjectData MapObjectData;
        public readonly List<Node> IncidentNodes;
        public readonly double Weight;
        public readonly Point Coords;

        public Node(MapObjectData mapObjectData)
        {
            IncidentNodes = new List<Node>();
            MapObjectData = mapObjectData;
            Coords = new Point(mapObjectData.Location.X, mapObjectData.Location.Y);
            Weight = HommRules.Current.MovementDuration *
                     AdditionalConstants.MovementResistanceMultiplier[mapObjectData.Terrain];
        }

        public void Connect(Node other)
        {
            IncidentNodes.Add(other);
            other.IncidentNodes.Add(this);
        }

        public override string ToString()
        {
            return Coords.ToString();
        }
    }
}
