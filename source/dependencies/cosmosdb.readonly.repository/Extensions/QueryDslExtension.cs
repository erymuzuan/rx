using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class QueryDslExtension
    {

        public static string CompileToCosmosDbSql(this Entity entity, QueryDsl query)
        {

            var sql = new StringBuilder();

            var hasFilters = query.Filters.Any();
            if (hasFilters)
            {
                sql.AppendLine("WHERE");
                if (query.Aggregates.Any())
                    sql.AppendLine(entity.CompileToElasticsearchBoolQuery(query.Filters.ToArray()));

                if (!query.Aggregates.Any())
                    sql.AppendLine(entity.CompileToElasticsearchBoolQuery(query.Filters.ToArray()));

                var fullText = entity.CompileToElasticsearchFullTextQuery(query.Filters.ToArray());
                if (!string.IsNullOrWhiteSpace(fullText))
                    sql.AppendLine(fullText);
            }


            if (query.Sorts.Any())
                sql.AppendLine("ORDER BY" + "[" + query.Sorts.ToString(",", x => x.GenerateQuery()) + "]");

            if (query.Fields.Any())
                sql.AppendLine("_source" + "[" + query.Fields.ToString(",", x => $@"""{x}""") + "]");

            if (query.Aggregates.Any())
                sql.AppendLine("aggs" + "{" + query.Aggregates.ToString(",", x => x.GenerateQuery()) + "}");

            sql.AppendLine("from" + query.Skip);
            sql.AppendLine("size" + query.Size);

            return sql.ToString();
        }
    }
}