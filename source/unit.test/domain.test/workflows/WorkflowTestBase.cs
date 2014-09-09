using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using Bespoke.Sph.Templating;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace domain.test.workflows
{
    public class WorkflowTestBase
    {
        protected Mock<IBinaryStore> BinaryStore { get; private set; }

        

        [SetUp]
        public virtual void Init()
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


            var edRepository = new Mock<IRepository<EntityDefinition>>(MockBehavior.Strict);
            edRepository.Setup(x => x.LoadOneAsync(It.IsAny<IQueryable<EntityDefinition>>()))
                .Returns(Task.FromResult(new EntityDefinition { Name= "Building", Plural = "Buildings", EntityDefinitionId= 10 }));
            edRepository.Setup(x => x.LoadOne(It.IsAny<IQueryable<EntityDefinition>>()))
                .Returns(new EntityDefinition { Name= "Building", Plural = "Buildings", EntityDefinitionId= 10 });
            ObjectBuilder.AddCacheList(edRepository.Object);

            var usersRepos = new Mock<IRepository<UserProfile>>(MockBehavior.Strict);
            usersRepos.Setup(x => x.LoadOneAsync(It.IsAny<IQueryable<UserProfile>>()))
                .Returns(Task.FromResult(new UserProfile { UserName = "admin", Email = "admin@bespoke.com.my" }));
            ObjectBuilder.AddCacheList(usersRepos.Object);
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());

            var email = new Mock<INotificationService>(MockBehavior.Strict);
            email.Setup(x => x.SendMessageAsync(It.IsAny<Message>(), It.IsAny<string>()))
                .Returns(Task.Delay(500))
                .Callback<Message, string>((m, e) => Console.WriteLine("sending email to {0} body is {2}-{1}", e, m.Body, m.Subject));
            ObjectBuilder.AddCacheList(email.Object);

        }



        protected WorkflowDefinition Create(string id = "8")
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", Id = id, SchemaStoreId = "schema-storeid" };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "email", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });

            return wd;
        }

        protected WorkflowCompilerResult Compile(WorkflowDefinition wd, bool verbose = false, bool
            assertError = true)
        {
            this.BinaryStore.Setup(x => x.GetContent("wd-storeid"))
               .Returns(new BinaryStore { Content = Encoding.Unicode.GetBytes(wd.ToXmlString()), StoreId = "wd-storeid" });

            wd.Version = 25;
            var options = new CompilerOptions
            {
                IsDebug = true,
                SourceCodeDirectory = @"c:\temp\sph",
                IsVerbose = verbose
            };
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll"));


            var result = wd.Compile(options);
            result.Errors.ForEach(Console.WriteLine);
            if (assertError)
                Assert.IsTrue(result.Result, result.ToJsonString(Formatting.Indented));

            return result;
        }

        protected Workflow CreateInstance(WorkflowDefinition wd, string dll)
        {
            // try to instantiate the Workflow
            var assembly = Assembly.LoadFrom(dll);
            var wfTypeName = string.Format("Bespoke.Sph.Workflows_{0}_{1}.{2}", wd.Id, wd.Version,
                wd.WorkflowTypeName);

            var wfType = assembly.GetType(wfTypeName);
            Assert.IsNotNull(wfType, wfTypeName + " is null");

            var wf = Activator.CreateInstance(wfType) as Workflow;
            Assert.IsNotNull(wf);

            wf.SerializedDefinitionStoreId = "wd-storeid";
            XmlSerializerService.RegisterKnownTypes(typeof(Workflow), wfType);

            var pemohonProperty = wf.GetType().GetProperty("pemohon");
            Assert.IsNotNull(pemohonProperty);
            dynamic pemohon = pemohonProperty.GetValue(wf);
            pemohon.Age = 28;

            wf.WorkflowDefinition = wd;

            return wf;
        }




    }
}
