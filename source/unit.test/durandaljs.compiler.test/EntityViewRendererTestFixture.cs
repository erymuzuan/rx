using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Bespoke.Sph.Templating;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    class EntityViewRendererTestFixture
    {
        // ReSharper disable InconsistentNaming
        private EntityDefinition patient;
        // ReSharper restore InconsistentNaming
        [TestFixtureSetUp]
        public void SetUp()
        {
            patient = HtmlCompileHelper.CreatePatientDefinition();
            var repos = new MockRepository<EntityDefinition>();
            repos.AddToDictionary("x.Id == view.EntityDefinitionId", patient);
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(repos);
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());

        }



        [Test]
        [Trace(Verbose = false)]
        public async Task View()
        {
            var view = new EntityView
            {
                Route = "test-view",
                Name = "This is a test",
                Id = "test-view"
            };
            view.AddFilter("Age", Operator.Le, new ConstantField { Type = typeof(int), Value = 25 });
            view.ViewColumnCollection.Add(new ViewColumn { Header = "Mrn", Path = "Mrn" });
            view.ViewColumnCollection.Add(new ViewColumn { Header = "Name", Path = "Name" });

            var compiler = new EntityViewHtmlRenderer();
            ObjectBuilder.ComposeMefCatalog(compiler);
            var html = await compiler.GenerateCodeAsync(view, patient);
            StringAssert.Contains("Mrn", html);
        }

        [Test]
        [Trace(Verbose = false)]
        public async Task ViewModelJs()
        {
            var view = new EntityView
            {
                Route = "test-view",
                Name = "This is a test",
                Id = "test-view"
            };
            view.AddFilter("Age", Operator.Le, new ConstantField { Type = typeof(int), Value = 25 });
            view.ViewColumnCollection.Add(new ViewColumn { Header = "Mrn", Path = "Mrn" });
            view.ViewColumnCollection.Add(new ViewColumn { Header = "Name", Path = "Name" });


            var compiler = new EntityViewJsRenderer();
            ObjectBuilder.ComposeMefCatalog(compiler);
            var js = await compiler.GenerateCodeAsync(view, patient);


            var filter = view.GenerateElasticSearchFilterDsl();
            StringAssert.Contains(filter, js);
        }

    }
}