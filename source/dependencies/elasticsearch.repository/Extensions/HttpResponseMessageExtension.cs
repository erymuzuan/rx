using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class HttpResponseMessageExtension
    {

        public static async Task<LoadOperation<T>> ReadContentAsLoadOperationAsync<T>(this HttpResponseMessage response, QueryDsl query = null, bool ensureSuccessStatusCode = true, string exceptionMessage = "Cannot execute query on es ")
            where T : DomainObject
        {
            var skip = query?.Skip ?? 0;
            var size = query?.Size ?? 1;
            var useSourceFields = query?.Fields.Any() ?? false;
            var lo = new LoadOperation<T>(query);
            var json = await response.ReadContentAsJsonAsync(ensureSuccessStatusCode, exceptionMessage);

            lo.TotalRows = json.SelectToken("$.hits.total").Value<int>();
            lo.CurrentPage = Convert.ToInt32(skip / size + 1);
            lo.PageSize = size;


            var hasAggregates = query?.Aggregates.Any() ?? false;
            if (hasAggregates)
            {
                foreach (var agg in query.Aggregates)
                {
                    agg.ExtractValueFromSearchResult(json);
                }
            }
            
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

            void Loop(JProperty jp)
            {

                if (jp.HasValues && jp.Value is JValue jv)
                {
                    var key = string.IsNullOrWhiteSpace(name) ? jp.Name : name + "." + jp.Name;
                    switch (jv.Value)
                    {
                        case long big:
                            reader.Add(key, Convert.ToInt32(big));
                            break;
                        case int numberInt:
                            reader.Add(key, Convert.ToInt32(numberInt));
                            break;
                        case double number:
                            reader.Add(key, Convert.ToDecimal(number));
                            break;
                        case float number:
                            reader.Add(key, Convert.ToDecimal(number));
                            break;
                    }

                    if (!reader.ContainsKey(key))
                        reader.Add(key, jv.Value);
                    return;
                }
                if (jp.HasValues && jp.Value is JObject jo2)
                {
                    name = string.IsNullOrWhiteSpace(name) ? jp.Name : name + "." + jp.Name;
                    foreach (var child in jo2.Children().OfType<JProperty>())
                    {
                        Loop(child);
                    }
                }
            }

            foreach (var cp in token.Children().OfType<JProperty>())
            {
                name = "";
                Loop(cp);
            }
            return reader;
        }
    }
}
