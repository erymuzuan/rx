using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public class MustJProperty : JProperty
    {
        public MustJProperty(JProperty prop) : base("must", prop.Value)
        {
        }
    }
}