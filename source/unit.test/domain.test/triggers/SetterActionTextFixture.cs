using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using Xunit;

namespace domain.test.triggers
{

    public class SetterActionTextFixture
    {
        [Fact]
        public async Task Setter()
        {
            var persistence = new MockPersistence();
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            ObjectBuilder.AddCacheList<IPersistence>(persistence);
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new MockChangePublisher());

            var customer = this.GetCustomerInstance();
            customer.FullName = "Wan Fatimah";
            customer.Gender = "Female";
            customer.CreatedBy = "erymuzuan";

            var setter = new SetterAction
            {
                TriggerId = "44",
                Title = "Unit test runner"
            };
            setter.SetterActionChildCollection.Add(new SetterActionChild { Path = "FullName", Field = new ConstantField { Type = typeof(string), Value = "Wan Fatimah Wan Husain" } }); ;
            setter.SetterActionChildCollection.Add(new SetterActionChild { Path = "PrimaryContact", Field = new DocumentField { Path = "CreatedBy", Type = typeof(string) } });
            await setter.ExecuteAsync(new RuleContext(customer));

            dynamic item = persistence.ChangedItems.First();
            Assert.Equal("Wan Fatimah Wan Husain", item.FullName);
            Assert.Equal("erymuzuan", item.PrimaryContact);

        }
    }
}