using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Bespoke.Sph.FormCompilers.DurandalJs.FormRenderers;
using Bespoke.Sph.Templating;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    class EntityFormViewModelTestFixture
    {
        private EntityDefinition m_ed;
        [TestFixtureSetUp]
        public void SetUp()
        {
            m_ed = new EntityDefinition { Name = "Course", Id = "course", RecordName = "No" };
            m_ed.MemberCollection.Add(new Member { Type = typeof(string), Name = "No" });
            m_ed.MemberCollection.Add(new Member { Type = typeof(string), Name = "Name" });
            m_ed.MemberCollection.Add(new Member { Type = typeof(int), Name = "Rating" });
            m_ed.MemberCollection.Add(new Member { Type = typeof(string), Name = "Author" });

            var repos = new MockRepository<EntityDefinition>();
            repos.AddToDictionary("x.Id == value(Bespoke.Sph.Domain.EntityForm).EntityDefinitionId", m_ed);
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(repos);
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());

        }

        [Test]
        [Trace(Verbose = false)]
        public async Task WithDurandalJsFormCompiler()
        {
            var form = new EntityForm
            {
                Route = "this-is-a-test",
                Name = "This is a test",
                Partial = "partial/this-is-a-test",
                IsEmailAvailable = true
            };
            form.Rules.Add("rule1");
            form.Rules.Add("rule2");
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
            form.FormDesign.FormElementCollection.Add(new TextBox{Path = "No", Label = "Course No", ElementId = "noTextBox", Tooltip = "Course No"});
            form.FormDesign.FormElementCollection.Add(new TextBox{Path = "Name", Label = "Course Name", ElementId = "nameTextBox", Tooltip = "Course Name"});
            form.FormDesign.FormElementCollection.Add(new TextBox{Path = "Author", Label = "Author", ElementId = "authorTextBox", Tooltip = "Course Author"});


            var register = new EntityOperation { Name = "RegisterCourse", ShowSuccessMessage = true, SuccessMessage = "Ok done", NavigateSuccessUrl = "course" };
            var dereg = new EntityOperation { Name = "DeregisterCourse" };

            m_ed.EntityOperationCollection.Add(register);
            m_ed.EntityOperationCollection.Add(dereg);

            var compiler = new DurandalJsFormCompiler();
            ObjectBuilder.ComposeMefCatalog(compiler);
            var result = await compiler.CompileAsync(form);
            Console.WriteLine(result);
        }
        [Test]
        public async Task SimpleForm()
        {
            var form = new EntityForm
            {
                Route = "this-is-a-test",
                Name = "This is a test",
                Partial = "partial/this-is-a-test",
                IsEmailAvailable = true
            };
            form.Rules.Add("rule1");
            form.Rules.Add("rule2");

            var register = new EntityOperation { Name = "RegisterCourse", ShowSuccessMessage = true, SuccessMessage = "Ok done", NavigateSuccessUrl = "course" };
            var dereg = new EntityOperation { Name = "DeregisterCourse" };
            var ed = new EntityDefinition { Name = "Course", Id = "course" };

            ed.EntityOperationCollection.Add(register);
            ed.EntityOperationCollection.Add(dereg);

            var compiler = new EntityFormJsViewModelRenderer();
            var js = await compiler.GenerateCodeAsync(form, ed);
            Console.WriteLine(js);
        }
    }
}
