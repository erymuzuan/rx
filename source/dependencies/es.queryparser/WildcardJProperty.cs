using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public class WildcardJProperty : JProperty
    {
        public WildcardJProperty(JProperty prop) : base("wildcard", prop.Value)
        {
        }
    }
}