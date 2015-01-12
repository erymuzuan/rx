using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace trigger.test
{
    [TestClass]
    public class TriggerCompilerTestFixture
    {
        private EntityDefinition m_ed;
        [TestInitialize]
        public void Init()
        {
            m_ed = new EntityDefinition
            {
                Name = "Customer",
                Id = "customer"
            };
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(new MockEdRepos(m_ed));
        }

        [TestMethod]
        public async Task GenerateSource()
        {
            var trigger = new Trigger
            {
                Entity = "Customer",
                WebId = Guid.NewGuid().ToString(),
                FiredOnOperations = "",
                IsFiredOnChanged = true,
                Id = "customer-test-trigger",
                Name = "customer test trigger"

            };
            var messaging = new MessagingAction
            {
                AdapterType = typeof(MessagingAction),
                OutboundMapType = typeof(MessagingAction),
                Operation = "TestOperation",
                Title = "PostTest"
            };
            trigger.ActionCollection.Add(messaging);
            var code = await trigger.GenerateCodeAsync();
            Console.WriteLine(code);
        }
    }
}
