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
            var building = new Designation();
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
            Assert.Fail();
            var persistence = new MockPersistence();
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            ObjectBuilder.AddCacheList<IPersistence>(persistence);
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new MockChangePublisher());

            var building = new Designation { Name = "4444", Title = "Test 1234", CreatedBy = "Shopping Mall"};
            var setter = new SetterAction
            {
                TriggerId = 44,
                Title = "Unit test runner"
            };
            setter.SetterActionChildCollection.Add(new SetterActionChild { Path = "Name", Field = new ConstantField { Type = typeof(string), Value = "Damansara Intan" } }); ;
            setter.SetterActionChildCollection.Add(new SetterActionChild { Path = "Note", Field = new DocumentField { Path = "Type", Type = typeof(string) } });
            setter.ExecuteAsync(new RuleContext(building)).Wait(5000);

            //Assert.AreEqual("Damansara Intan", persistence.Designation.Name);
            //Assert.AreEqual("Shopping Mall", persistence.Designation.Note);

        }
    }

    internal class MockLdap : IDirectoryService
    {
        public string CurrentUserName { get { return "test"; } }
        public Task<string[]> GetUserInRolesAsync(string role)
        {
            throw new System.NotImplementedException();
        }

        public Task<string[]> GetUserRolesAsync(string userName)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AuthenticateAsync(string userName, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserProfile> GetUserAsync(string userName)
        {
            throw new System.NotImplementedException();
        }
    }

    internal class MockTemplateEnging : ITemplateEngine
    {
        public Task<string> GenerateAsync(string template, dynamic model)
        {
            return Task.FromResult(template);
        }
    }
}
