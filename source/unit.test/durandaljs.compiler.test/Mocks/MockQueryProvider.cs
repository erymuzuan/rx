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
            if (exp.Arguments.Count == 2)
            {
                var operand = exp.Arguments[1].Operand;
                if (null == operand) return expression.Type.ToString();
                var body = operand.Body;
                if (null == body) return expression.Type.ToString();
                var predicate = string.Format("{0}.{1} {2} {3}.{4}",
                    body.Left.Expression.Name,
                    body.Left.Member.Name,
                    body.NodeType,
                    body.Right.Expression.Member.Name,
                    body.Right.Member.Name
                    );
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