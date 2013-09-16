using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

#if !SILVERLIGHT
using System.Xml.Linq;
using System.Data.SqlTypes;
#endif

namespace Bespoke.Sph.Domain
{
    public static class XmlSerializerService
    {
        /// <summary>
        /// Clone object, deep copy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Clone<T>(this T source) where T : class
        {
            if (null == source) throw new ArgumentNullException("source");
            using (var stream = new MemoryStream())
            {
                ToXmlStream(stream, source);
                stream.Position = 0;
                var clone = DeserializeFromXml<T>(stream);

                return clone;
            }

        }

        public static TOutput Convert<TInput, TOutput>(TInput input)
            where TInput : class
            where TOutput : class
        {
            using (var stream = new MemoryStream())
            {
                ToXmlStream(stream, input);
                stream.Position = 0;
                var clone = DeserializeFromXml<TOutput>(stream);

                return clone;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlReader"></param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(XmlReader xmlReader)
        {
            var ser = GetDefaultSerializer(typeof(T));
            return (T)ser.Deserialize(xmlReader);
        }
#if !SILVERLIGHT

        public static T DeserializeFromXml<T>(SqlXml sql)
        {
            if (sql.IsNull) return default(T);

            using (var xmlReader = sql.CreateReader())
            {
                var ser = GetDefaultSerializer(typeof(T));
                return (T)ser.Deserialize(xmlReader);
            }
        }

#endif
        public static T DeserializeFromXmlWithId<T>(string xmlString, int id) where T : class
        {
            var item = DeserializeFromXml<T>(xmlString); 
            var propId = typeof(T).GetProperties().Single(p => p.Name == typeof(T).Name + "Id");
            propId.SetValue(item, id);
            return item;
        }

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


        public static object DeserializeFromXml(this XElement xml, string type)
        {
            var t = Type.GetType(type);
            return DeserializeFromXml(xml, t);

        }

        public static object DeserializeFromXml(this XElement xml, Type type)
        {
            var xmlString = xml.ToString();
            if (string.IsNullOrEmpty(xmlString))
            {
                return null;
            }
            var ser = GetDefaultSerializer(type);
            //
            // to write xml to memory stream and deserialize it
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(xmlString);
                    writer.Flush();

                    stream.Position = 0;
                    var data = ser.Deserialize(stream);
                    return data;
                }
            }
        }


        /// <summary>
        /// Desrialize from stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(this Stream stream) where T : class
        {
            if (null == stream)
            {
                return default(T);
            }

            XmlSerializer ser = GetDefaultSerializer(typeof(T));
            //
            // to write xml to memory stream and deserialize it
            stream.Position = 0;
            var data = ser.Deserialize(stream) as T;
            return data;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ToXmlString<T>(T value)
        {
            XmlSerializer serializer = GetDefaultSerializer(value.GetType());
            using (var writer = new StringWriter())
            {
                
                serializer.Serialize(writer, value);
                writer.Flush();
                return writer.ToString();
            }
        }


        private static readonly Dictionary<Type, XmlSerializer> m_serializers = new Dictionary<Type, XmlSerializer>(16);
        private static XmlSerializer GetDefaultSerializer(Type type)
        {
            if (!m_serializers.ContainsKey(type))
            {
                var serializer = new XmlSerializer(type, Strings.DEFAULT_NAMESPACE);
                if (!m_serializers.ContainsKey(type)) m_serializers.Add(type, serializer); // for the race condition
            }

            return m_serializers[type];
        }

        public static string ToXmlString(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            XmlSerializer serializer = GetDefaultSerializer(value.GetType());

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, value);
                writer.Flush();
                return writer.ToString();
            }
        }

        /// <summary>
        /// Just a helper for the XmlSerializerService.ToXmlString(value)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToXmlString(this DomainObject value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var serializer = GetDefaultSerializer(value.GetType());
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, value);
                writer.Flush();
                return writer.ToString();
            }
        }
        /// <summary>
        /// Just a helper for the XmlSerializerService.ToXmlString(value)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="serializerType">If you want to use the base type</param>
        /// <returns></returns>
        public static string ToXmlString(this DomainObject value, Type serializerType)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var serializer = GetDefaultSerializer(serializerType);
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, value);
                writer.Flush();
                return writer.ToString();
            }
        }

#if !SILVERLIGHT
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
        /// Just a helper for the XmlSerializerService.ToXmlString(value)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XElement ToXElement(this DomainObject value)
        {
            XmlSerializer serializer = GetDefaultSerializer(value.GetType());
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, value);
                writer.Flush();
                string xml = writer.ToString();
                return XElement.Parse(xml);
            }
        }
        /// <summary>
        /// Just a helper for the XmlSerializerService.ToXmlString(value)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="serializedType">the type serilized to</param>
        /// <returns></returns>
        public static XElement ToXElement(this DomainObject value, Type serializedType)
        {
            XmlSerializer serializer = GetDefaultSerializer(serializedType);
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, value);
                writer.Flush();
                string xml = writer.ToString();
                return XElement.Parse(xml);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static SqlXml ToSqlXml<T>(T value)
        {
            XmlSerializer serializer = GetDefaultSerializer(value.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, value);
                stream.Position = 0;
                return new SqlXml(stream);
            }
        }


#endif
        /// <summary>
        /// 
        /// </summary>
        public static void ToXmlStream<T>(Stream stream, T value)
        {
            XmlSerializer serializer = GetDefaultSerializer(value.GetType());
            serializer.Serialize(stream, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void ToXmlStream(Stream stream, object value)
        {
            var serializer = GetDefaultSerializer(value.GetType());
            serializer.Serialize(stream, value);
        }
        
#if !SILVERLIGHT
        private static string UTF8ByteArrayToString(byte[] characters)
        {
            var encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Serialize the objects into UTF8 encoded XML string
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static string ToUtf8EncodedXmlString(object graph)
        {
            if (graph == null) return string.Empty;
            
            var strm = new MemoryStream();
            var xs = GetDefaultSerializer(graph.GetType());
            var xmlTextWriter = new XmlTextWriter(strm, Encoding.UTF8);
            xs.Serialize(xmlTextWriter, graph);
            strm = (MemoryStream)xmlTextWriter.BaseStream;
            string xml = UTF8ByteArrayToString(strm.ToArray());

            strm.Dispose();
            xmlTextWriter.Close();

            return xml;
        }

#endif

        /// <summary>
        /// This returns an object which the undelrlying type is the typeName
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="graph">graph</param>
        /// <param name="typeName">The actual type to cast to</param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(string graph, string typeName) where T : class
        {
            Type type = Type.GetType(typeName);
            if (string.IsNullOrEmpty(graph))
            {
                return null;
            }
            if (null == type)
            {
#if !SILVERLIGHT
                // ReSharper disable RedundantAssignment
                type = Assembly.LoadFrom("domain.rp.dll").GetType(typeName);
                // ReSharper restore RedundantAssignment
#endif
            }

            XmlSerializer ser = GetDefaultSerializer(typeof(T));
            //
            // to write xml to memory stream and deserialize it
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(graph);
                    writer.Flush();

                    stream.Position = 0;
                    return (T)ser.Deserialize(stream);
                }
            }
        }

#if !SILVERLIGHT
        /// <summary>
        /// Creates and XElement from object graph for used in LINQ queries
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static XElement ToXElement<T>(T graph)
        {
            using (var stream = new MemoryStream())
            {
                ToXmlStream(stream, graph);
                stream.Position = 0;

                using (var reader = XmlReader.Create(stream))
                {
                    var e = XElement.Load(reader);
                    return e;
                }
            }
        }
#endif
    }
}
