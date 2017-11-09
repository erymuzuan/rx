using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Extensions
{
    internal static class HttpClientExtensions
    {
        public static Task<string> ReadContentAsStringAsync(this HttpResponseMessage response, bool ensureSuccessStatusCode = true, string exceptionMessage = "Cannot execute query on es ")
        {
            if (ensureSuccessStatusCode)
                response.EnsureSuccessStatusCode();
            if (!(response.Content is StreamContent content)) throw new Exception(exceptionMessage);
            return content.ReadAsStringAsync();
        }
        public static async Task<JObject> ReadContentAsJsonAsync(this HttpResponseMessage response, bool ensureSuccessStatusCode = true, string exceptionMessage = "Cannot execute query on es ")
        {
            var text = await response.ReadContentAsStringAsync(ensureSuccessStatusCode, exceptionMessage);
            return JObject.Parse(text);
        }
        public static async Task<LoadOperation<T>> ReadContentAsLoadOperationAsync<T>(this HttpResponseMessage response, int skip = 0, int size = 1, bool ensureSuccessStatusCode = true, string exceptionMessage = "Cannot execute query on es "
            , bool useSourceFields = false)
            where T : DomainObject
        {
            var lo = new LoadOperation<T>();
            var json = await response.ReadContentAsJsonAsync(ensureSuccessStatusCode, exceptionMessage);

            lo.TotalRows = json.SelectToken("$.hits.total").Value<int>();
            lo.CurrentPage = Convert.ToInt32(skip / size + 1);
            lo.PageSize = size;

            var tokens = json.SelectToken("$.hits.hits");

            if (useSourceFields)
            {
                var rows = tokens.Select(t => t.SelectToken("$._source")).Where(x => null != x);
                var fields = from t in rows
                             select MapToDictionary(t);
                lo.Readers.AddRange(fields);
                return lo;
            }


            var list = from t in tokens
                       let src = t.SelectToken("$._source")
                       where null != src
                       select src.ToString().DeserializeFromJson<T>();

            lo.ItemCollection.AddRange(list);

            return lo;
        }

        private static Dictionary<string, object> MapToDictionary(JToken token)
        {
            var reader = new Dictionary<string, object>
            {
                {"Id",token.Parent.Parent["_id"].Value<string>() }
            };
            string name;

            void Loop(JToken jo)
            {
                foreach (var jt in jo.Children())
                {
                    if (jt is JProperty jp)
                    {
                        var key = name + "." + jp.Name;
                        if (jp.HasValues && jp.Value is JObject jo2)
                        {
                            name = key;
                            Loop(jo2);
                        }

                        if (jp.HasValues && jp.Value is JValue jv)
                        {
                            reader.Add(key, jp.Value);
                        }

                    }
                }


            }

            foreach (var jt in token.Children())
            {
                if (jt is JProperty jp)
                {
                    if (jp.HasValues && jp.Value is JObject jo)
                    {
                        name = jp.Name;
                        Loop(jo);
                        continue;
                    }

                    if (jp.HasValues && jp.Value is JValue jv)
                    {
                        reader.Add(jp.Name, jp.Value);
                    }

                }
            }
            return reader;
        }


    }
}
