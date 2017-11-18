using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public class RangeJProperty : JProperty
    {
        public RangeJProperty(JProperty prop) : base("range", prop.Value)
        {
        }
    }
}