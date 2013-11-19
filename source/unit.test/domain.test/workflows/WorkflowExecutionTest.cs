using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using Bespoke.Sph.Templating;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class WorkflowExecutionTest
    {
        private Mock<IBinaryStore> m_store;

        [SetUp]
        public void Init()
        {
            var doc = new BinaryStore
            {
                Content = File.ReadAllBytes(@".\workflows\PemohonWakaf.xsd")
            };
            m_store = new Mock<IBinaryStore>(MockBehavior.Strict);
            m_store.Setup(x => x.GetContent("schema-storeid"))
                .Returns(doc);
            ObjectBuilder.AddCacheList(m_store.Object);
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
            ecp.Setup(x => x.PublishAdded(It.IsAny<string>(), It.IsAny<IEnumerable<Entity>>()))
                .Returns(() => Task.Delay(100));
            ecp.Setup(x => x.PublishChanges(It.IsAny<string>(), It.IsAny<IEnumerable<Entity>>(), It.IsAny<IEnumerable<AuditTrail>>()))
                .Returns(() => Task.Delay(100));
            ecp.Setup(x => x.PublishDeleted(It.IsAny<string>(), It.IsAny<IEnumerable<Entity>>()))
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


        [Test]
        public void BuildValidation()
        {
            var wd = new WorkflowDefinition { Name = "3 Is Three" };
            var screen = new ScreenActivity { Name = "Pohon", IsInitiator = true };
            screen.FormDesign.FormElementCollection.Add(new TextBox { Label = "Nama", Path = string.Empty });
            wd.ActivityCollection.Add(screen);


            var result = wd.ValidateBuild();
            Assert.IsFalse(result.Result);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.AreEqual("Name not valid identifier", result.Errors[0]);
            Assert.AreEqual("TextBox \"Nama\" does not have valid path", result.Errors[2]);

        }

        [Test]
        public void InitiateAsyncMessage()
        {
            var wf = new Workflow { WorkflowDefinitionId = 10, Version = 25, WebId = "A", WorkflowId = 35 };
            var screen = new ScreenActivity
            {
                Name = "Approve User",
                WebId = "B",
                NextActivityWebId = "C",
                Performer = new Performer
                {
                    UserProperty = "Username",
                    Value = "admin"
                }
            };
            screen.InitiateAsync(wf).ContinueWith(_ =>
            {
                var exc = _.Exception;
                if (null != exc)
                {
                    foreach (var e in exc.InnerExceptions)
                    {
                        Console.WriteLine(e);
                    }
                }
                Console.WriteLine(exc);
            }).Wait();
        }


        private WorkflowDefinition Create()
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", WorkflowDefinitionId = 8, SchemaStoreId = "schema-storeid" };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "email", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });

            return wd;
        }

        private WorkflowCompilerResult Compile(WorkflowDefinition wd)
        {
            m_store.Setup(x => x.GetContent("wd-storeid"))
               .Returns(new BinaryStore { Content = Encoding.Unicode.GetBytes(wd.ToXmlString()), StoreId = "wd-storeid" });

            wd.Version = Directory.GetFiles(".", "workflows.8.*.dll").Length + 1;
            var options = new CompilerOptions
            {
                IsDebug = true,
                SourceCodeDirectory = @"d:\temp\",
            };
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\web.sph.dll")));


            var result = wd.Compile(options);
            result.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(result.Result);

            Assert.IsTrue(File.Exists(result.Output), "assembly " + result);

            return result;
        }

        private void Run(WorkflowDefinition wd, string dll, Action<Task<ActivityExecutionResult>> continuationAction)
        {
            // try to instantiate the Workflow
            var assembly = Assembly.LoadFrom(dll);
            var wfTypeName = string.Format("Bespoke.Sph.Workflows_{0}_{1}.{2}", wd.WorkflowDefinitionId, wd.Version,
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
            wf.StartAsync().ContinueWith(_ =>
            {
                var result0 = _.Result;
                Console.WriteLine(result0.Status);

                wf.ExecuteAsync().ContinueWith(continuationAction).Wait();
            }).Wait();
        }


        [Test]
        public void Delay()
        {
            var wd = this.Create();
            wd.ActivityCollection.Add(new DelayActivity { Name = "Wait a second", Seconds = 1, WebId = "_WA_", NextActivityWebId = "_D_" });
            wd.ActivityCollection.Add(new EndActivity { WebId = "_C_", Name = "Habis" });
            var result = this.Compile(wd);
            this.Run(wd, result.Output, Console.WriteLine);
        }

        [Test]
        public void CompileAndRun()
        {
            var wd = this.Create();

            var pohon = new ScreenActivity
            {
                Title = "Pohon",//[A-Z|a-z|.]
                Name = "Pohon",
                ViewVirtualPath = "~/Views/Workflows_8_1/pohon.cshtml",
                WebId = "_A_",
                IsInitiator = true,
                NextActivityWebId = "_B_"
            };
            pohon.FormDesign.FormElementCollection.Add(new TextBox { Path = "Nama", Label = "Test" });
            pohon.FormDesign.FormElementCollection.Add(new TextBox { Path = "Title", Label = "Tajuk" });
            wd.ActivityCollection.Add(pohon);

            var decide = new DecisionActivity
            {
                Name = "Check applicant age group",
                WebId = "_B_",
                NextActivityWebId = "_C_"

            };
            decide.DecisionBranchCollection.Add(new DecisionBranch
            {
                Name = "Less than 25",
                Expression = "item.pemohon.Age < 25",
                NextActivityWebId = "_C_Below25"
            });
            decide.DecisionBranchCollection.Add(new DecisionBranch
            {
                Name = "25 to 50",
                Expression = "item.pemohon.Age >= 25 && item.pemohon.Age < 50",
                NextActivityWebId = "CREATE_BUILDING"
            });
            decide.DecisionBranchCollection.Add(new DecisionBranch
            {
                Name = "all else",
                Expression = "item.pemohon.Age >= 50",
                NextActivityWebId = "_EMAIL_",
                IsDefault = true
            });

            wd.ActivityCollection.Add(decide);

            var email = new NotificationActivity
            {
                From = "erymuzuan@gmail.com",
                To = "=item.email",
                Subject = "Ada permohonan baru @Model.Title",
                Body = "Permohonan baru di @Model.Title oleh @Model.pemohon.MyKad",
                WebId = "_EMAIL_",
                NextActivityWebId = "_C_",
                UserName = "admin"

            };
            wd.ActivityCollection.Add(email);


            var approval = new ScreenActivity
            {
                Title = "Kelulusan",
                WebId = "_C_",
                Name = "Kelulusan",
                NextActivityWebId = "_WA_",
                ViewVirtualPath = "d"
            };
            wd.ActivityCollection.Add(approval);


            wd.ActivityCollection.Add(new EndActivity { WebId = "_D_" });

            var land = new CreateEntityActivity { Name = "Create Building", EntityType = "Building", NextActivityWebId = "_D_", WebId = "CREATE_BUILDING" };
            land.PropertyMappingCollection.Add(new SimpleMapping { Source = "Title", Destination = "Name" });
            wd.ActivityCollection.Add(land);



            var result = this.Compile(wd);
            this.Run(wd, result.Output, r2 =>
            {
                var result2 = r2.Result;
                Console.WriteLine(result2);
                Assert.AreEqual("CREATE_BUILDING", result2.NextActivity);
            });


        }
    }
}
