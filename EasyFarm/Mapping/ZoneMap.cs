using System.Collections.Generic;

namespace EasyFarm.Mapping
{
    public class ZoneMap : IZoneMap
    {
        public List<ISpot> Spots { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var zoneMap = obj as ZoneMap;

            if (zoneMap.Spots.Count != Spots.Count)
            {
                return false;
            }

            foreach (Spot spot in zoneMap.Spots)
            {
                if (!Spots.Contains(spot))
                {
                    return false;
                }
            }

            foreach (Spot spot in Spots)
            {
                if (!zoneMap.Spots.Contains(spot))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return (Spots != null ? Spots.GetHashCode() : 0);
        }
    }
}