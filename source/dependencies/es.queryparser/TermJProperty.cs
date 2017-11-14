using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public class TermJProperty : JProperty
    {
        public TermJProperty(JProperty prop) : base("term", prop.Value)
        {
        }
    }
}