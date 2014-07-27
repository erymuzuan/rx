using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Bespoke.Sph.Domain
{
    public static class JsonSerializerService
    {

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


        public static IEnumerable<Member> GenerateSchema(string json)
        {
            var jo = JObject.Parse(json);
            var ed = new EntityDefinition();

            foreach (var c in jo.Children())
            {
                var p = c as JProperty;
                if(null == p)continue;

                var member = ExtractChildMember(p, null);
                ed.MemberCollection.Add(member);
            }
            return ed.MemberCollection;
        }

        private static Member ExtractChildMember(JToken jt, Member parent)
        {
            var m = new Member();
            var p = jt as JProperty;
            if (null == p)
            {

                // array of objects
                if (jt.Type == JTokenType.Object)
                {
                    foreach (var c in jt.Children())
                    {
                        var cm = ExtractChildMember(c, m);

                        if (parent.MemberCollection.All(x => x.Name != cm.Name))
                            parent.MemberCollection.Add(cm);
                    }
                }
                return m;
            }

            m.Name = p.Name;
            var typeName = p.Value.Type;
            var type = Type.GetType(string.Format("System.{0}, mscorlib", p.Value.Type));
            if (typeName == JTokenType.Integer)
                type = typeof(int);//"
            if (typeName == JTokenType.Null)
                type = typeof(string);//"
            if (typeName == JTokenType.Date)
                type = typeof(DateTime);//"
            if (null != type)
                m.Type = type;

            if (p.Value.Type == JTokenType.Object || p.Value.Type == JTokenType.Array)
            {
                foreach (var c in p.Value.Children())
                {
                    ExtractChildMember(c, m);
                }
            }
            if (null != parent)
                parent.MemberCollection.Add(m);

            return m;
        }
    }
}
