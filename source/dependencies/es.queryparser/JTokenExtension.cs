using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public static class JTokenExtension
    {
        public static T GetTokenValue<T>(this JToken json,string path)
        {
            var token = json.SelectToken(path);
            return null != token ? token.Value<T>() : default;
        }
    }
}