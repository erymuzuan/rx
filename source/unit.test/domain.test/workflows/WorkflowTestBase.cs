using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using Bespoke.Sph.Templating;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class WorkflowTestBase
    {
        protected Mock<IBinaryStore> BinaryStore { get; private set; }


        [SetUp]
        public void Init()
        {
            var doc = new BinaryStore
            {
                Content = File.ReadAllBytes(@".\workflows\PemohonWakaf.xsd")
            };
            BinaryStore = new Mock<IBinaryStore>(MockBehavior.Strict);
            BinaryStore.Setup(x => x.GetContent("schema-storeid"))
                .Returns(doc);
            ObjectBuilder.AddCacheList(BinaryStore.Object);
            var qp = new Mock<QueryProvider>(MockBehavior.Loose);
            ObjectBuilder.AddCacheList(qp.Object);

            var ds = new Mock<IDirectoryService>(MockBehavior.Loose);
            ObjectBuilder.AddCacheList(ds.Object);

            var ps = new Mock<IPersistence>(MockBehavior.Strict);
            ps.Setup(
                x =>
                    x.SubmitChanges(It.IsAny<IEnumerable<Entity>>(), It.IsAny<IEnumerable<Entity>>(),
                        It.IsAny<PersistenceSession>()))
                .Returns(() => Task.FromResult(new SubmitOperation()));
            ObjectBuilder.AddCacheList(ps.Object);

            var ecp = new Mock<IEntityChangePublisher>(MockBehavior.Loose);
            ecp.Setup(x => x.PublishAdded(It.IsAny<string>(), It.IsAny<IEnumerable<Entity>>(), It.IsAny<Dictionary<string, object>>()))
                .Returns(() => Task.Delay(100));
            ecp.Setup(x => x.PublishChanges(It.IsAny<string>(), It.IsAny<IEnumerable<Entity>>(), It.IsAny<IEnumerable<AuditTrail>>(), It.IsAny<Dictionary<string, object>>()))
                .Returns(() => Task.Delay(100));
            ecp.Setup(x => x.PublishDeleted(It.IsAny<string>(), It.IsAny<IEnumerable<Entity>>(), It.IsAny<Dictionary<string, object>>()))
                .Returns(() => Task.Delay(100));
            ObjectBuilder.AddCacheList(ecp.Object);

            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());


            var usersRepos = new Mock<IRepository<UserProfile>>(MockBehavior.Strict);
            usersRepos.Setup(x => x.LoadOneAsync(It.IsAny<IQueryable<UserProfile>>()))
                .Returns(Task.FromResult(new UserProfile { Username = "admin", Email = "admin@bespoke.com.my" }));
            ObjectBuilder.AddCacheList(usersRepos.Object);
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());

            var email = new Mock<INotificationService>(MockBehavior.Strict);
            email.Setup(x => x.SendMessageAsync(It.IsAny<Message>(), It.IsAny<string>()))
                .Returns(Task.Delay(500))
                .Callback<Message, string>((m, e) => Console.WriteLine("sending email to {0} body is {2}-{1}", e, m.Body, m.Subject));
            ObjectBuilder.AddCacheList(email.Object);

        }

    }
}
