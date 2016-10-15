using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mono.Cecil;
using Newtonsoft.Json;


namespace Bespoke.Sph.Domain
{
    public static class JsonSerializerService
    {
        public static string GetJsonSchema(this TypeDefinition t)
        {
            var elements = from p in t.LoadProperties()
                           where p.DeclaringType.FullName != typeof(DomainObject).FullName
                           select p.GetJsonSchema();

            var schema = new StringBuilder();
            schema.Append($@"
{{
  ""type"": ""object"",
  ""properties"": {{
   {elements.ToString(",\r\n")}
  }}
}}");
            return schema.ToString();
        }

        public static string GetJsonSchema(this PropertyDefinition prop)
        {
            var elements = new Dictionary<string, string>();
            var typeBags = new Dictionary<string, string>
            {
                {typeof(string).FullName,"string"},
                {typeof(DateTime).FullName,"string"},
                {typeof(System.Xml.XmlDocument).FullName,"string"},
                {typeof(int).FullName,"integer"},
                {typeof(byte).FullName,"integer"},
                {typeof(short).FullName,"integer"},
                {typeof(long).FullName,"integer"},
                {typeof(decimal).FullName,"number"},
                {typeof(double).FullName,"integer"},
                {typeof(float).FullName,"number"},
                {typeof(bool).FullName,"boolean"},
            };
            var formatBags = new Dictionary<string, string>
            {
                {typeof(DateTime).FullName,"date-time"}
            };


            var type = prop.PropertyType;
            var td = type as TypeDefinition;
            var nullable = typeof(Nullable<>).FullName;
            var generic = type as GenericInstanceType;
            var genericElementType = generic?.ElementType.FullName;
            if (null != generic && genericElementType == nullable)
            {
                type = generic.GenericArguments.First();
                Console.WriteLine("Nullable of type " + type.FullName);
            }
            elements.AddIfNotExist("required", "true");
            elements.Add("type", string.Empty);
            if (typeBags.ContainsKey(type.FullName))
                elements["type"] = $@"""{typeBags[type.FullName]}""";
            if (formatBags.ContainsKey(type.FullName))
                elements.AddIfNotExist("format", $@"""{formatBags[type.FullName]}""");

            if (null != generic && genericElementType == nullable)
            {
                type = generic.GenericArguments.First();
                td = type as TypeDefinition;

                if (typeof(DateTime).IsOfType(type))
                {
                    elements.AddIfNotExist("format", @"""date-time""");
                }
                if (null != td && td.BaseType.IsOfType(typeof(Enum)))
                {
                    elements.AddIfNotExist("enum", "[" + td.Fields.Where(x => x.HasConstant).Select(x => x.Name).ToString(",", x => $@"""{x}""") + "]");
                    elements["type"] = @"""string""";
                }

                elements["type"] = $@"[{elements["type"]}, ""null""]";
            }

            if (null != td && td.BaseType.IsOfType(typeof(Enum)))
            {
                elements.AddIfNotExist("enum", "[" + td.Fields.Where(x => x.HasConstant).Select(x => x.Name).ToString(",", x => $@"""{x}""") + "]");
            }

            // most likely IList, or so
            if (string.IsNullOrWhiteSpace(elements["type"]) && null != generic)
            {
                var propertyType = type.LoadTypeDefinition();
                var array = propertyType?.HasInterface(typeof(IList));
                if (array ?? false)
                {
                    var itemType = generic.GenericArguments.First();
                    td = type as TypeDefinition;
                    if (!itemType.IsOfType(typeof(string)) && !itemType.IsPrimitive/* and IsClass*/)
                    {
                        if (null == td && prop.DeclaringType.Scope == itemType.Scope)
                        {
                            td = itemType.LoadTypeDefinition();
                        }
                        if (null != td)
                        {
                            var children = from p in td.Properties
                                           where p.DeclaringType.FullName != typeof(DomainObject).FullName
                                           select p.GetJsonSchema();
                            elements["type"] = @"[""array"", ""null""]";
                            var items = $@"{{
					""type"":[""object"", ""null""],
					""properties"" : {{{ children.ToString(",\r\n", x => $"{x}")} }}
				}}";
                            elements.Add("items", items);
                        }
                    }
                    else
                    {
                        // TODO, now get back the type
                        elements["type"] = @"[""array"", ""null""]";
                        var items = $@"{{
					""type"":[""{typeBags[itemType.FullName]}"", ""null""]
				}}";
                        elements.Add("items", items);

                    }
                }
            }


            if (null != td && string.IsNullOrWhiteSpace(elements["type"]))
            {
                var children = from p in td.Properties
                                   //where p.DeclaringType != typeof(DomainObject)
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
            var option = PersistenceOptionAttribute.GetAttribute<T>();
            if (null == option)
                throw new InvalidOperationException(typeof(T) + " does not have PersistenceOptionAttribute");
            if (!option.IsSource)
                throw new InvalidOperationException(typeof(T) + " does not have PersistenceOptionAttribute set to IsSource=true");
            if (!File.Exists(file))
                throw new ArgumentException("Cannot find file " + file, nameof(file));
            try
            {

                var readAllText = option.HasDerivedTypes;
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
