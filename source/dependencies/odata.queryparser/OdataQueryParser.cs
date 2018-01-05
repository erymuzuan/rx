using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Microsoft.OData.UriParser;
using Microsoft.OData.UriParser.Aggregation;

namespace odata.queryparser
{
    [Export("QueryParser", typeof(IQueryParser))]
    public class OdataQueryParser : IQueryParser
    {
        public QueryDsl Parse(string text, string entity)
        {
            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var ed = repos.LoadOneAsync<EntityDefinition>(x => x.Name == entity).Result;
            var model = ed.GenerateEdmModel();
            var requestUri = new Uri($"/{ed.Plural}?{text}", UriKind.Relative);
            var parser = new ODataUriParser(model, requestUri);

            var filters = ParseFilters(parser);
            var sorts = ParseSorts(parser);
            var parsedSkip = parser.ParseSkip() ?? 0;
            var skip = Convert.ToInt32(parsedSkip);
            var parsedTop = parser.ParseTop() ?? 0;
            var size = Convert.ToInt32(parsedTop);
            var aggregates = ParseAggregates(parser) ?? new List<Aggregate>();

            var query = new QueryDsl(filters, sorts, skip, size);
            query.Aggregates.AddRange(aggregates);

            return query;
        }

        public string Provider => "Odata";
        public string ContentType => "application/odata";

        private static Filter[] ParseFilters(ODataUriParser parser)
        {
            var filterClause = parser.ParseFilter();
            if (null == filterClause) return null;
            var visitor = new OdataQueryNodeVisitor();
            filterClause.Expression.Accept(visitor);

            return visitor.GetFilters().ToArray();
        }

        private static Sort[] ParseSorts(ODataUriParser parser)
        {
            var orderByClause = parser.ParseOrderBy();
            if (null == orderByClause) return null;
            var sorts = OdataQuerySortsBuilder.TryParseClause(orderByClause);

            return sorts.ToArray();
        }

        private static IEnumerable<Aggregate> ParseAggregates(ODataUriParser parser)
        {
            var applyClause = parser.ParseApply();
            if (null == applyClause) return null;
            var transformationNodes = applyClause.Transformations
                .Where(x => x.Kind == TransformationNodeKind.Aggregate);
            var aggregates = OdataQueryAggregatesBuilder.TryParseNodes(transformationNodes);

            return aggregates.ToList();
        }
    }
}