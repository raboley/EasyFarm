using System.Collections.Generic;

namespace FinalFantasyXI.Mapping
{
    public class Spot : ISpot
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int F { get; set; } = 0;
        public int G { get; set; } = 0;
        public int H { get; set; } = 0;

        public List<Spot> Neighbors { get; set; }
        public Spot Previous { get; set; }

        public bool IsWall { get; set; } = false;


    public override bool Equals(object obj)
        {
            ISpot spot = obj as Spot;
            if (spot != null)
            {
                if (X == spot.X && Y == spot.Y && Z == spot.Z)
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ Z;
                return hashCode;
            }
        }
    }
}