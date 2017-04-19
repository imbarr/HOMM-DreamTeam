using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoMM;
using HoMM.ClientClasses;

namespace Homm.Client
{
    static class Targeting
    {
        public static bool IsValidTarget(this Node node, Hero hero, double travelTime, double maxTravelTime, string myRespawnSide)
        {
            if (travelTime > maxTravelTime)
                return false;
            var data = node.MapObjectData;
            return data.NeutralArmy == null && data.Hero == null && (data.ResourcePile != null ||
                   data.Mine != null && data.Mine.Owner != myRespawnSide) ||
                   data.NeutralArmy != null && IsWin(hero.Army, data.NeutralArmy.Army);
        }

        public static bool IsValidDwelling(this Node node, Dictionary<Resource, int> treasury,
            double travelTime, double maxTravelTime, int minHireNumber)
        {
            if (travelTime > maxTravelTime)
                return false;
            var data = node.MapObjectData;
            return data.Dwelling != null
                && data.Dwelling.UnitType != UnitType.Militia
                && data.NeutralArmy == null
                && data.Hero == null
                && node.MaximumHireNumber(treasury) >= minHireNumber;
        }

        public static int MaximumHireNumber(this Node node, Dictionary<Resource, int> treasury)
        {
            var dwelling = node.MapObjectData.Dwelling;
            if (dwelling == null)
                return 0;
            var available = dwelling.AvailableToBuyCount;
            var priceForOne = UnitsConstants.Current.UnitCost[dwelling.UnitType];
            var affordable = priceForOne.Min(p =>
                treasury[p.Key] / priceForOne[p.Key]);
            return Math.Min(available, affordable);
        }

        public static bool IsObstacle(this Node node, Point currentLocation)
        {
            var data = node.MapObjectData;
            return data.NeutralArmy != null
                   || data.Hero != null && node.Coords != currentLocation;
        }

        public static bool IsWin(Dictionary<UnitType, int> army, Dictionary<UnitType, int> other)
        {
            return Combat.Resolve(new ArmiesPair(army, other)).IsAttackerWin;
        }
    }
}
