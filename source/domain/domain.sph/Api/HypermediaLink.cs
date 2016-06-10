using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Domain.Api
{
    public class HypermediaLink
    {
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("rel")]
        public string Rel { get; set; }
        [JsonProperty("href")]
        public string Href { get; set; }
        [JsonProperty("desc")]
        public string Description { get; set; }
        [JsonProperty("doc")]
        public string DocumentationUrl { get; set; }

        public override string ToString()
        {
            var source = JObject.Parse(this.ToJson());
            if (string.IsNullOrWhiteSpace(Method)) source.Remove("method");
            if (string.IsNullOrWhiteSpace(Rel)) source.Remove("rel");
            if (string.IsNullOrWhiteSpace(Href)) source.Remove("href");
            if (string.IsNullOrWhiteSpace(Description)) source.Remove("desc");
            if (string.IsNullOrWhiteSpace(DocumentationUrl)) source.Remove("doc");

            return source.ToString();
        }

        public string GetJsonBody()
        {
            var json = new StringBuilder(this.ToString());
            json.Remove(json.Length - 1, 1);
            json.Remove(0, 1);

            return json.ToString();
        }
    }
}