using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight;

namespace Bespoke.Sph.Windows.CommercialSpaceModule.Converters
{
    public class BooleanToVisibiltyConverter : ViewModelBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (IsInDesignMode) return Visibility.Visible;
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
