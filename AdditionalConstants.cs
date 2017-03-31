using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using HoMM.ClientClasses;

namespace Homm.Client
{
    class AdditionalConstants
    {
        public readonly ReadOnlyDictionary<Terrain, double> MovementResistanceMultiplier =
            new ReadOnlyDictionary<Terrain, double>(new Dictionary<Terrain, double>
            {
                {Terrain.Road, 1.0},
                {Terrain.Desert, 1.5},
                {Terrain.Grass, 1.3},
                {Terrain.Snow, 1.7},
                {Terrain.Marsh, 1.7}
            });
    }
}
