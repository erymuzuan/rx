using System;
using System.Linq;
using System.Xml.Linq;
namespace Bespoke.Migration.Sph
{
    public static class XLinqHelper
    {
        public static string GetAttributeStringValue(this XElement xml, params string[] elements)
        {
            XElement ele = xml;
            for (int i = 0; i < elements.Length - 1; i++)
            {
                ele = ele.Element(elements[i]);
                if (null == ele)
                    return null;
            }
            var att = ele.Attribute(elements.Last());
            return null != att ? att.Value : null;
        }

        public static string GetElementStringValue(this XElement xml, params string[] elements)
        {
            XElement ele = xml;
            foreach (var s in elements)
            {
                ele = ele.Element(s);
                if (null == ele)
                    return null;

            }
            return ele.Value;
        }
        public static string[] GetElementStringValues(this XElement xml, params string[] elements)
        {
            XElement ele = xml;
            foreach (var s in elements)
            {
                ele = ele.Element(s);
                if (null == ele)
                    return null;

            }
            return (from s in ele.Elements("string")
                    select s.Value).ToArray();
        }


        public static DateTime GetAttributeDateTimeValue(this XElement xml, params string[] elements)
        {
            var sv = xml.GetAttributeStringValue(elements);
            DateTime val;
            if (DateTime.TryParse(sv, out val))
                return val;
            return DateTime.MinValue;
        }
        public static bool GetAttributeBooleanValue(this XElement xml, params string[] elements)
        {
            var sv = xml.GetAttributeStringValue(elements);
            bool val;
            if (bool.TryParse(sv, out val))
                return val;
            return false;
        }

        public static int GetAtrributeInt32Value(this XElement xml, params string[] elements)
        {
            var sv = xml.GetAttributeStringValue(elements);
            int val;
            if (int.TryParse(sv, out val))
                return val;
            return 0;
        }

        public static int GetElementInt32Value(this XElement xml, params string[] elements)
        {
            var sv = xml.GetElementStringValue(elements);
            int val;
            if (int.TryParse(sv, out val))
                return val;
            return 0;
        }

        public static DateTime GetElementDateTimeValue(this XElement xml, params string[] elements)
        {
            var sv = xml.GetElementStringValue(elements);
            DateTime val;
            if (DateTime.TryParse(sv, out val))
                return val;
            return DateTime.MinValue;
        }

        public static DateTime? GetElementNullableDateTimeValue(this XElement xml, params string[] elements)
        {
            var sv = xml.GetElementStringValue(elements);
            DateTime val;
            if (DateTime.TryParse(sv, out val))
            {
                if (val == DateTime.MinValue) return null;
                return val;
            }
            return null;
        }



    }
}
