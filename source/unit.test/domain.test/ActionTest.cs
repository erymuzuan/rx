using Bespoke.SphCommercialSpaces.Domain;
using Bespoke.SphCommercialSpaces.Domain.QueryProviders;
using NUnit.Framework;

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
            ObjectBuilder.AddCacheList<IPersistence>(persistence);
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new MockChangePublisher());

            var building = new Building();
            var setter = new SetterAction();
            setter.PathValueCollection.Add("//bs:Building/@Name", "Damansara Intan");
            setter.ExecuteAsync(building).Wait(5000);

            Assert.AreEqual("Damansara Intan", persistence.Building.Name);

        }
    }
}
