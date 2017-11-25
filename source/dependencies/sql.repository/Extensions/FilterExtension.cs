using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class FilterExtension
    {
        public static SqlPredicate ToSqlPredicate(this Filter filter)
        {
            return new SqlPredicate(filter);
        }

        public static SqlPredicate ToSqlPredicate(this IEnumerable<Filter> filters)
        {
            return new SqlPredicate(filters);
        }
    }
}