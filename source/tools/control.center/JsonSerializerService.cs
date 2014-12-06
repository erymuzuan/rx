using System;
using System.Linq;
using System.IO;
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
            var schemaGenerator = new JsonSchemaGenerator();
            var jSchema = schemaGenerator.Generate(type);
            jSchema = MapSchemaTypes(jSchema, type);

            return jSchema;
        }
        private static JsonSchema MapSchemaTypes(JsonSchema jSchema, Type type)
        {
            foreach (var js in jSchema.Properties)
            {
                Type fieldType = type.GetProperty(js.Key).PropertyType;

                if (fieldType.IsGenericType
                    && fieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type underlyingType = Nullable.GetUnderlyingType(fieldType);
                    if (underlyingType == typeof(DateTime))
                        js.Value.Format = "date-time";
                    else if (underlyingType.BaseType == typeof(Enum))
                        js.Value.Enum = new JArray(Enum.GetNames(underlyingType));
                }
                else if (fieldType == typeof(DateTime))
                {
                    js.Value.Format = "date-time";
                }
                else if (fieldType.BaseType == typeof(Enum))
                {
                    js.Value.Enum = new JArray(Enum.GetNames(fieldType));
                }
                else if (js.Value.Items != null && js.Value.Items.Any())
                {
                    foreach (var item in js.Value.Items)
                    {
                        var arg = fieldType.GetGenericArguments();
                        if (arg.Any())
                            fieldType = arg[0];

                        MapSchemaTypes(item, fieldType);
                    }
                }
                else if (js.Value.Properties != null && js.Value.Properties.Any())
                {
                    MapSchemaTypes(js.Value, fieldType);
                }
            }
            return jSchema;
        }

        public static JsonSchema GetJsonSchemaFromObject(Type type)
        {
            var schemaGenerator = new JsonSchemaGenerator();
            JsonSchema jSchema = schemaGenerator.Generate(type);

            foreach (var js in jSchema.Properties)
            {
                Type fieldType = type.GetProperty(js.Key).PropertyType;

                if (fieldType.IsGenericType
                    && fieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type underlyingType = Nullable.GetUnderlyingType(fieldType);
                    if (underlyingType == typeof(DateTime))
                        js.Value.Format = "date-time";
                    else if (underlyingType.BaseType == typeof(Enum))
                        js.Value.Enum = new JArray(Enum.GetNames(underlyingType));
                }
                else if (fieldType == typeof(DateTime))
                {
                    js.Value.Format = "date-time";
                }
            }

            return jSchema;
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
            if (null == stream) throw new ArgumentNullException("stream");
            using (var sr = new StreamReader(stream))
            {
                string result = await sr.ReadToEndAsync();
                return JsonConvert.DeserializeObject<T>(result, setting);
            }

        }


        public static T DeserializeJson<T>(this Stream stream)
        {
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            if (null == stream) throw new ArgumentNullException("stream");

            if (null == stream) throw new ArgumentNullException("stream");
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
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            return JsonConvert.DeserializeObject<T>(json, setting);
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
            using (var reader = new StreamReader(stream, Encoding.UTF8))
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
