using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class QueryDslExtension
    {
        
        public static string CompileToElasticsearchQueryDsl(this Entity entity, QueryDsl query)
        {

            var elements = new Dictionary<string, object>();
            if (query.Filters.Any())
                elements.Add("filter", entity.CompileToElasticsearchBoolQuery(query.Filters.ToArray()));
            if (query.Sorts.Any())
                elements.Add("sort", "[" + query.Sorts.ToString(",", x => x.GenerateQuery()) + "]");

            if (query.Fields.Any())
                elements.Add("_source", "[" + query.Fields.ToString(",", x => $@"""{x}""") + "]");

            elements.Add("from", query.Skip);
            elements.Add("size", query.Size);

            return $@"{{
    {elements.ToString(",\r\n", x => $@"""{x.Key}"":{x.Value}")}

}}";
        }
    }
}