using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFarm.Pathfinding
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