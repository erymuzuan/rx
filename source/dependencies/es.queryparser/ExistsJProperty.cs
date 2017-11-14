using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public class ExistsJProperty : JProperty
    {
        public ExistsJProperty(JProperty prop) : base("exists", prop.Value)
        {
        }
    }
}