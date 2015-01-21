using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.Templating;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class SolutionCompilerTestFixture
    {
        // ReSharper disable InconsistentNaming
        private EntityDefinition course;
        private WorkflowDefinition work;
        // ReSharper restore InconsistentNaming
        [TestFixtureSetUp]
        public void SetUp()
        {
            course = new EntityDefinition { Name = "Course", Id = "course", RecordName = "No" };
            course.MemberCollection.Add(new Member { Type = typeof(string), Name = "No" });
            course.MemberCollection.Add(new Member { Type = typeof(string), Name = "Name" });
            course.MemberCollection.Add(new Member { Type = typeof(int), Name = "Rating" });
            course.MemberCollection.Add(new Member { Type = typeof(string), Name = "Author" });

            work = new WorkflowDefinition { Id = "simple-work", Name = "Simple Work" };

            var edr = new MockRepository<EntityDefinition>();
            edr.AddToDictionary("x.Name == pm.Name", course);

            var wdr = new MockRepository<WorkflowDefinition>();
            wdr.AddToDictionary("x.Name == pm.Name", work);


            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(edr);
            ObjectBuilder.AddCacheList<IRepository<WorkflowDefinition>>(wdr);
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());

        }

        [Test]
        [Trace(Verbose = false)]
        public async Task CompileEntityDefinitionModel()
        {
            var sol = new Solution { Id = "dev" };
            sol.ProjectMetadataCollection.Add(new ProjectMetadata
            {
                Name = "Course",
                Type = typeof(EntityDefinition)
            });

            ObjectBuilder.ComposeMefCatalog(sol);
            var results = (await sol.CompileAsync("DurandalJs")).ToList();
            if (DebuggerHelper.IsVerbose)
                results.SelectMany(x => x.Outputs).ToList().ForEach(Console.WriteLine);

            StringAssert.Contains("domain.Course", results[0].Outputs[0]);
        }
        [Test]
        [Trace(Verbose = false)]
        public async Task CompileWorkflowDefinitionModelSimpleVariable()
        {
            work.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Mrn", Type = typeof(string) });

            var sol = new Solution { Id = "dev" };
            sol.ProjectMetadataCollection.Add(new ProjectMetadata
            {
                Name = "Simple Work",
                Type = typeof(WorkflowDefinition)
            });

            ObjectBuilder.ComposeMefCatalog(sol);
            var results = (await sol.CompileAsync("DurandalJs")).ToList();
            if (DebuggerHelper.IsVerbose)
                results.SelectMany(x => x.Outputs).ToList().ForEach(Console.WriteLine);

            StringAssert.Contains("_simplework.domain.SimpleWorkWorkflow", results[0].Outputs[0]);
            StringAssert.Contains("Mrn: ko.observable(),", results[0].Outputs[0]);
        }
        [Test]
        [Trace(Verbose = false)]
        public async Task CompileWorkflowDefinitionModelClrTypeVariable()
        {
            work.VariableDefinitionCollection.Add(new ClrTypeVariable
            {
                Type = typeof(SampleCrlVariable),
                Assembly = typeof(SampleCrlVariable).GetShortAssemblyQualifiedName(),
                Name = "SamplePerson",
                CanInitiateWithDefaultConstructor = true,
            });


            var sol = new Solution { Id = "dev" };
            sol.ProjectMetadataCollection.Add(new ProjectMetadata
            {
                Name = "Simple Work",
                Type = typeof(WorkflowDefinition)
            });

            ObjectBuilder.ComposeMefCatalog(sol);
            var results = (await sol.CompileAsync("DurandalJs")).ToList();
            if (DebuggerHelper.IsVerbose)
                results.SelectMany(x => x.Outputs).ToList().ForEach(Console.WriteLine);

            StringAssert.Contains("_simplework.domain.SimpleWorkWorkflow", results[0].Outputs[0]);
            StringAssert.Contains("Mrn: ko.observable(),", results[0].Outputs[0]);
        }
    }

    public class SampleCrlVariable
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime? Dob { get; set; }
        public SampleChildCrlVariable CrlVariable { get; set; }
        private readonly ObjectCollection<SampleChildCrlVariable> m_childCollection = new ObjectCollection<SampleChildCrlVariable>();

        public ObjectCollection<SampleChildCrlVariable> ChildCollection
        {
            get { return m_childCollection; }
        }
    }
    public class SampleChildCrlVariable
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
