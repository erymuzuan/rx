using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public class PrefixJProperty : JProperty
    {
        public PrefixJProperty(JProperty prop) : base("prefix", prop.Value)
        {
        }
    }
}