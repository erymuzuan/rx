using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public static class OperatorExtension
    {
        public static Operator Invert(this Operator op)
        {

            switch (op)
            {
                case Operator.Eq:
                    return Operator.Neq;
                case Operator.Lt:
                    return Operator.Ge;
                case Operator.Le:
                    return Operator.Gt;
                case Operator.Gt:
                    return Operator.Le;
                case Operator.Ge:
                    return Operator.Lt;
                case Operator.Substringof:
                    return Operator.NotContains;
                case Operator.StartsWith:
                    return Operator.NotStartsWith;
                case Operator.EndsWith:
                    return Operator.NotEndsWith;
                case Operator.NotContains:
                    return Operator.Substringof;
                case Operator.Neq:
                    return Operator.Eq;
                case Operator.NotStartsWith:
                    return Operator.StartsWith;
                case Operator.NotEndsWith:
                    return Operator.EndsWith;
                case Operator.IsNull:
                    return Operator.IsNotNull;
                case Operator.IsNotNull:
                    return Operator.IsNull;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}