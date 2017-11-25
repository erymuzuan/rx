using System.Collections.Generic;
using Bespoke.Sph.Domain;

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