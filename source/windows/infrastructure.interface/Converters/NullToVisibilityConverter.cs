using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bespoke.Sph.Windows.Infrastructure.Converters
{
    public class NullToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(null == Application.Current) return Visibility.Visible;
            return null != value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}