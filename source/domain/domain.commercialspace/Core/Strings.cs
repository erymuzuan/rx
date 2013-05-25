using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;

namespace Bespoke.SphCommercialSpaces.Domain
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
