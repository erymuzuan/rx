using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;

namespace durandaljs.compiler.test
{
    internal class MockQueryProvider : QueryProvider
    {
        public override string GetQueryText(Expression expression)
        {
            dynamic exp = expression;
            var predicate = expression.Type.ToString();
            if (exp.Arguments.Count == 2)
            {
                var operand = exp.Arguments[1].Operand;
                if (null == operand) return expression.Type.ToString();
                var body = operand.Body;
                if (null == body) return expression.Type.ToString();
                var ce = body.Right as ConstantExpression;

                var operatorExpression = "";
                switch ((ExpressionType)body.NodeType)
                {
                    case ExpressionType.Equal:
                        operatorExpression = "==";
                        break;
                    case ExpressionType.NotEqual:
                        operatorExpression = "!=";
                        break;
                    case ExpressionType.LessThanOrEqual:
                        operatorExpression = "<=";
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        operatorExpression = ">=";
                        break;
                    case ExpressionType.LessThan:
                        operatorExpression = "<";
                        break;
                    case ExpressionType.GreaterThan:
                        operatorExpression = ">";
                        break;
                    case ExpressionType.IsFalse:
                        operatorExpression = "false";
                        break;

                }

                if (null != ce)
                {
                    predicate = string.Format("{0}.{1} {2} {3}",
                       body.Left.Expression.Name,
                       body.Left.Member.Name,
                       operatorExpression,
                       ce.Value
                       );
                }

                var me = body.Right as MemberExpression;
                if (null != me)
                {
                    var rce = body.Right.Expression as ConstantExpression;
                    if (null != rce)
                    {
                        predicate = string.Format("{0}.{1} {2} {3}",
                           body.Left.Expression.Name,
                           body.Left.Member.Name,
                           operatorExpression,
                           me
                           );

                    }
                    var rme = body.Right.Expression as MemberExpression;
                    if (null != rme)
                    {

                        predicate = string.Format("{0}.{1} {2} {3}.{4}",
                           body.Left.Expression.Name,
                           body.Left.Member.Name,
                           operatorExpression,
                           body.Right.Expression.Member.Name,
                           body.Right.Member.Name
                           );
                    }

                }
                if (DebuggerHelper.IsVerbose)
                    Console.WriteLine(predicate);
                return predicate;

            }
            return expression.Type.ToString();

        }

        public override object Execute(Expression expression)
        {
            throw new System.NotImplementedException();
        }
    }
}