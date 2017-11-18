using System;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public static class JTokenExtension
    {
        public static object GetTokenValue(this JToken json,string path)
        {
            var token = json.SelectToken(path);
            return null != token ? token.Value<object>() : default;
        }
        public static T GetTokenValue<T>(this JToken json,string path)
        {
            var token = json.SelectToken(path);
            return null != token ? token.Value<T>() : default;
        }

        public static IJEnumerable<JToken> SelectToken2(this JToken prop, string path)
        {
            var token = prop.SelectToken(path);
            if(null == token)
                return Array.Empty<JToken>().AsJEnumerable();
            return token.Children();
        }
    }
}