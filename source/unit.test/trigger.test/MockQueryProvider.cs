using System;
using System.Linq.Expressions;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;

namespace trigger.test
{
    public class MockQueryProvider : QueryProvider
    {
        public override string GetQueryText(Expression expression)
        {
            throw new NotImplementedException();
        }

        public override object Execute(Expression expression)
        {
            Console.WriteLine(expression.ToString());
            return new EntityDefinition { Name = "Customer", EntityDefinitionId = 10 };
        }
    }
}