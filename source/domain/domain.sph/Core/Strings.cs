using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace Bespoke.Sph.Domain
{
    public static class Strings
    {
        public const string DEFAULT_NAMESPACE = "http://www.bespoke.com.my/";

        public static bool IsEqual<T>(this T value, T value2) where T : struct ,IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            return value.Equals(value2);
        }
        //public static bool IsEqual<T>(this T? value, T? value2) where T : struct ,IConvertible
        //{
        //    if (!typeof(T).IsEnum)
        //    {
        //        throw new ArgumentException("T must be an enumerated type");
        //    }
        //    return value.Equals(value2);
        //}

        public static string PadLeft(this int value, int width = 6, char pad = '0')
        {
            var f = string.Format(CultureInfo.CurrentCulture, "{0}", value);
            return f.PadLeft(6, '0');
        }
        public static string PadLeft(this int? value, int width = 6, char pad = '0')
        {
            var f = string.Format(CultureInfo.CurrentCulture, "{0}", value);
            return f.PadLeft(6, '0');
        }

        public static string GetFormattedValue(this string value)
        {
            DateTime date;
            decimal dec;
            if (DateTime.TryParse(value, out date))
            {
                return date.ToString("dd MMM yyyy");
            }
            if (decimal.TryParse(value, out dec))
            {
                return string.Format("{0:f2}", dec);
            }
            return value;
        }


        public static string ToEmptyString(this object value)
        {
            if (null == value) return string.Empty;
            return string.Format("{0}", value);
        }
        public static string RegexSingleValue(string input, string pattern, string group)
        {
            const RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            var matches = Regex.Matches(input, pattern, option);
            return matches.Count == 1 ? matches[0].Groups[@group].Value.Trim() : null;
        }
        public static string ToSDate(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            value = value.Replace(" ", string.Empty);


            if (value.Length != 8) return value;
            var chars = value.Trim().ToCharArray();

            var sb = new StringBuilder();
            for (int i = 0; i < chars.Length; i++)
            {
                if (!char.IsDigit(chars[i]))
                    return value;
                sb.Append(chars[i]);
                switch (i)
                {
                    case 3:
                    case 5:
                        sb.Append("-");
                        break;
                    default:
                        break;
                }
            }

            return sb.ToString();
        }

        public static string GetShortAssemblyQualifiedName(this Type type)
        {
            return string.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name);
        }

        public static string ConvertToUnsecureString(this SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
