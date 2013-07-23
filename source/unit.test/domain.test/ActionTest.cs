using Bespoke.SphCommercialSpaces.Domain;
using Bespoke.SphCommercialSpaces.Domain.QueryProviders;
using NUnit.Framework;
using roslyn.scriptengine;

namespace domain.test
{
    [TestFixture]
    class ActionTest
    {

        [Test]
        public void Email()
        {
            var building = new Building();
            CustomAction email = new EmailAction { To = "ruzzaima@bespoke.com.my", SubjectTemplate = "test @Model.Name" };
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
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            ObjectBuilder.AddCacheList<IPersistence>(persistence);
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new MockChangePublisher());

            var building = new Building{LotNo = "4444"};
            var setter = new SetterAction();
            setter.PathValueCollection.Add("Name", new ConstantField{ Type = typeof(string), Value = "Damansara Intan"});;
            setter.PathValueCollection.Add("Note", new DocumentField{ Path = "LotNo", Type = typeof(string)});
            setter.ExecuteAsync(building).Wait(5000);

            Assert.AreEqual("Damansara Intan", persistence.Building.Name);
            Assert.AreEqual("4444", persistence.Building.Note);

        }
    }
}
