using System;
using System.Globalization;
using System.Windows.Data;

namespace Bespoke.Sph.ControlCenter.Converters
{
    public class ZeroStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
                return (int)value == 0;
            if (value is decimal)
                return (decimal)value == decimal.Zero;
            if (value is double)
                return 0d.Equals((double)value);

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }
}