using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EasyFarm.Mapping
{
    class GraphConverter : IValueConverter
    {
        public Object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((int)value < 0)
                {
                    if((string)parameter == "X")
                    {
                        return ((int)value * -1 * 20) + 200;
                    }
                    return (int) value * -1 * 20;
                }
                if((string)parameter == "Z")
                {
                    return ((int)value * 20) + 200;
                }

                return ((int) value * 20);
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
