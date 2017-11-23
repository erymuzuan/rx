using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.triggers
{

    [Trait("Category", "Trigger")]
    [Trait("Field", "Setter")]
    public class SetterActionTextFixture
    {
        public ITestOutputHelper Console { get; }

        public SetterActionTextFixture(ITestOutputHelper console)
        {
            Console = console;
        }


        [Fact]
        public async Task Setter()
        {
            var persistence = new MockPersistence(Console);
            var publisher = new MockChangePublisher(Console);
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            ObjectBuilder.AddCacheList<IPersistence>(persistence);
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(publisher);

            var customer =await this.GetCustomerInstanceAsync();
            customer.FirstName = "Wan Fatimah";
            customer.Gender = "Female";
            customer.CreatedBy = "erymuzuan";

            var setter = new SetterAction
            {
                TriggerId = "44",
                Title = "Unit test runner"
            };
            setter.SetterActionChildCollection.Add(new SetterActionChild { Path = "FirstName", Field = new ConstantField { Type = typeof(string), Value = "Wan Fatimah Wan Husain" } });
            setter.SetterActionChildCollection.Add(new SetterActionChild { Path = "PrimaryContact", Field = new DocumentField { Path = "CreatedBy", Type = typeof(string) } });
            await setter.ExecuteAsync(new RuleContext(customer));

            dynamic item = publisher.ChangedItems.First();
            Assert.Equal("Wan Fatimah Wan Husain", item.FirstName);
            Assert.Equal("erymuzuan", item.PrimaryContact);

        }
    }
}