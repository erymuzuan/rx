using System.Linq.Expressions;
using Bespoke.Sph.Domain.QueryProviders;

namespace domain.test
{
    internal class MockQueryProvider : QueryProvider
    {
        public override string GetQueryText(Expression expression)
        {
            throw new System.NotImplementedException();
        }

        public override object Execute(Expression expression)
        {
            throw new System.NotImplementedException();
        }
    }
}