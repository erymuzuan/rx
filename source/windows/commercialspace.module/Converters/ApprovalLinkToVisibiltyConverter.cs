using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Bespoke.Cycling.Domain;
using GalaSoft.MvvmLight;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Converters
{
    public class ApprovalLinkToVisibiltyConverter : ViewModelBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (IsInDesignMode) return Visibility.Visible;

            var ride = value as Ride;
            if (null == ride) return Visibility.Collapsed;

            var role = parameter as string;
            if (string.IsNullOrEmpty(role)) return Visibility.Collapsed;

            //TODO: webcontext
            // if (WebContext.Current.User.IsInRole(role) && !ride.IsApproved)
            //    return Visibility.Visible;

            //return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
