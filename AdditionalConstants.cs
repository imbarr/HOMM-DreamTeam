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
                {Terrain.Road, 0.75},
                {Terrain.Desert, 1.15},
                {Terrain.Grass, 1.00},
                {Terrain.Snow, 1.30},
                {Terrain.Marsh, 1.30}
            });
    }
}
