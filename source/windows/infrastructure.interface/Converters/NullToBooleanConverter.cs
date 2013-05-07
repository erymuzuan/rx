using System;
using System.Globalization;
using System.Windows.Data;

namespace Bespoke.Cycling.Windows.Infrastructure.Converters
{
    public class NullToBooleanConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null != value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}