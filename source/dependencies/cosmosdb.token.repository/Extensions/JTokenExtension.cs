using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.CosmosDbRepository.Extensions
{
    public static class JTokenExtension
    {
        public static JToken CreateIdNodeWith(this JToken token, string src="$.WebId")
        {
            var id = token.SelectToken(src);
            token.First.AddBeforeSelf(new JProperty("id", id));
            return token;
        }
    }
}