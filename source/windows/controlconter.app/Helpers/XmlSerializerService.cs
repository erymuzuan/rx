using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;

namespace Bespoke.Sph.ControlCenter.Helpers
{
    public static class XmlSerializerService
    {
        public static T Deserialize<T>(this string xmlString) where T : class
        {
            XElement xe = XElement.Parse(xmlString);
            return xe.Deserialize<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ToXmlString<T>(T value)
        {
            var serializer = GetDefaultSerializer(value.GetType());
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, value);
                writer.Flush();
                return writer.ToString();
            }
        }


        private static Dictionary<Type, XmlSerializer> m_serializers = new Dictionary<Type, XmlSerializer>(16);
        private static XmlSerializer GetDefaultSerializer(Type type)
        {
            if (!m_serializers.ContainsKey(type))
            {
                var serializer = new XmlSerializer(type, Strings.DefaultNamespace);
                if (!m_serializers.ContainsKey(type)) m_serializers.Add(type, serializer); // for the race condition
            }

            return m_serializers[type];
        }

        /// <summary>
        /// Just a helper for the XmlSerializerService.ToXmlString(value)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToXmlString(this DomainObject value)
        {
            var serializer = GetDefaultSerializer(value.GetType());
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, value);
                writer.Flush();
                return writer.ToString();
            }
        }

        /// <summary>
        /// Helps to deserialize the XElement to DomainObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this XElement element) where T : class
        {
            return DeserializeFromXml<T>(element.ToString());
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(string xmlString) where T : class
        {
            if (string.IsNullOrEmpty(xmlString))
            {
                return default(T);
            }

            var ser = GetDefaultSerializer(typeof(T));
            //
            // to write xml to memory stream and deserialize it
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(xmlString);
                    writer.Flush();

                    stream.Position = 0;
                    var data = ser.Deserialize(stream) as T;
                    return data;
                }
            }
        }
        
    }
}
