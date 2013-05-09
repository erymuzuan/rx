using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bespoke.Sph.Windows.Infrastructure.Converters
{
    public class BooleanToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(null == Application.Current) return Visibility.Visible;
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}