using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bespoke.SphCommercialSpaces.Domain.QueryProviders
{
    internal class SubqueryRemover : DbExpressionVisitor
    {
        HashSet<SelectExpression> m_selectsToRemove;
        Dictionary<string, Dictionary<string, Expression>> m_map;

        public Expression Remove(SelectExpression outerSelect, params SelectExpression[] selectsToRemove)
        {
            return Remove(outerSelect, (IEnumerable<SelectExpression>)selectsToRemove);
        }

        public Expression Remove(SelectExpression outerSelect, IEnumerable<SelectExpression> selectsToRemove)
        {
            var list = selectsToRemove.ToList();
            this.m_selectsToRemove = new HashSet<SelectExpression>(list);
            this.m_map = list.ToDictionary(d => d.Alias, d => d.Columns.ToDictionary(d2 => d2.Name, d2 => d2.Expression));
            return this.Visit(outerSelect);
        }

        protected override Expression VisitSelect(SelectExpression select)
        {
            if (this.m_selectsToRemove.Contains(select))
            {
                return this.Visit(select.From);
            }
            return base.VisitSelect(select);
        }

        protected override Expression VisitColumn(ColumnExpression column)
        {
            Dictionary<string, Expression> nameMap;
            if (this.m_map.TryGetValue(column.Alias, out nameMap))
            {
                Expression expr;
                if (nameMap.TryGetValue(column.Name, out expr))
                {
                    return this.Visit(expr);
                }
                throw new Exception("Reference to undefined column");
            }
            return column;
        }
    }
}