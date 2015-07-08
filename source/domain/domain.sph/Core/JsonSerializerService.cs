using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;


namespace Bespoke.Sph.Domain
{
    public static class JsonSerializerService
    {
        public static JsonSchema GetJsonSchemaFromObject2(Type type)
        {
            var generator = new JsonSchemaGenerator();
            var schema = generator.Generate(type);
            schema = MapSchemaTypes(schema, type);

            return schema;
        }
        private static JsonSchema MapSchemaTypes(JsonSchema schema, Type type)
        {
            foreach (var prop in schema.Properties)
            {
                var fieldType = type.GetProperty(prop.Key).PropertyType;

                if (fieldType.IsGenericType
                    && fieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var underlyingType = Nullable.GetUnderlyingType(fieldType);
                    if (underlyingType == typeof(DateTime))
                        prop.Value.Format = "date-time";
                    else if (underlyingType.BaseType == typeof(Enum))
                        prop.Value.Enum = new JArray(Enum.GetNames(underlyingType).Cast<object>());
                }
                else if (fieldType == typeof(DateTime))
                {
                    prop.Value.Format = "date-time";
                }
                else if (fieldType.BaseType == typeof(Enum))
                {
                    prop.Value.Enum = new JArray(Enum.GetNames(fieldType).Cast<object>());
                }
                else if (prop.Value.Items != null && prop.Value.Items.Any())
                {
                    foreach (var item in prop.Value.Items)
                    {
                        var arg = fieldType.GetGenericArguments();
                        if (arg.Any())
                            fieldType = arg[0];

                        MapSchemaTypes(item, fieldType);
                    }
                }
                else if (prop.Value.Properties != null && prop.Value.Properties.Any())
                {
                    MapSchemaTypes(prop.Value, fieldType);
                }
            }
            return schema;
        }

        public static JsonSchema GetJsonSchemaFromObject(Type type)
        {
            var generator = new JsonSchemaGenerator();
            var schema = generator.Generate(type);

            foreach (var jp in schema.Properties)
            {
                var prop = type.GetProperty(jp.Key);
                if (null == prop) continue;

                if (prop.PropertyType.IsGenericType
                    && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
                    if (underlyingType == typeof(DateTime))
                        jp.Value.Format = "date-time";
                    else if (underlyingType.BaseType == typeof(Enum))
                        jp.Value.Enum = new JArray(Enum.GetNames(underlyingType).Cast<object>());
                }
                else if (prop.PropertyType == typeof(DateTime))
                {
                    jp.Value.Format = "date-time";
                }

                if (prop.PropertyType.Namespace == type.Namespace)
                {
                    SetJsonSchemaDateTimeFormat(prop, jp);
                }
                if (prop.PropertyType.Namespace == typeof(Entity).Namespace)
                {
                    SetJsonSchemaDateTimeFormat(prop, jp);
                }
            }

            return schema;
        }

        private static void SetJsonSchemaDateTimeFormat(PropertyInfo parentProperty, KeyValuePair<string, JsonSchema> parentSchema)
        {
            if (null == parentSchema.Value.Properties) return;
            foreach (var jp in parentSchema.Value.Properties)
            {
                var prop = parentProperty.PropertyType.GetProperty(jp.Key);
                if (null == prop) continue;

                if (prop.PropertyType.IsGenericType
                    && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
                    if (underlyingType == typeof(DateTime))
                        jp.Value.Format = "date-time";
                    else if (underlyingType.BaseType == typeof(Enum))
                        jp.Value.Enum = new JArray(Enum.GetNames(underlyingType).Cast<object>());
                }
                else if (prop.PropertyType == typeof(DateTime))
                {
                    jp.Value.Format = "date-time";
                }

                if (prop.PropertyType.Namespace == parentProperty.PropertyType.Namespace)
                {
                    SetJsonSchemaDateTimeFormat(prop, jp);
                }
                if (prop.PropertyType.Namespace == typeof(Entity).Namespace)
                {
                    SetJsonSchemaDateTimeFormat(prop, jp);
                }
            }

        }

        /// <summary>
        /// Clone object, deep copy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T JsonClone<T>(this T source) where T : class
        {
            if (null == source) throw new ArgumentNullException("source");

            var json = ToJsonString(source);
            var clone = json.DeserializeFromJson<T>();

            return clone;


        }

        public async static Task<T> DeserializeJsonAsync<T>(this Stream stream)
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
                var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
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
        public static T DeserializeFromJsonFile<T>(this string file) where T: Entity
        {
            if (!File.Exists(file))
                throw new ArgumentException("Cannot find file " + file, nameof(file));
            try
            {
                var readAllText = StoreAsSourceAttribute.GetAttribute<T>().HasDerivedTypes;
                if (readAllText)
                {
                    var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    return JsonConvert.DeserializeObject<T>(File.ReadAllText(file), setting);
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
