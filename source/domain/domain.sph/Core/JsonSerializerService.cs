using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Bespoke.Sph.Domain
{
    public static class JsonSerializerService
    {

        public static string GetJsonSchema(this Type t)
        {
            var schema = new StringBuilder();
            var properties = from p in t.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                             where p.DeclaringType != typeof(DomainObject)
                             select p.GetJsonSchema();

            schema.Append($@"
{{
  ""type"": ""object"",
  ""properties"": {{
   {properties.ToString(",\r\n")}
  }}
}}");
            return schema.ToString();
        }

        public static string GetJsonSchema(this PropertyInfo prop)
        {
            var elements = new Dictionary<string, string>();
            var typeBags = new Dictionary<Type, string>
            {
                {typeof(string),"string"},
                {typeof(DateTime),"string"},
                {typeof(System.Xml.XmlDocument),"string"},
                {typeof(int),"integer"},
                {typeof(byte),"integer"},
                {typeof(short),"integer"},
                {typeof(long),"integer"},
                {typeof(decimal),"number"},
                {typeof(double),"integer"},
                {typeof(float),"number"},
                {typeof(bool),"boolean"},
            };
            var formatBags = new Dictionary<Type, string>
            {
                {typeof(DateTime),"date-time"}
            };



            var type = prop.PropertyType;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }
            elements.AddIfNotExist("required", "true");
            elements.Add("type", string.Empty);
            if (typeBags.ContainsKey(type))
                elements["type"] =$@"""{typeBags[type]}""";
            if (formatBags.ContainsKey(type))
                elements.AddIfNotExist("format", $@"""{formatBags[type]}""");

            if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(prop.PropertyType);
                if (type == typeof(DateTime))
                    elements.AddIfNotExist("format", @"""date-time""");
                else if (type.BaseType == typeof(Enum))
                    elements.AddIfNotExist("enum", "[" + Enum.GetNames(type).ToString(",", x => $@"""{x}""") + "]");

                elements["type"] = $@"[{elements["type"]}, ""null""]";
            }

            if (type.BaseType == typeof(Enum))
            {
                elements.AddIfNotExist("enum", "[" + Enum.GetNames(type).ToString(",", x => $@"""{x}""") + "]");
            }

            // most likely IList
            if (string.IsNullOrWhiteSpace(elements["type"]) && type.IsGenericType)
            {
                // TODO : Need a more robust way to check for IEnumerable<>
                var array = type.GetGenericTypeDefinition().GetInterfaces().Any(x => x.FullName == "System.Collections.IList");
                if (array)
                {
                    var itemType = type.GenericTypeArguments[0];
                    if (itemType.IsClass && itemType != typeof(string))
                    {
                        var children = from p in itemType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                       where p.DeclaringType != typeof(DomainObject)
                                       select p.GetJsonSchema();
                        elements["type"] = @"[""array"", ""null""]";
                        var items = $@"{{
                        ""type"":[""object"", ""null""],
                        ""properties"" : {{{ children.ToString(",\r\n", x => $"{x}")} }}
                    }}";
                        elements.Add("items", items);

                    }
                    else
                    {
                        // TODO, now get back the type
                        elements["type"] = @"[""array"", ""null""]";
                        var items = $@"{{
                        ""type"":[""{typeBags[itemType]}"", ""null""]
                    }}";
                        elements.Add("items", items);

                    }


                }
            }

            if (string.IsNullOrWhiteSpace(elements["type"]))
            {
                var children = from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                               where p.DeclaringType != typeof(DomainObject)
                               select p.GetJsonSchema();
                elements.AddIfNotExist("required", "true");
                elements["type"] = @"[""object"", ""null""]";
                elements.Add("properties", "{" + children.ToString(",\r\n", x => $"{x}") + "}");
            }


            var code = new StringBuilder();
            code.Append($@" ""{prop.Name}"": {{
");
            code.JoinAndAppendLine(elements.Keys, ",\r\n", x => $@"""{x}"" : {elements[x]}");
            code.Append("}");
            return code.ToString();
        }
       

        /// <summary>
        /// Clone object, deep copy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T JsonClone<T>(this T source) where T : class
        {
            if (null == source) throw new ArgumentNullException(nameof(source));

            var json = ToJsonString(source);
            var clone = json.DeserializeFromJson<T>();

            return clone;


        }

        public static async Task<T> DeserializeJsonAsync<T>(this Stream stream)
        {
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            if (null == stream) throw new ArgumentNullException(nameof(stream));
            using (var sr = new StreamReader(stream))
            {
                string result = await sr.ReadToEndAsync();
                return JsonConvert.DeserializeObject<T>(result, setting);
            }

        }


        public static T DeserializeJson<T>(this Stream stream)
        {
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            if (null == stream) throw new ArgumentNullException(nameof(stream));

            if (null == stream) throw new ArgumentNullException(nameof(stream));
            using (var sr = new StreamReader(stream))
            {
                string result = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(result, setting);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json">The json string</param>
        /// <returns></returns>
        public static T DeserializeFromJson<T>(this string json)
        {
            try
            {
                var setting = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                return JsonConvert.DeserializeObject<T>(json, setting);
            }
            catch (OutOfMemoryException)
            {
                var temp = Path.GetTempFileName();
                File.WriteAllText(temp, json);
                using (var stream = File.Open(temp, FileMode.Open))
                {
                    return stream.DeserializeFromJson<T>();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file">The path to the json file</param>
        /// <returns></returns>
        public static T DeserializeFromJsonFile<T>(this string file) where T : Entity
        {
            if (!File.Exists(file))
                throw new ArgumentException("Cannot find file " + file, nameof(file));
            try
            {
                var source = StoreAsSourceAttribute.GetAttribute<T>();
                if (null == source)
                    throw new InvalidOperationException(typeof(T) + " does not have StoreAsSourceAttribute");
                var readAllText = source.HasDerivedTypes;
                if (readAllText)
                {
                    var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    try
                    {
                        return JsonConvert.DeserializeObject<T>(File.ReadAllText(file), setting);
                    }
                    catch (IOException e) when (e.Message.Contains("because it is being used by another process"))
                    {
                        Thread.Sleep(500);
                        return file.DeserializeFromJsonFile<T>();
                    }
                }

                using (var reader = File.OpenText(file))
                {
                    var serializer = new JsonSerializer();
                    return (T)serializer.Deserialize(reader, typeof(T));
                }
            }
            catch (JsonReaderException e)
            {
                ObjectBuilder.GetObject<ILogger>().Log(new LogEntry(e, new[] { "File", file }));
                throw new Exception($"Cannot deserialize the content of {file} to {typeof(T).FullName}", e);
            }
            catch (JsonSerializationException e)
            {
                ObjectBuilder.GetObject<ILogger>().Log(new LogEntry(e, new[] { "File", file }));
                throw new Exception($"Cannot deserialize the content of {file} to {typeof(T).FullName}", e);
            }

        }


        public static T DeserializeFromJson<T>(this Stream stream)
        {
            var json = StreamToString(stream);
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            return JsonConvert.DeserializeObject<T>(json, setting);
        }
        public static T Clone<T>(this T item)
        {
            var json = item.ToJsonString();
            return json.DeserializeFromJson<T>();
        }


        public static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (var reader = new StreamReader(stream, Encoding.Unicode))
            {
                return reader.ReadToEnd();
            }
        }

        public static T DeserializeFromJsonWithId<T>(string json, int id) where T : class
        {
            var item = DeserializeFromJson<T>(json);
            var propId = typeof(T).GetProperties().Single(p => p.Name == typeof(T).Name + "Id");
            propId.SetValue(item, id);
            return item;
        }

        public static string ToJson<T>(this T value)
        {
            var setting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            setting.Formatting = Formatting.Indented;
            return JsonConvert.SerializeObject(value, setting);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ToJsonString<T>(this T value, Formatting format = Formatting.None)
        {
            var setting = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            setting.Formatting = format;
            return JsonConvert.SerializeObject(value, setting);
        }
        public static string ToJsonString<T>(this T value, bool pretty)
        {
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            setting.Formatting = pretty ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(value, setting);
        }
    }
}
