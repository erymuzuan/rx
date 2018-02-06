using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    [Export("QueryParser", typeof(IQueryParser))]
    public class QueryParser : IQueryParser
    {
        public QueryDsl Parse(string text, string entity)
        {
            var json = JObject.Parse(text);
            var query = new QueryDsl();
            var queryToken = json.SelectToken("$.query");
            if (null != queryToken)
            {
                var filters = new FiltersVisitor().DynamicVisit(queryToken).ToArray();
                query.Filters.AddRange(filters);
            }
            var aggregateToken = json.SelectToken("$.aggs");
            if (null != aggregateToken)
            {
                var aggs = new AggregatesVisitor().DynamicVisit(aggregateToken).ToArray();
                query.Aggregates.AddRange(aggs);
            }
           
            query.Skip = json.GetTokenValue<int>("$.from");
            query.Size = json.GetTokenValue<int>("$.size");


            var sortToken = json.SelectToken("$.sort");
            if (null != sortToken)
            {
                var sorts = from jt in sortToken
                            let st = jt.First as JProperty
                            where null != st
                            let order = st.First as JObject
                            where null != order
                            let direction = order.First as JProperty
                            where null != direction
                            /* The order defaults to desc when sorting on the _score, and defaults to asc when sorting on anything else.*/
                            let dic = direction.Value.ToString() == "desc" ? SortDirection.Desc : SortDirection.Asc
                            select new Sort { Path = st.Name, Direction = dic };

                query.Sorts.AddRange(sorts);

            }
            var sourcesToken = json.SelectToken("$._source");
            var fieldsToken = json.SelectToken("$.fields");
            if (null != sourcesToken)
            {
                var sources = sourcesToken.Select(jt => jt.Value<string>());
                query.Fields.AddRange(sources);
            }
            if (null != fieldsToken)
            {
                var sources = fieldsToken.Select(jt => jt.Value<string>());
                query.Fields.AddRange(sources);
            }
            return query;
        }

        public string Provider => "elasticsearch";
        public string ContentType => "application/json+elasticsearch";
    }
}