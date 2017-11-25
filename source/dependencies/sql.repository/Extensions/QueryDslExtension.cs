using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class QueryDslExtension
    {
        public static string CompileToSql(this QueryDsl query)
        {
            throw new NotImplementedException("Searching all the types within indices is not supported yet\r\n" + query);
        }

        public static string CompileToSqlCount<T>(this QueryDsl query) where T : DomainObject, new()
        {
            var elements = new Dictionary<string, object>();
            var entity = new T();

            var hasFilters = query.Filters.Any();
            if (hasFilters)
            {
                var arrayEsFilters = query.Filters.ToSqlPredicate();
                elements.Add("WHERE", arrayEsFilters.CompileToBoolQuery(entity));
            }


            return $@"SELECT COUNT([Id]) FROM [{ConfigurationManager.ApplicationName}].[{typeof(T).Name}]
    {elements.ToString("\r\n", x => $@"{x.Key} {x.Value}")}
";
        }


        public static string CompileToSql<T>(this QueryDsl query) where T : DomainObject, new()
        {
            var elements = new Dictionary<string, object>();
            var entity = new T();

            var hasFilters = query.Filters.Any();
            if (hasFilters)
            {
                var arrayEsFilters = query.Filters.ToSqlPredicate();
                elements.Add("WHERE", arrayEsFilters.CompileToBoolQuery(entity));
            }
            if (query.Sorts.Any())
                elements.Add("ORDER BY", query.Sorts.ToString(", ", x => x.GenerateQuery()));
            else
                elements.Add("ORDER BY", "[Id]");

            var fields = "[Id], [Json]";
            if (query.Fields.Any())
            {
                var projectsions = new List<string>(query.Fields);
                if (!projectsions.Contains("Id"))
                    projectsions.Insert(0, "Id");

                // TODO : only use JSON_VALUE for those fields without colums
                fields = projectsions.ToString(",", x => $@"JSON_VALUE([Json], '$.{x}') as '{x}'");
            }

            if (query.Aggregates.Any())
                elements.Add("GROUP BY", query.Aggregates.ToString(", ", x => x.GenerateQuery()));


            return $@"SELECT {fields} FROM [{ConfigurationManager.ApplicationName}].[{typeof(T).Name}]
    {elements.ToString("\r\n", x => $@"{x.Key} {x.Value}")}

OFFSET {query.Skip} ROWS
FETCH NEXT {query.Size} ROWS ONLY";
        }
    }
}