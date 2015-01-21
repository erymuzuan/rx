using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.FormCompilers.DurandalJs;
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
        [Trace(Verbose = true)]
        public void ClrTypeVariable()
        {
            var clr = new ClrTypeVariable
            {
                Type = typeof(Patient),
                Assembly = typeof(Address).GetShortAssemblyQualifiedName(),
                Name = "SamplePerson",
                CanInitiateWithDefaultConstructor = true,
            };

            var member = clr.CreateMember();
            Assert.AreEqual(6, member.MemberCollection.Count);
            Assert.AreEqual("Name", member.MemberCollection[0].Name);
            Assert.AreEqual("Age", member.MemberCollection[1].Name);
            Assert.AreEqual("Dob", member.MemberCollection[2].Name);
            Assert.AreEqual("Address", member.MemberCollection[3].Name);
            Assert.AreEqual("HomeAddress", member.MemberCollection[4].Name);
            Assert.AreEqual(2, member.MemberCollection[3].MemberCollection.Count);
            Assert.AreEqual("Street", member.MemberCollection[3].MemberCollection[0].Name);
            Assert.AreEqual("Postcode", member.MemberCollection[3].MemberCollection[1].Name);
            Assert.AreEqual("WorkAddressCollection", member.MemberCollection[5].Name);

            var compiler = new DurandalJsSolutionCompiler();
            var script = compiler.GenerateJavascriptClass(member, "xxxx", "Xxx", "ss");
            StringAssert.Contains("xxxx.domain.Patient", script);
            StringAssert.Contains("Address: ko.observable(new bespoke.xxxx.domain.Address()),", script);
            StringAssert.Contains("HomeAddress: ko.observable(new bespoke.xxxx.domain.Address()),", script);

            const string CHILD_CODE = "Address";
            StringAssert.Contains("xxxx.domain." + CHILD_CODE, script);
            Assert.AreEqual(1, new Regex(CHILD_CODE).Matches(script).Count, script);


        }

        [Test]
        [Trace(Verbose = true)]
        public async Task CompileWorkflowDefinitionModelClrTypeVariable()
        {
            work.VariableDefinitionCollection.Add(new ClrTypeVariable
            {
                Type = typeof(Address),
                Assembly = typeof(Address).GetShortAssemblyQualifiedName(),
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
            StringAssert.Contains("bespoke.TestATM_simplework.domain.SampleCrlVariable", results[0].Outputs[0]);
            StringAssert.Contains("bespoke.TestATM_simplework.domain.SampleChildCrlVariable", results[0].Outputs[0]);
        }
    }

    public class Patient
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime? Dob { get; set; }
        public Address Address { get; set; }
        public Address HomeAddress { get; set; }
        private readonly ObjectCollection<Address> m_childCollection = new ObjectCollection<Address>();

        public ObjectCollection<Address> WorkAddressCollection
        {
            get { return m_childCollection; }
        }
    }
    public class Address
    {
        public string Street { get; set; }
        public int Postcode { get; set; }
    }
}
