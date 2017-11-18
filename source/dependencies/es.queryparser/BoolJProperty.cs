using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public class BoolJProperty : JProperty
    {
        public BoolJProperty(JProperty prop) : base("bool", prop.Value)
        {
        }
    }
}