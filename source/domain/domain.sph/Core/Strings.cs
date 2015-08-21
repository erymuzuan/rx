using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

        public static bool IsEqual<T>(this T value, T value2) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            return value.Equals(value2);
        }

        /// <summary>
        /// Find out if the type is part of SPH
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsSystemType(this object obj)
        {
            if (null == obj) throw new ArgumentNullException(nameof(obj));
            var elementType = obj.GetType();
            if (string.IsNullOrWhiteSpace(elementType.Namespace)) return false;
            if (elementType.Namespace.StartsWith(typeof(Entity).Namespace))// custom entity
            {
                return true;
            }
            return false;
        }
        public static string ToCsharpIdentitfier(this string text)
        {

            var rg = new Regex("([a-z])([A-Z])");

            var t = rg.Replace(text, "$1_$2");

            var code = new List<char>();
            bool first = true;
            bool gap = false;
            bool previousUpper = false;
            foreach (var c in t)
            {
                if (first && !char.IsLetter(c))
                {
                    continue;
                }
                if (char.IsLetter(c) && first)
                {
                    code.Add(char.ToUpperInvariant(c));
                    first = false;
                    previousUpper = true;
                    continue;
                }

                if (char.IsLetter(c) && gap)
                {
                    code.Add(char.ToUpperInvariant(c));
                    gap = false;
                    continue;
                }

                if (char.IsLetter(c) && char.IsUpper(c) && !previousUpper)
                {
                    code.Add(char.ToUpperInvariant(c));
                    gap = false;
                    previousUpper = true;
                    continue;
                }

                if (char.IsLetter(c) || char.IsDigit(c))
                {
                    code.Add(char.ToLowerInvariant(c));
                    gap = false;
                }
                else
                {
                    gap = true;
                }


            }
            return new string(code.ToArray());
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

        public static string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1-$2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1-$2"
            );
        }

        public static string ToIdFormat(this string text)
        {
            return text.SplitCamelCase().Replace(".", "-")
                .Replace("_", "-")
                .Replace(",", "-")
                .Replace("'", "-")
                .Replace(";", "-")
                .Replace(":", "-")
                .Replace("~", "-")
                .Replace("`", "-")
                .Replace("!", "-")
                .Replace("@", "-")
                .Replace("#", "-")
                .Replace("$", "-")
                .Replace("%", "-")
                .Replace("^", "-")
                .Replace("&", "-")
                .Replace("*", "-")
                .Replace("(", "-")
                .Replace(")", "-")
                .Replace("+", "-")
                .Replace("=", "-")
                .Replace("{", "-")
                .Replace("[", "-")
                .Replace("}", "-")
                .Replace("]", "-")
                .Replace("|", "-")
                .Replace("\\", "-")
                .Replace(" ", "-")
                .Replace("\"", "-")
                .Replace("/", "-")
                .Replace("?", "-")
                .Replace("--", "-")
                .Replace("--", "-")
                .Replace("--", "-")
                .ToLowerInvariant()
                ;
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

        public static string ToLiteral(this string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }

        public static string EscapeUriString(this string value)
        {
            return Uri.EscapeUriString(value.ToEmptyString());
        }
        public static string EscapeDataString(this string value)
        {
            return Uri.EscapeDataString(value.ToEmptyString());
        }

        public static string ToEmptyString(this object value)
        {
            if (null == value) return string.Empty;
            return $"{value}";
        }

        public static DateTime? RegexDateTimeValue(string input, string pattern, string group, params string[] formats)
        {
            var val = RegexSingleValue(input, pattern, group);
            DateTime dv;
            if (DateTime.TryParseExact(val, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dv))
                return dv;
            return null;
        }
        public static int? RegexInt32Value(string input, string pattern, string group)
        {
            var val = RegexSingleValue(input, pattern, group);
            int dv;
            if (int.TryParse(val, out dv))
                return dv;
            return null;
        }
        public static decimal? RegexDecimalValue(string input, string pattern, string group)
        {
            var val = RegexSingleValue(input, pattern, group);
            decimal dv;
            if (decimal.TryParse(val, out dv))
                return dv;
            return null;
        }

        public static string RegexSingleValue(string input, string pattern, string group)
        {
            const RegexOptions OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            var matches = Regex.Matches(input, pattern, OPTIONS);
            return matches.Count == 1 ? matches[0].Groups[@group].Value.Trim() : null;
        }

        public static string[] RegexValues(string input, string pattern, string group)
        {
            const RegexOptions OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            var matches = Regex.Matches(input, pattern, OPTIONS);
            var ff = from Match v in matches
                     select v.Groups[@group].Value.Trim();
            return ff.ToArray();
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
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
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
