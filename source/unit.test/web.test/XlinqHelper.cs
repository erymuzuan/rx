using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace web.test
{
    public static class XLinqHelper
    {


        public static string GetAttributeStringValue(this XElement xml, params string[] elements)
        {
            XNamespace x = XmlNamespace;
            XElement ele = xml;
            for (int i = 0; i < elements.Length - 1; i++)
            {
                ele = ele.Element(x + elements[i]);
                if (null == ele)
                    return null;
            }
            var att = ele.Attribute(elements.Last());
            return null != att ? att.Value : null;
        }

        public static string GetElementStringValue(this XElement xml, params string[] elements)
        {
            XNamespace x = XmlNamespace;
            XElement ele = xml;
            foreach (var s in elements)
            {
                ele = ele.Element(x + s);
                if (null == ele)
                    return null;

            }
            return ele.Value;
        }
        public static string[] GetElementStringValues(this XElement xml, params string[] elements)
        {
            XNamespace x = XmlNamespace;
            XElement ele = xml;
            foreach (var s in elements)
            {
                ele = ele.Element(x + s);
                if (null == ele)
                    return null;

            }
            return (from s in ele.Elements(x + "string")
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

        public const string XmlNamespace = "http://www.bespoke.com.my/";

        /// <summary>
        /// For full domain name, just get the part of the SAMaccount name
        /// </summary>
        /// <param name="threadUserName"></param>
        /// <returns></returns>
        public static string RemoveDomainName(this string threadUserName)
        {
            if (string.IsNullOrEmpty(threadUserName))
                throw new ArgumentNullException("threadUserName");
            if (threadUserName.Contains("\\"))
            {
                return threadUserName.Split(new[] { '\\' }).Last();
            }

            if (threadUserName.Contains("@"))
            {
                return threadUserName.Split(new[] { '@' }).First();
            }
            return threadUserName;
        }


        /// <summary>
        /// Just a helper for the XmlSerializerService.ToXmlString(value)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XElement ToXElement(this object value)
        {
            var serializer = GetDefaultSerializer(value.GetType());
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, value);
                writer.Flush();
                var xml = writer.ToString();
                return XElement.Parse(xml);
            }
        }


        private static readonly Dictionary<Type, XmlSerializer> m_serializers = new Dictionary<Type, XmlSerializer>(32);
        private static XmlSerializer GetDefaultSerializer(Type type)
        {
            if (!m_serializers.ContainsKey(type))
            {
                var serializer = new XmlSerializer(type, XmlNamespace);
                try
                {
                    m_serializers.Add(type, serializer);
                }
                catch (ArgumentException)
                {
                    return new XmlSerializer(type, XmlNamespace);
                }
            }

            return m_serializers[type];
        }


    }
}
