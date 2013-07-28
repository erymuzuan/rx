using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;
using Bespoke.SphCommercialSpaces.Domain.QueryProviders;
using NUnit.Framework;
using roslyn.scriptengine;

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
                email.ExecuteAsync(building).Wait(5000);
            else
                email.Execute(building);


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

            var building = new Building { LotNo = "4444" };
            var setter = new SetterAction
            {
                TriggerId = 44,
                Title = "Unit test runner"
            };
            setter.SetterActionChildCollection.Add(new SetterActionChild { Path = "Name", Field = new ConstantField { Type = typeof(string), Value = "Damansara Intan" } }); ;
            setter.SetterActionChildCollection.Add(new SetterActionChild { Path = "Note", Field = new DocumentField { Path = "LotNo", Type = typeof(string) } });
            setter.ExecuteAsync(building).Wait(5000);

            Assert.AreEqual("Damansara Intan", persistence.Building.Name);
            Assert.AreEqual("4444", persistence.Building.Note);

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
