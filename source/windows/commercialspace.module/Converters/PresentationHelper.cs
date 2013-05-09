using System;

namespace Bespoke.Sph.Windows.CommercialSpaceModule.Converters
{
    public class PresentationHelper
    {
        public static int ParseIdFromUri(Uri value)
        {
            var lastToken = GetLastTokenFromUri(value);

            if (lastToken == null)
            {
                throw new ArgumentException(String.Format("Could not parse Id value from uri '{0}'.", value));
            }
            int id;

            if (Int32.TryParse(lastToken, out id) == false)
            {
                throw new ArgumentException(String.Format("Id value in uri '{0}' was not an Int32 value.", value));
            }
            return id;
        }

        public static string GetLastTokenFromUri(Uri value)
        {
            var uriAsString = value.ToString();

            if (uriAsString.Contains("/") == false || uriAsString == "/")
            {
                return null;
            }
            var splitByForwardSlash = uriAsString.Split('/');

            return splitByForwardSlash.Length == 0 ? null : splitByForwardSlash[splitByForwardSlash.Length - 1];
        }
    }
}
