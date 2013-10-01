using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using NUnit.Framework;

namespace domain.test.triggers
{
    [TestFixture]
    class ActionTest
    {

        [Test]
        public void Email()
        {
            var building = new Building();
            ObjectBuilder.AddCacheList<ITemplateEngine>(new MockTemplateEnging());
            CustomAction email = new EmailAction
            {
                To = "ruzzaima@bespoke.com.my",
                SubjectTemplate = "test @Model.Name",
                From = "admin@bespoke.com.my",
                BodyTemplate = "What ever"
            };
            if (email.UseAsync)
                email.ExecuteAsync(new RuleContext(building)).Wait(5000);
            else
                email.Execute(new RuleContext(building));


        }
        /**/
        [Test]
        public void Setter()
        {
            var persistence = new MockPersistence();
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            ObjectBuilder.AddCacheList<IPersistence>(persistence);
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new MockChangePublisher());

            var building = new Building { UnitNo = "4444", Note = "Test 1234", Type = "Shopping Mall"};
            var setter = new SetterAction
            {
                TriggerId = 44,
                Title = "Unit test runner"
            };
            setter.SetterActionChildCollection.Add(new SetterActionChild { Path = "Name", Field = new ConstantField { Type = typeof(string), Value = "Damansara Intan" } }); ;
            setter.SetterActionChildCollection.Add(new SetterActionChild { Path = "Note", Field = new DocumentField { Path = "Type", Type = typeof(string) } });
            setter.ExecuteAsync(new RuleContext(building)).Wait(5000);

            Assert.AreEqual("Damansara Intan", persistence.Building.Name);
            Assert.AreEqual("Shopping Mall", persistence.Building.Note);

        }
    }

    internal class MockLdap : IDirectoryService
    {
        public string CurrentUserName { get { return "test"; } }
    }

    internal class MockTemplateEnging : ITemplateEngine
    {
        public Task<string> GenerateAsync(string template, dynamic model)
        {
            return Task.FromResult(template);
        }
    }
}
