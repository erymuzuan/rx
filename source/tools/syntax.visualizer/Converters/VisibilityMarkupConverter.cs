using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Bespoke.Sph.SyntaxVisualizers.Converters
{
    public class VisibilityMarkupConverter : MarkupExtension, IValueConverter
    {
        public string Value { get; set; }
        public bool Inverse { get; set; }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Inverse)
                return Value == value as string ? Visibility.Collapsed : Visibility.Visible;
            return Value == value as string ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
