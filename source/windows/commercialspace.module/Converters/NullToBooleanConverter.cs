using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Converters
{
    public class NullToBooleanConverter : ViewModelBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (IsInDesignMode) return Visibility.Visible;
            return null != value ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
