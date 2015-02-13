using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Bespoke.Sph.SyntaxVisualizers.Converters
{
    [ValueConversion(typeof(int), typeof(bool))]
    public class ComparisonToBooleanConverter : MarkupExtension, IValueConverter
    {
        public string Operator { get; set; }
        public string Compare { get; set; }

        public object Convert(object value, Type targetType, object parameterObject, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                int number;
                if (int.TryParse(Compare, out number))
                {
                    if (Operator == "eq") return number == (int)value;
                    if (Operator == "neq") return number != (int)value;
                    if (Operator == "gt") return (int)value > number;
                    if (Operator == "lt") return (int)value < number;
                }
            }
            if (value is decimal)
            {
                decimal number;
                if (decimal.TryParse(Compare, out number))
                {
                    if (Operator == "eq") return number == (decimal)value;
                    if (Operator == "neq") return number != (decimal)value;
                    if (Operator == "gt") return (decimal)value > number;
                    if (Operator == "lt") return (decimal)value < number;
                }
            }

            var s = value as string;
            if (s != null)
            {
                if (Operator == "eq")
                    return s == this.Compare;
                if (Operator == "neq")
                    return s != this.Compare;

                throw new InvalidOperationException("Operator " + this.Operator + " cannot be recognized, only eq and neq for string");
            }
            return false;
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
