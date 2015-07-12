using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bespoke.Sph.Powershells
{
    internal class DynParamQuotedString
    {
        /*
            This works around the PowerShell bug where ValidateSet values aren't quoted when necessary, and
            adding the quotes breaks it. Example:

            ValidateSet valid values = 'Test string'  (The quotes are part of the string)

            PowerShell parameter binding would interperet that as [Test string] (no single quotes), which wouldn't match
            the valid value (which has the quotes). If you make the parameter a DynParamQuotedString, though,
            the parameter binder will coerce [Test string] into an instance of DynParamQuotedString, and the binder will
            call ToString() on the object, which will add the quotes back in.
        */

        internal static string DefaultQuoteCharacter = "'";

        public DynParamQuotedString(string quotedString) : this(quotedString, DefaultQuoteCharacter) { }
        public DynParamQuotedString(string quotedString, string quoteCharacter)
        {
            OriginalString = quotedString;
            m_quoteCharacter = quoteCharacter;
        }

        public string OriginalString { get; set; }
        readonly string m_quoteCharacter;

        public override string ToString()
        {
            // I'm sure this is missing some other characters that need to be escaped. Feel free to add more:
            if (Regex.IsMatch(OriginalString, @"\s|\(|\)|""|'"))
            {
                return string.Format("{1}{0}{1}", OriginalString.Replace(m_quoteCharacter, string.Format("{0}{0}", m_quoteCharacter)), m_quoteCharacter);
            }
            return OriginalString;
        }

        public static string[] GetQuotedStrings(IEnumerable<string> values)
        {
            var returnList = new List<string>();
            foreach (string currentValue in values)
            {
                returnList.Add((new DynParamQuotedString(currentValue)).ToString());
            }
            return returnList.ToArray();
        }
    }
}