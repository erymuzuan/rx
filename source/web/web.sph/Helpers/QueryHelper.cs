using System;
using System.Linq;
using System.Linq.Expressions;

namespace Bespoke.Sph.Web.Helpers
{
    public static class QueryHelper
    {

        public static IQueryable<T> WhereIf<T, TC>(this IQueryable<T> list,Expression<Func<T, bool>> predicate, TC? test) where TC : struct
        {
            if (test.HasValue)
            {
                return list.Where(predicate);
            }
            return list;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> list,Expression<Func<T, bool>> predicate, bool test) 
        {
            if (test)
            {
                return list.Where(predicate);
            }
            return list;
        }
    }

    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}