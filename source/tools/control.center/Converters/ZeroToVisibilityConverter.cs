using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bespoke.Sph.ControlCenter.Converters
{
    public class ZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
                return (int)value == 0 ? Visibility.Collapsed : Visibility.Visible;
            if (value is decimal)
                return (decimal)value == decimal.Zero ? Visibility.Collapsed : Visibility.Visible;
            if (value is double)
                return (double)value < 0.0001d ? Visibility.Collapsed : Visibility.Visible;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }
}