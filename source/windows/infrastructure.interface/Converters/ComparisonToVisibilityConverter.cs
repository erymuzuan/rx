using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Bespoke.Cycling.Windows.Infrastructure.Converters
{
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class ComparisonToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public string Operator { get; set; }
        public string Compare { get; set; }

        public object Convert(object value, Type targetType, object parameterObject, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                int c;
                if (int.TryParse(Compare, out c))
                {
                    if (Operator == "eq")
                    {
                        return (c == (int)value) ? Visibility.Visible : Visibility.Collapsed;
                    }

                    if (Operator == "gt")
                    {
                        return (int)value > c ? Visibility.Visible : Visibility.Collapsed;
                    }

                    if (Operator == "lt")
                    {
                        return (int)value < c ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }

            if (value is string)
            {
                var stringValue = value as string;
                if (Operator == "eq")
                    return stringValue == this.Compare ? Visibility.Visible : Visibility.Collapsed;
                if (Operator == "neq")
                    return stringValue != this.Compare ? Visibility.Visible : Visibility.Collapsed;

                throw new InvalidOperationException("Operator " + this.Operator + " cannot be recognized, only eq and neq for string");
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameterObject, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("oii");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}