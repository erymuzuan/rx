using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class FilterExtension
    {
        public static ElasticsearchFilter ToElasticsearchFilter(this Filter filter)
        {
            return new ElasticsearchFilter(filter);
        }
        public static ElasticsearchFilter ToElasticsearchFilter(this Filter filter, JObject mapping)
        {
            return new ElasticsearchFilter(filter, mapping);
        }

        public static ElasticsearchFilter ToElasticsearchFilter(this IEnumerable<Filter> filters)
        {
            return new ElasticsearchFilter(filters);
        }
        public static ElasticsearchFilter ToElasticsearchFilter(this IEnumerable<Filter> filters, JObject mapping)
        {
            return new ElasticsearchFilter(filters, mapping);
        }
    }
}