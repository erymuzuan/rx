using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Microsoft.OData.UriParser;
using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace odata.queryparser
{
    [Export("QueryParser", typeof(IQueryParser))]
    public class OdataQueryParser : IQueryParser
    {
        public QueryDsl Parse(string text, string entity)
        {
            var qs = text.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var ed = repos.LoadOneAsync<EntityDefinition>(x => x.Name == entity).Result;

            var model = ed.GenerateEdmModel();
            var requestUri = new Uri($"/{ed.Plural}?{text}", UriKind.Relative);

            var parser = new ODataUriParser(model, requestUri);

            string GetQueryStringValue(string key)
            {
                var pair = qs.SingleOrDefault(x => x.StartsWith(key))
                    .ToEmptyString().Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                if (pair.Length <= 1)
                    return string.Empty;

                return pair.LastOrDefault().ToEmptyString();
            }

            var filters = ParseFilters(parser);
            var sorts = ParseSorts(GetQueryStringValue("$orderby"));
            var parsedSkip = parser.ParseSkip() ?? 0;
            var skip = Convert.ToInt32(parsedSkip);
            var parsedTop = parser.ParseTop();
            var size = Convert.ToInt32(parsedTop);

            var query = new QueryDsl(filters, sorts, skip, size);

            return query;
        }

        public string Provider => "Odata";
        public string ContentType => "application/odata";

        private static Filter[] ParseFilters(ODataUriParser parser)
        {
            var parsedFilter = parser.ParseFilter();
            var visitor = new ODataQueryNodeVisitor();
            parsedFilter.Expression.Accept(visitor);

            return visitor.GetFilters().ToArray();
        }

        //TODO : when the Field is a function call .e.g "$filter=year(Dob) eq 1950"

        private static Sort[] ParseSorts(string odata)
        {
            var queries = odata.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var sorts = from q in queries
                        let words = q.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                        let path = words[0].Replace("/", ".")
                        let direction = (words.Length == 2 && words[1] == "desc") ? SortDirection.Desc : SortDirection.Asc
                        select new Sort { Path = path, Direction = direction };

            return sorts.ToArray();
        }
    }
}