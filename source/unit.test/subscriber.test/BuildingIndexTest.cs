using Bespoke.Sph.Messaging;
using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace subscriber.test
{
    [TestFixture]
    public class BuildingIndexTest
    {

        [Test]
        public void Index()
        {
            var sub = new BuildingSubscriberSub();
            sub.Execute("test", new Building { Name = "Damansara Intan", BuildingId = 100,LotNo = "2305"});
        }
        public class BuildingSubscriberSub : BuildingIndexerSubscriber
        {
            public void Execute(string operation, Building item)
            {
               // base.ProcessMessage(item).Wait(5000);
            }
        }

    }

}
