using System;

namespace FinalFantasyXI.Pathfinding
{
    public static class MappingConversion
    {
        public static int ConvertFromFloatToInt(float startingFloat)
        {

            int roundedInt = Convert.ToInt16(Math.Round(Convert.ToDouble(startingFloat)));
            return roundedInt;
        }
    }
}