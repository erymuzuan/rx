using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bespoke.Sph.Mangements.Converters
{

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == Application.Current) return Visibility.Visible;
            if (null == value) return Visibility.Collapsed;
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
