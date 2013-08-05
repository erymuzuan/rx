using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Bespoke.SphCommercialSpaces.Domain
{
    public static class JsonSerializerService
    {

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
        public static T DeserializeFromJson<T>(string json)
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
        public static string ToJsonString<T>(this T value)
        {
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            return JsonConvert.SerializeObject(value, setting);
        }


    }
}
