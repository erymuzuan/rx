using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace Bespoke.Sph.Domain
{
    public static class Strings
    {
        public const string DEFAULT_NAMESPACE = "http://www.bespoke.com.my/";

        public static string WriteLine2(this string value)
        {
            Console.WriteLine(value);
            return value;
        }

        public static bool IsEqual<T>(this T value, T value2) where T : struct ,IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            return value.Equals(value2);
        }

        public static string ToCamelCase(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            if (text.Replace("_", string.Empty).ToCharArray().All(char.IsUpper))
            {
                return new string(text.ToCamelCaseHelperWithAllUpper().Where(c => c != char.MinValue).ToArray());
            }

            return new string(text.ToCamelCaseHelper().ToArray());
        }
        private static IEnumerable<char> ToCamelCaseHelper(this string text)
        {
            bool first = true;
            foreach (var c in text)
            {
                if (first)
                {
                    first = false;
                    yield return Char.ToLower(c);
                }
                else
                {
                    yield return c;
                }
            }
        }
        private static IEnumerable<char> ToCamelCaseHelperWithAllUpper(this string text)
        {
            bool _ = false;
            bool x = false;
            foreach (var c in text)
            {
                if (x) yield return char.MinValue;
                if (c == '_')
                {
                    _ = true;
                    yield return char.MinValue;
                }
                else
                {
                    if (_)
                    {
                        _ = false;
                        x = true;
                        yield return c;
                    }
                    yield return char.ToLower(c);

                }


            }
        }

        public static StringBuilder AppendLinf(this StringBuilder text, string format, params object[] args)
        {
            text.AppendFormat(format, args);
            text.AppendLine();

            return text;
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
            const RegexOptions OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            var matches = Regex.Matches(input, pattern, OPTIONS);
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
                }
            }

            return sb.ToString();
        }

        public static string GetShortAssemblyQualifiedName(this Type type)
        {
            return string.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name);
        }
        public static string ToCSharp(this Type type)
        {
            if (type == typeof(DateTime)) return "DateTime";
            if (type == typeof(string)) return "string";
            if (type == typeof(int)) return "int";
            if (type == typeof(bool)) return "bool";
            if (type == typeof(decimal)) return "decimal";
            if (type == typeof(float)) return "float";
            if (type == typeof(double)) return "double";
            if (type == typeof(short)) return "short";
            if (type == typeof(long)) return "long";

            if (type == typeof(DateTime?)) return "DateTime?";
            if (type == typeof(int?)) return "int?";
            if (type == typeof(bool?)) return "bool?";
            if (type == typeof(decimal?)) return "decimal?";
            if (type == typeof(float?)) return "float?";
            if (type == typeof(double?)) return "double?";
            if (type == typeof(short?)) return "short?";
            if (type == typeof(long?)) return "long?";
            return type.FullName;
        }
        public static object ToDbNull(this object value)
        {
            return value ?? DBNull.Value;
        }


        public static T? ReadNullable<T>(this object val) where T : struct
        {
            if (val == null) return default(T);
            Console.WriteLine("{0} -> {1}", val.GetType(), val);
            if (val == DBNull.Value) return default(T);
            return (T)val;
        }
        public static string ReadNullableString(this object val)
        {
            if (val == null) return null;
            if (val == DBNull.Value) return null;
            return (string)val;
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

        public static string ConvertJavascriptObjectToFunction(this string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;
            if (path.Contains("().")) return path;
            return path.Replace(".", "().");
        }

    }
}
