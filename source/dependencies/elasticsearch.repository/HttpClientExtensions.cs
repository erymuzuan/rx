using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository
{
    internal static class HttpClientExtensions
    {
        public static Task<string> ReadContentAsString(this HttpResponseMessage response, bool ensureSuccessStatusCode = true, string exceptionMessage = "Cannot execute query on es ")
        {
            if (ensureSuccessStatusCode)
                response.EnsureSuccessStatusCode();
            if (!(response.Content is StreamContent content)) throw new Exception(exceptionMessage);
            return content.ReadAsStringAsync();
        }
        public static async Task<JObject> ReadContentAsJson(this HttpResponseMessage response, bool ensureSuccessStatusCode = true, string exceptionMessage = "Cannot execute query on es ")
        {
            var text = await response.ReadContentAsString(ensureSuccessStatusCode, exceptionMessage);
            return JObject.Parse(text);
        }
    }
}
