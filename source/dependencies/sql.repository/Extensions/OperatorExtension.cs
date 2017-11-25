using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class OperatorExtension
    {
        public static string ToSqlOperator(this Operator @operator)
        {
            switch (@operator)
            {
                case Operator.Eq:
                    return "=";
                case Operator.Lt:
                    return  "<";
                case Operator.Le:
                    return "<"; 
                case Operator.Gt:
                    return ">";
                case Operator.Ge:
                    return ">=";
                case Operator.Substringof:
                    return "LIKE";
                case Operator.StartsWith:
                    return "LIKE";
                case Operator.EndsWith:
                    return "LIKE";
                case Operator.NotContains:
                    return "NOT LIKE";
                case Operator.Neq:
                    return "<>";
                case Operator.NotStartsWith:
                    return "NOT LIKE";
                case Operator.NotEndsWith:
                    return "NOT LIKE";
                case Operator.IsNull:
                    return "IS NULL";
                case Operator.IsNotNull:
                    return "IS NOT NULL";
                case Operator.FullText:
                    return "WTF";
                default:
                    throw new ArgumentOutOfRangeException(nameof(@operator), @operator, null);
            }
        }
    }
}