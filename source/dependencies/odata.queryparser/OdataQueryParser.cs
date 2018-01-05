using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Microsoft.OData.UriParser;

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

            var filters = TryParseFilters(parser);
            var sorts = TryParseSorts(parser);
            var parsedSkip = parser.ParseSkip() ?? 0;
            var skip = Convert.ToInt32(parsedSkip);
            var parsedTop = parser.ParseTop() ?? 0;
            var size = Convert.ToInt32(parsedTop);

            var query = new QueryDsl(filters, sorts, skip, size);

            return query;
        }

        public string Provider => "Odata";
        public string ContentType => "application/odata";

        private static Filter[] TryParseFilters(ODataUriParser parser)
        {
            var filterClause = parser.ParseFilter();
            if (null == filterClause) return null;
            var visitor = new ODataQueryNodeVisitor();
            filterClause.Expression.Accept(visitor);

            return visitor.GetFilters().ToArray();
        }

        private static Sort[] TryParseSorts(ODataUriParser parser)
        {
            var orderByClause = parser.ParseOrderBy();
            if (null == orderByClause) return null;
            var sorts = new List<Sort>();
            OdataQuerySortsBuilder.TryNodeValue(orderByClause, sorts);

            return sorts.ToArray();
        }
    }
}