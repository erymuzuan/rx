using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Microsoft.OData.UriParser;

namespace odata.queryparser
{
    public static class OdataQuerySortsBuilder
    {
        public static IEnumerable<Sort> TryParseClause(OrderByClause node)
        {
            var sorts = new List<Sort>();

            while (true)
            {
                var expression = (SingleValuePropertyAccessNode) node.Expression;
                sorts.Add(new Sort
                {
                    Path = expression.Property.Name,
                    Direction = node.Direction == OrderByDirection.Ascending ? SortDirection.Asc : SortDirection.Desc
                });
                if (null != node.ThenBy)
                {
                    node = node.ThenBy;
                    continue;
                }
                break;
            }

            return sorts;
        }
    }
}