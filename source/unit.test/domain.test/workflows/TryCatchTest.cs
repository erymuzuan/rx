using System;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using domain.test.reports;
using domain.test.triggers;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class TryCatchTest
    {
        private readonly string m_schemaStoreId = Guid.NewGuid().ToString();
        [SetUp]
        public void Init()
        {
            var doc = new BinaryStore
            {
                Content = File.ReadAllBytes(@".\workflows\PemohonWakaf.xsd")
            };
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);
            store.Setup(x => x.GetContent(m_schemaStoreId))
                .Returns(doc);
            ObjectBuilder.AddCacheList(store.Object);

            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            var tracker = new MockRepository<Tracker>();
            tracker.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.Tracker]", new Tracker());
            ObjectBuilder.AddCacheList<IRepository<Tracker>>(tracker);
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new MockChangePublisher());



        }

        [Test]
        public async Task ThrowException()
        {
            dynamic wf =await CreateWorkflowInstanceAsync();
            wf.Status = "A";

            ActivityExecutionResult resultA = await wf.ExecuteAsync("B");
            Assert.AreEqual("F", resultA.NextActivities[0]);

        }
        [Test]
        public async Task Ok()
        {
            var wf =await CreateWorkflowInstanceAsync();
            wf.Status = "Something else";

            ActivityExecutionResult b = await wf.ExecuteAsync("B");
            Assert.AreEqual("C", b.NextActivities[0]);

        }
        [Test]
        public void SanitizeSingleThrow()
        {
            CompilerOptions options;
            var wd = CreateWorkflowDefinition(out options, "throw new InvalidOperationException(\"Test One\");");
            var b = wd.ActivityCollection.OfType<ExpressionActivity>().Single(x => x.Name == "B");

            var code = wd.SanitizeMethodBody(b);
            Console.WriteLine(code);
            StringAssert.DoesNotContain("item", code);
            StringAssert.DoesNotContain("result", code);

        }
        [Test]
        public async Task SingleThrow()
        {
            var wf =await CreateWorkflowInstanceAsync("throw new InvalidOperationException(\"Test One\");");
            wf.Status = "A";

            ActivityExecutionResult b = await wf.ExecuteAsync("B");
            Assert.AreEqual("F", b.NextActivities[0]);

        }

        private async Task<dynamic> CreateWorkflowInstanceAsync(string code = null)
        {
            CompilerOptions options;
            var wd = CreateWorkflowDefinition(out options, code);
            options.Emit = true;

            using (var ms = new MemoryStream())
            {
                options.Stream = ms;
                var cr =await wd.CompileAsync(options).ConfigureAwait(false);
                cr.Errors.ForEach(Console.WriteLine);
                Assert.IsTrue(cr.Result);
                var dll = Assembly.Load(cr.Buffer);
                var type = dll.GetType(wd.DefaultNamespace + "." + wd.WorkflowTypeName);
                dynamic wf = Activator.CreateInstance(type);
                wf.WorkflowDefinition = wd;
                return wf;
            }

        }

        private WorkflowDefinition CreateWorkflowDefinition(out CompilerOptions options, string code = null)
        {
            var wd = new WorkflowDefinition { Name = "Try and catch", Id = "try-and-catch", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Status", Type = typeof(string) });
            const string TRY_SCOPE = "tryscope1";
            const string CATCH_SCOPE = "catchscope1";
            const string EXPRESSION = @"
if(this.Status == ""A"")
{
    throw new InvalidOperationException(""Test message"");
}";
            var a = new ExpressionActivity
            {
                Name = "A",
                WebId = "A",
                TryScope = TRY_SCOPE,
                IsInitiator = true,
                NextActivityWebId = "B"
            };
            var b = new ExpressionActivity
            {
                Name = "B",
                WebId = "B",
                TryScope = TRY_SCOPE,
                NextActivityWebId = "C",
                Expression = code ?? EXPRESSION
            };
            var c = new ExpressionActivity { Name = "C", WebId = "C" };
            var f = new ExpressionActivity { Name = "F", WebId = "F", CatchScope = CATCH_SCOPE, NextActivityWebId = "D" };
            var d = new ExpressionActivity { Name = "D", WebId = "D", CatchScope = CATCH_SCOPE, NextActivityWebId = "E" };
            var e = new ExpressionActivity { Name = "E", WebId = "E", CatchScope = CATCH_SCOPE };
            var z = new ExpressionActivity { Name = "Z", WebId = "Z" };


            wd.ActivityCollection.Add(a);
            wd.ActivityCollection.Add(b);
            wd.ActivityCollection.Add(c);
            wd.ActivityCollection.Add(f);
            wd.ActivityCollection.Add(d);
            wd.ActivityCollection.Add(e);
            wd.ActivityCollection.Add(z);

            var tryScope = new TryScope
            {
                Id = TRY_SCOPE
            };

            var catchScope1 = new CatchScope
            {
                Id = CATCH_SCOPE,
                ExceptionType = typeof(InvalidOperationException).Name,
                ExceptionVar = "ioe"
            };

            tryScope.CatchScopeCollection.Add(catchScope1);
            wd.TryScopeCollection.Add(tryScope);
            options = new CompilerOptions();
            return wd;
        }
    }
}