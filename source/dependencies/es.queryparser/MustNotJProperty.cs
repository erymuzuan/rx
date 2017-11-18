using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public class MustNotJProperty : JProperty
    {
        public MustNotJProperty(JProperty prop) : base("must_not", prop.Value)
        {
        }
    }
}