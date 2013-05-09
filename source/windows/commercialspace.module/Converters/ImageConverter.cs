using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Bespoke.Sph.Windows.CommercialSpaceModule.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var memStream = new MemoryStream((byte[])value, false);
            var empImage = new BitmapImage();
           // empImage.SetValue(memStream);
            return empImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
