using System;
using System.Globalization;
using System.Windows.Data;

namespace Bespoke.Sph.Windows.Infrastructure.Converters
{
    public class DontShowZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value) return string.Empty;
            if (value is int && (int)value == 0) return string.Empty;
            if (value is decimal && (decimal)value == decimal.Zero) return string.Empty;
            if (value is double && (double)value <= 0.00001d) return string.Empty;

            return string.Format("{0:# ###.00}",value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var numberstring = value as string;
            if (string.IsNullOrWhiteSpace(numberstring))
                return 0d;
            double dval;
            if (double.TryParse(numberstring, out dval))
                return dval;

            return 0d;
        }
    }
}