using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class QueryDslExtension
    {
        public static string CompileToElasticsearchQuery(this QueryDsl query)
        {
            throw new NotImplementedException("Searching all the types within indices is not supported yet\r\n" + query);
        }

        public static string CompileToElasticsearchQuery<T>(this QueryDsl query, JObject mapping) where T : DomainObject, new()
        {
            var elements = new Dictionary<string, object>();
            var entity = new T();

            var hasFilters = query.Filters.Any();
            if (hasFilters)
            {
                var arrayEsFilters = query.Filters.ToElasticsearchFilter(mapping);
                if (query.Aggregates.Any())
                    elements.Add("query", arrayEsFilters.CompileToBoolQuery(entity));

                if (!query.Aggregates.Any())
                    elements.Add("filter", arrayEsFilters.CompileToBoolQuery(entity));

                var fullText = arrayEsFilters.CompileToFullTextQuery(entity);
                if (!string.IsNullOrWhiteSpace(fullText))
                    elements.AddOrReplace("query", fullText);
            }


            if (query.Sorts.Any())
                elements.Add("sort", "[" + query.Sorts.ToString(",", x => x.GenerateQuery()) + "]");

            if (query.Fields.Any())
                elements.Add("_source", "[" + query.Fields.ToString(",", x => $@"""{x}""") + "]");

            if (query.Aggregates.Any())
                elements.Add("aggs", "{" + query.Aggregates.ToString(",", x => x.GenerateQuery()) + "}");

            elements.Add("from", query.Skip);
            elements.Add("size", query.Size);

            return $@"{{
    {elements.ToString(",\r\n", x => $@"""{x.Key}"":{x.Value}")}

}}";
        }
    }
}