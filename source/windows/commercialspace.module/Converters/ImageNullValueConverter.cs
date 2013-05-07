using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Converters
{
    public class ImageNullValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null != value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
