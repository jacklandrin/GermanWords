using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GermanWords.DeutschWelle
{
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            long retValue = 0;
            if (value is TimeSpan)
            {
                var durValue = (TimeSpan)value;
                retValue = durValue.Ticks;
            }
            else if(value is Duration)
            {
                var durValue = (Duration)value;
                if (durValue.HasTimeSpan && durValue != Duration.Automatic && durValue != Duration.Forever)
                {
                    retValue = durValue.TimeSpan.Ticks;
                }
            }
            return retValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
