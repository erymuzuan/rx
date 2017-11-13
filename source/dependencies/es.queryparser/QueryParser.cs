﻿using System.Linq;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public class QueryParser : IQueryParser
    {
        public QueryDsl Parse(string text)
        {
            var json = JObject.Parse(text);
            var query = new QueryDsl();
            var queryToken = json.SelectToken("$.query");
            if (null != queryToken)
            {
                var filters = from st in queryToken.OfType<JProperty>()
                              let term = st.First as JObject
                              where null != term
                              let field = term.First as JProperty
                              where null != field
                              select new Filter(field.Name, Operator.Eq, field.Value);

                query.Filters.AddRange(filters);
            }

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
            return query;
        }

        public string Provider => "elasticsearch";
        public string Version => "1.7";
    }
}