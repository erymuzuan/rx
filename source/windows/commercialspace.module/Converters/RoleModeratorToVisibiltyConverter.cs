using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Converters
{
    public class RoleOrganizerToVisibiltyConverter :ViewModelBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (IsInDesignMode) return Visibility.Visible;
            //if (WebContext.Current.User.IsInRole("organizer")) return Visibility.Visible;
            //return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
