using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class JObjectExtension
    {
        public static (string type, string indexed) GetMapping(this JObject mapping, string path)
        {
            var etype = "";
            if (mapping.First is JProperty jp)
                etype = jp.Name;

            var mappingPath = $"{etype}.properties.{path.Replace(".", ".properties.")}";
            var term = mapping.SelectToken(mappingPath);
            var type = term["type"];
            var index = term["index"];
            return (type.ToEmptyString(), index.ToEmptyString());
        }
    }
}