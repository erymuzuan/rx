using System.Linq.Expressions;
using Bespoke.Sph.Domain.QueryProviders;

namespace durandaljs.compiler.test
{
    internal class MockQueryProvider : QueryProvider
    {
        public override string GetQueryText(Expression expression)
        {
            return expression.Type.ToString();

        }

        public override object Execute(Expression expression)
        {
            throw new System.NotImplementedException();
        }
    }
}