using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Converters
{
    public class RoleModeratorToVisibiltyConverter : ViewModelBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (IsInDesignMode) return Visibility.Visible;

            var role = parameter as string;
            if (string.IsNullOrEmpty(role)) return Visibility.Collapsed;

            //return WebContext.Current.User.IsInRole(role) ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
