using System;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Bespoke.Sph.Templating;
using Moq;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    class ScreenActivityFormRendererTestFixture
    {
        // ReSharper disable InconsistentNaming
        private WorkflowDefinition wd;
        // ReSharper restore InconsistentNaming
        [TestFixtureSetUp]
        public void SetUp()
        {
            var doc = new BinaryStore
            {
                Content = File.ReadAllBytes(@"C:\project\work\sph\source\unit.test\domain.test\bin\Debug\workflows\PemohonWakaf.xsd")
            };
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);
            store.Setup(x => x.GetContent("x"))
                .Returns(doc);
            ObjectBuilder.AddCacheList(store.Object);

            wd = new WorkflowDefinition { Id = "simple-screen-workflow", SchemaStoreId = "x", Name = "Simple Screen Workflow" };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "CourseNo", Type = typeof(string) });
            var repos = new MockRepository<WorkflowDefinition>();
            repos.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.WorkflowDefinition]", wd);
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<WorkflowDefinition>>(repos);
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());

        }

        [Test]
        public void Debuggin()
        {
            Assert.IsFalse(DebuggerHelper.IsVerbose);
        }
        [Test]
        [TraceAttribute(Verbose = true)]
        public void Verbose()
        {
            Assert.IsTrue(DebuggerHelper.IsVerbose);
        }
        [Test]
        [TraceAttribute(Verbose = false)]
        public void NotVerbose()
        {
            Assert.IsFalse(DebuggerHelper.IsVerbose);
        }

        [Test]
        public async Task WithDurandalJsFormCompiler()
        {
            var form = new ScreenActivityForm
            {
                Route = "test-form",
                Name = "This is a test",
                Id = "test-form"
            };
            string code = "DateTime.Now";// + DateTime.Now.ToString("g");
            var button = new Button
            {
                ElementId = "button",
                CommandName = "buttonClick",
                IsAsynchronous = true,
                Command = code,
                IsToolbarItem = true
            };
            form.FormDesign.FormElementCollection.Add(button);
            form.FormDesign.FormElementCollection.Add(new TextBox { Path = "No", Label = "Course No", ElementId = "noTextBox", Tooltip = "Course No" });
            form.FormDesign.FormElementCollection.Add(new TextBox { Path = "Name", Label = "Course Name", ElementId = "nameTextBox", Tooltip = "Course Name" });


            var a = new ScreenActivity { Name = "Start Me", WebId = "a", FormId = "test-form", IsInitiator = true, NextActivityWebId = "b" };
            var b = new ExpressionActivity { Name = "Warn Me", WebId = "b", Expression = "Console.WriteLine(\"Hello {0}\", this.CourseNo);", NextActivityWebId = "c" };
            var c = new EndActivity { Name = "End Me", WebId = "c" };
            wd.ActivityCollection.Add(a);
            wd.ActivityCollection.Add(b);
            wd.ActivityCollection.Add(c);

            var compiler = new DurandalJsFormCompiler();
            ObjectBuilder.ComposeMefCatalog(compiler);
            var result = await compiler.CompileAsync(form);
            Console.WriteLine(result);
        }

    }
}