using Bespoke.Cycling.Domain;
using GalaSoft.MvvmLight;
using System;
using System.Windows.Data;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Converters
{
    /// <summary>
    /// Two way IValueConverter that lets you bind a property on a bindable object
    /// that can be an empty string value to a dependency property that should 
    /// be set to null in that case
    /// </summary>
    public class RideEndLocationConverter : ViewModelBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            const string select = "Please pick one";
            if (IsInDesignMode)
            {
                return select;
            }

            var ride = value as Ride;
            if (null == ride) return "<Null Ride>";
            if (null == ride.EndLocation) return select;
            if (string.IsNullOrWhiteSpace(ride.EndLocation.Name)) return select;
            //if(0 == ride.Metadata.StartLocation.GeoLocationId) return string.Empty;

            return ride.EndLocation.Name;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("No back conversion");
        }
    }
    /// <summary>
    /// Two way IValueConverter that lets you bind a property on a bindable object
    /// that can be an empty string value to a dependency property that should 
    /// be set to null in that case
    /// </summary>
    public class RideLocationToPickLinkConverter : ViewModelBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            const string select = "Please pick one";
            if (IsInDesignMode)
            {
                return select;
            }

            var loc = value as GeoLocation;
            if (null == loc)  return select;
            if (string.IsNullOrWhiteSpace(loc.Name)) return select;
            //if(0 == ride.Metadata.StartLocation.GeoLocationId) return string.Empty;

            return loc.Name;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("No back conversion");
        }
    }
}
