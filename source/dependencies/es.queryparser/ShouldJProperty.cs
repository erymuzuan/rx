using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public class ShouldJProperty : JProperty
    {
        public ShouldJProperty(JProperty prop) : base("should", prop.Value)
        {
        }
        public ShouldJProperty(JToken prop) : base("should", prop.Children())
        {
        }
    }
}