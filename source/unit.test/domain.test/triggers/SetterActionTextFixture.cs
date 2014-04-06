using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using NUnit.Framework;

namespace domain.test.triggers
{
    class SetterActionTextFixture
    {
        [Test]
        public void Setter()
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
                TriggerId = 44,
                Title = "Unit test runner"
            };
            setter.SetterActionChildCollection.Add(new SetterActionChild { Path = "FullName", Field = new ConstantField { Type = typeof(string), Value = "Wan Fatimah Wan Husain" } }); ;
            setter.SetterActionChildCollection.Add(new SetterActionChild { Path = "PrimaryContact", Field = new DocumentField { Path = "CreatedBy", Type = typeof(string) } });
            setter.ExecuteAsync(new RuleContext(customer)).Wait(5000);

            dynamic item = persistence.ChangedItems.First();
            Assert.AreEqual("Wan Fatimah Wan Husain", item.FullName);
            Assert.AreEqual("erymuzuan", item.PrimaryContact);

        }
    }
}