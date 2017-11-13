using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public class FiltersVisitor : JObjectVisitor<IList<Filter>>
    {
        protected override IList<Filter> Visit(JObject p)
        {
            throw new System.NotImplementedException();
        }
    }
}