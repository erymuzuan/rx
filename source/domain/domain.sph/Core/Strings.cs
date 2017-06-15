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
using System.Xml;
using Humanizer;
using Mono.Cecil;

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
            // ReSharper disable AssignNullToNotNullAttribute
            return elementType.Namespace.StartsWith(typeof(Entity).Namespace);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        public static string EscapeVerbatim(this string text)
        {
            return text.Replace("\"", "\"\"");
        }
        public static string ToVerbatim(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return "string.Empty";
            return $@"@""{ text.Replace("\"", "\"\"")}""";
        }
        public static string ToCsharpIdentitfier(this string text)
        {

            var rg = new Regex("([a-z])([A-Z])");

            var t = rg.Replace(text, "$1_$2");

            var code = new List<char>();
            var first = true;
            var gap = false;
            var previousUpper = false;
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


        public static string Tidy(this string html)
        {
            return Regex.Replace(html, @"^\s+$[\r\n]*", "", RegexOptions.Multiline)
                 .Replace("class=\" ", "class=\"")
                 .Replace("<input  class=\"", "<input class=\"")
                 .Replace(" title=\"\"", "")
                 .Replace(", enable :true \"", "\"");
        }

        public static string ToCamelCase(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            var removeSpace = Regex.Replace(text, "\\s[A-Za-z]{1}", match =>
            {
                var v = match.ToString();
                return v.Replace(" ", "").ToUpperInvariant();
            });
            text = removeSpace.Replace("@", "").Replace("_", string.Empty);

            if (text.ToCharArray().All(char.IsUpper))
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

        public static string ToClrIdentifier(this string value, string strategy = "")
        {
            switch (strategy)
            {
                case "Auto":
                    return value.ToClrAuto();
                case "camel":
                    return value.ToCamelCase();
                case "_":
                    return value.ToIdFormat().Replace("-", "_");
                // default to Pascal since the previous code just take Pascal 
                //case "pascal":
                default:
                    return value.ToPascalCase();
            }
        }

        public static string ToClrAuto(this string text)
        {
            var sb = new StringBuilder(text);
            var norms = new[]
            {
                ".",
                "-",
                "_",
                ",",
                "'",
                ";",
                ":",
                "~",
                "`",
                "!",
                "@",
                "#",
                "$",
                "%",
                "^",
                "&",
                "*",
                "(",
                ")",
                "+",
                "=",
                "{",
                "[",
                "}",
                "]",
                "|",
                "\\",
                " ",
                "\"",
                "/",
                "?",
                "__",
                "__",
                "__",
            };
            foreach (var norm in norms)
            {
                sb.Replace(norm, "_");
            }
            if (sb.Length > 0 && sb[0] == '_')
                sb.Remove(0, 1);

            return sb.ToString();
        }
        public static string ToIdFormat(this string text)
        {
            var camel = text.SplitCamelCase();
            var sb = new StringBuilder(camel);
            var norms = new[]
            {
                ".",
                "-",
                "_",
                ",",
                "'",
                ";",
                ":",
                "~",
                "`",
                "!",
                "@",
                "#",
                "$",
                "%",
                "^",
                "&",
                "*",
                "(",
                ")",
                "+",
                "=",
                "{",
                "[",
                "}",
                "]",
                "|",
                "\\",
                " ",
                "\"",
                "/",
                "?",
                "--",
                "--",
                "--",
            };
            foreach (var norm in norms)
            {
                sb.Replace(norm, "-");
            }
            if (sb.Length > 0 && sb[0] == '-')
                sb.Remove(0, 1);

            return sb.ToString().ToLowerInvariant();
        }

        public static string ToPascalCase(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            return string.Join("", text.Split(new[] { '$', '_', ' ', '-', '.', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Substring(0, 1).ToUpper() + s.Substring(1)).ToArray());
        }
        public static string TimeStampToString(this byte[] tstamp)
        {
            var val = GetTimeStampValue(tstamp);
            return "0x" + val.ToString("X").PadLeft(16, '0');
        }
        private static long GetTimeStampValue(byte[] tstamp)
        {
            if (tstamp == null || tstamp.Length == 0)
                throw new ArgumentNullException(nameof(tstamp));
            var buffer = new byte[tstamp.Length];
            tstamp.CopyTo(buffer, 0);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        private static IEnumerable<char> ToCamelCaseHelper(this string text)
        {
            var first = true;
            foreach (var c in text)
            {
                if (first)
                {
                    first = false;
                    yield return char.ToLower(c);
                }
                else
                {
                    yield return c;
                }
            }
        }


        private static IEnumerable<char> ToCamelCaseHelperWithAllUpper(this string text)
        {
            var _ = false;
            var x = false;
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
        public static StringBuilder AppendLine(this StringBuilder text, string value, bool predicate)
        {
            if (predicate)
                text.AppendLine(value);
            return text;
        }

        public static StringBuilder JoinAndAppend<T>(this StringBuilder text,
            IEnumerable<T> list,
            string seperator = ",",
            Func<T, string> projection = null)
        {
            if (null == projection)
                projection = x => $"{x}";

            var line = string.Join(seperator, list.Select(projection));
            return text.Append(line);
        }
        public static StringBuilder JoinAndAppendLine<T>(this StringBuilder text,
            IEnumerable<T> list,
            string seperator = ",",
            Func<T, string> projection = null)
        {
            if (null == projection)
                projection = x => $"{x}";

            var line = string.Join(seperator, list.Select(projection));
            return text.AppendLine(line);
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
                return $"{dec:f2}";
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
            return null == value ? string.Empty : $"{value}";
        }
        public static string ToString<T>(this IEnumerable<T> list, string seperator = ",", Func<T, string> projection = null)
        {
            if (null == projection)
                projection = x => $"{x}";
            return string.Join(seperator, list.Select(projection));
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
            if (string.IsNullOrWhiteSpace(input)) return null;
            const RegexOptions OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            var matches = Regex.Matches(input, pattern, OPTIONS);
            return matches.Count > 0 ? matches[0].Groups[@group].Value.Trim() : null;
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
            if (type == typeof(void)) return "void";
            if (type == typeof(DateTime)) return "DateTime";
            if (type == typeof(string)) return "string";
            if (type == typeof(int)) return "int";
            if (type == typeof(bool)) return "bool";
            if (type == typeof(decimal)) return "decimal";
            if (type == typeof(float)) return "float";
            if (type == typeof(double)) return "double";
            if (type == typeof(short)) return "short";
            if (type == typeof(long)) return "long";
            if (type == typeof(byte)) return "byte";
            if (type == typeof(byte[])) return "byte[]";

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

        public static Type TryGuessType(this object literal)
        {
            if (literal is bool) return typeof(bool);
            if (literal is decimal) return typeof(decimal);
            if (literal is int) return typeof(int);
            if (literal is DateTime) return typeof(DateTime);

            int intValue;
            if (int.TryParse($"{literal}", out intValue)) return typeof(int);
            decimal decimalValue;
            if (decimal.TryParse($"{literal}", out decimalValue)) return typeof(decimal);
            DateTime dateTimeValue;
            if (DateTime.TryParse($"{literal}", out dateTimeValue)) return typeof(DateTime);
            bool boolValue;
            if (bool.TryParse($"{literal}", out boolValue)) return typeof(bool);
            return typeof(string);
        }
        public static Type GetType(string typeName)
        {
            var t = Type.GetType(typeName);
            if (null != t) return t;

            var splits = typeName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var typeName1 = splits.FirstOrDefault().ToEmptyString().Trim();
            var assemblyName = splits.LastOrDefault().ToEmptyString().Trim();
            string path = $"{ConfigurationManager.CompilerOutputPath}\\{assemblyName}.dll";
            if (!File.Exists(path))
            {
                var system = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(x => x.FullName.StartsWith($"{assemblyName},"));
                return system?.GetType(typeName1);
            }

            var dll = System.Reflection.Assembly.LoadFile(path);
            t = dll.GetType(splits.First().Trim());
            return t;
        }
        public static TypeDefinition GetTypeDefinition(string typeName)
        {
            var splits = typeName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var fullName = splits.FirstOrDefault().ToEmptyString().Trim();
            var assemblyName = splits.LastOrDefault().ToEmptyString().Trim();
            var path = $"{ConfigurationManager.CompilerOutputPath}\\{assemblyName}.dll";

            var assembly = AssemblyDefinition.ReadAssembly(path);
            var type = assembly.MainModule.Types.FirstOrDefault(x => x.FullName == fullName);
            return type;
        }

        public static object ToDbNull(this string value, int length)
        {
            if (length <= 0)
                throw new ArgumentException("Length must be greater than 0", nameof(length));
            if (null == value)
                return DBNull.Value;

            if (!string.IsNullOrWhiteSpace(value) && value.Length > length)
                return value.Substring(0, length);

            return value;
        }

        public static object TruncateLeft(this string value, int length)
        {
            if(length <= 0)
                throw new ArgumentException("Length must be greater than 0", nameof(length));
            if (null == value)
                return DBNull.Value;

            if (!string.IsNullOrWhiteSpace(value) && value.Length > length)
                return value.Truncate(length, "",TruncateFrom.Left);

            return value;
        }
        public static object TruncateRight(this string value, int length)
        {
            if (length <= 0)
                throw new ArgumentException("Length must be greater than 0", nameof(length));
            if (null == value)
                return DBNull.Value;

            if (!string.IsNullOrWhiteSpace(value) && value.Length > length)
                return value.Truncate(length, "");

            return value;
        }


        public static object ToDbNull(this object value)
        {
            var xml = value as XmlDocument;
            if (null != xml)
                return xml.OuterXml;

            return value ?? DBNull.Value;
        }


        public static int? ParseNullableInt32(this string val) 
        {
            if (val == null) return default(int);
            int value;
            return int.TryParse(val, out value) ? value : default(int?);
        }
        public static decimal? ParseNullableDecimal(this string val) 
        {
            if (val == null) return default(decimal);
            decimal value;
            return decimal.TryParse(val, out value) ? value : default(decimal?);
        }
        public static bool? ParseNullableBoolean(this string val) 
        {
            if (val == null) return default(bool);
            bool value;
            return bool.TryParse(val, out value) ? value : default(bool?);
        }
        public static DateTime? ParseNullableDateTime(this string val) 
        {
            if (val == null) return default(DateTime);
            DateTime value;
            return DateTime.TryParse(val, out value) ? value : default(DateTime?);
        }

        public static T? ReadNullable<T>(this object val) where T : struct
        {
            if (val == null) return default(T);
            Console.WriteLine($"{val.GetType()} -> {val}");
            if (val == DBNull.Value) return default(T);
            return (T)val;
        }
        public static string ReadNullableString(this object val)
        {
            if (val == null) return null;
            if (val == DBNull.Value) return null;
            return (string)val;
        }
        public static T ReadNullableObject<T>(this object val) where T : class
        {
            if (val == null) return default(T);
            if (val == DBNull.Value) return default(T);
            return (T)val;
        }
        public static XmlDocument ReadNullableXmlDocument(this object val)
        {
            if (val == null) return null;
            if (val == DBNull.Value) return null;
            var xml = val as XmlDocument;
            if (null != xml) return xml;

            var text = val as string;
            if (null != text)
            {
                var xd = new XmlDocument();
                xd.LoadXml(text);
                return xd;
            }
            return null;
        }
        public static byte[] ReadNullableByteArray(this object val)
        {
            if (val == null) return null;
            if (val == DBNull.Value) return null;
            return (byte[])val;
        }

        public static string ConvertToUnsecureString(this SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException(nameof(securePassword));

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
            return path.Replace(".", "().")
                .Replace("$root().", "$root.")
                .Replace(".partial().", ".partial.");
        }

    }
}
