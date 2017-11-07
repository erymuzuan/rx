using System;
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
        public static async Task<LoadOperation<T>> ReadContentAsLoadOperationAsync<T>(this HttpResponseMessage response, int skip = 0, int size = 1, bool ensureSuccessStatusCode = true, string exceptionMessage = "Cannot execute query on es ")
            where T : DomainObject
        {
            var lo = new LoadOperation<T>();
            var json = await response.ReadContentAsJsonAsync(ensureSuccessStatusCode, exceptionMessage);

            lo.TotalRows = json.SelectToken("$.hits.hits.total").Value<int>();
            lo.CurrentPage = Convert.ToInt32((skip / size) + 1);
            lo.PageSize = size;

            var tokens = json.SelectToken("$.hits.hits");
            var list = from t in tokens
                let src = t.SelectToken("$._source")
                where null != src
                select src.ToString().DeserializeFromJson<T>();

            lo.ItemCollection.AddRange(list);

            return lo;
        }
    }
}
