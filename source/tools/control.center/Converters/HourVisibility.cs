using System;
using System.Globalization;
using System.Windows.Data;

namespace Bespoke.Sph.ControlCenter.Converters
{

    public class HourVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value) return true;
            var dateTime = (DateTime) value;
            if (dateTime.Minute == 0) return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}