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

        private EntityDefinition CreateCourseInstance()
        {
            var course = new EntityDefinition { Name = "Course", Id = "course", RecordName = "No" };
            course.MemberCollection.Add(new Member { Type = typeof(string), Name = "No" });
            course.MemberCollection.Add(new Member { Type = typeof(string), Name = "Name" });
            course.MemberCollection.Add(new Member { Type = typeof(int), Name = "Rating" });
            course.MemberCollection.Add(new Member { Type = typeof(string), Name = "Author" });

            var edr = new MockRepository<EntityDefinition>();
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(edr);

            edr.AddToDictionary(XNamePmName, course);
            return course;
        }

        private WorkflowDefinition CreateWorkflowInstance()
        {
            var wdr = new MockRepository<WorkflowDefinition>();
            ObjectBuilder.AddCacheList<IRepository<WorkflowDefinition>>(wdr);

            var work = new WorkflowDefinition { Id = "simple-work", Name = "Simple Work" };
            wdr.AddToDictionary(XNamePmName, work);
            return work;
        }
        // ReSharper restore InconsistentNaming
        private const string XNamePmName = "x.Name == pm.Name";
        [TestFixtureSetUp]
        public void SetUp()
        {


            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());

        }

        [Test]
        [Trace(Verbose = false)]
        public async Task CompileEntityDefinitionModelWithAggregate()
        {
            var course = this.CreateCourseInstance();
            var tutor = new Member { Name = "Tutor" };
            tutor.AddMember("Age", typeof(int));
            tutor.AddMember("Name", typeof(string));
            tutor.AddMember("Title", typeof(string));
            course.MemberCollection.Add(tutor);


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
        public void GenerateCodeEntityDefinitionModelWithAggregate()
        {
            var course = this.CreateCourseInstance();
            var tutor = new Member { Name = "Tutor" };
            tutor.AddMember("Age", typeof(int));
            tutor.AddMember("Name", typeof(string));
            tutor.AddMember("Title", typeof(string));

            var experiences = new Member { Name = "Experiences", AllowMultiple = true };
            experiences.AddMember("Title", typeof(string));
            experiences.AddMember("Years", typeof(int));


            tutor.MemberCollection.Add(experiences);
            course.MemberCollection.Add(tutor);

            var codes = course.GenerateCode().ToList();
            Assert.AreEqual(1, codes.Count(x => x.Name == "Tutor"));

            var @class = codes.Single(x => x.Name == "Tutor");
            Assert.AreEqual(1, @class.PropertyCollection.Count(x => x.Name == "Age"));
            codes.Where(x => !x.Name.Contains("Controller")).ToList().ForEach(Console.WriteLine);
        }


        [Test]
        [Trace(Verbose = false)]
        public void GenerateCodeEntityDefinitionWithCollection()
        {
            var course = this.CreateCourseInstance();
            var ratings = new Member { Name = "RatingCollection", AllowMultiple = true };
            ratings.AddMember("Star", typeof(int));
            ratings.AddMember("Date", typeof(DateTime));
            ratings.AddMember("User", typeof(string));
            course.MemberCollection.Add(ratings);


            var codes = course.GenerateCode().ToList();
            codes.ForEach(Console.WriteLine);
            Assert.AreEqual(1, codes.Count(x => x.Name == "Rating"));

            var @class = codes.Single(x => x.Name == "Rating");
            Assert.AreEqual(1, @class.PropertyCollection.Count(x => x.Name == "Star"));
            Console.WriteLine(@class);
        }
        [Test]
        [Trace(Verbose = false)]
        public void GenerateCodeEntityDefinitionWithPlural()
        {
            var course = this.CreateCourseInstance();
            var ratings = new Member { Name = "Ratings", AllowMultiple = true };
            ratings.AddMember("Star", typeof(int));
            ratings.AddMember("Date", typeof(DateTime));
            ratings.AddMember("User", typeof(string));
            course.MemberCollection.Add(ratings);


            var codes = course.GenerateCode().ToList();
            codes.ForEach(Console.WriteLine);
            Assert.AreEqual(1, codes.Count(x => x.Name == "Rating"));

            var @class = codes.Single(x => x.Name == "Rating");
            Assert.AreEqual(1, @class.PropertyCollection.Count(x => x.Name == "Star"));
            Console.WriteLine(@class);
        }
        [Test]
        [Trace(Verbose = false)]
        public async Task CompileEntityDefinitionModel()
        {
            var course = this.CreateCourseInstance();
            var ratings = new Member { Name = "Ratings", AllowMultiple = true };
            ratings.AddMember("Star", typeof(int));
            ratings.AddMember("Date", typeof(DateTime));
            ratings.AddMember("User", typeof(string));
            course.MemberCollection.Add(ratings);


            var tutor = new Member { Name = "Tutor" };
            tutor.AddMember("Age", typeof(int));
            tutor.AddMember("Name", typeof(string));
            tutor.AddMember("Title", typeof(string));
            course.MemberCollection.Add(tutor);


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

            StringAssert.Contains("Ratings : ko.observableArray([]),", results[0].Outputs[0]);
            StringAssert.Contains("domain.Course = function(", results[0].Outputs[0]);
            StringAssert.Contains("domain.Rating = function(", results[0].Outputs[0]);
        }
        [Test]
        [Trace(Verbose = false)]
        public async Task CompileWorkflowDefinitionModelSimpleVariable()
        {
            var work = this.CreateWorkflowInstance();
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
            StringAssert.Contains("Mrn : ko.observable(),", results[0].Outputs[0]);
        }


        [Test]
        [Trace(Verbose = true)]
        public void CreatePropertyOnMemberComplexCollection()
        {
            var member = new Member { Name = "AddressCollection", Type = typeof(Address), AllowMultiple = true };
            var prop = member.CreateProperty();
            Assert.AreEqual("AddressCollection", prop.Name);
            Assert.IsTrue(prop.Initialized, prop.Code);
            Assert.IsTrue(prop.IsReadOnly, prop.Code);
            Console.WriteLine(prop);
        }
        [Test]
        [Trace(Verbose = true)]
        public void CreatePropertyOnMemberNativeArray()
        {
            var member = new Member { Name = "NickNames", Type = typeof(string), AllowMultiple = true };
            var prop = member.CreateProperty();
            Assert.AreEqual("NickNames", prop.Name);
            Assert.IsTrue(prop.Initialized, prop.Code);
            Assert.IsTrue(prop.IsReadOnly, prop.Code);
            Console.WriteLine(prop);
        }
        [Test]
        [Trace(Verbose = true)]
        public void CreatePropertyOnMemberNativeNullableArray()
        {
            var member = new Member { Name = "Points", Type = typeof(int), IsNullable = true, AllowMultiple = true };
            var prop = member.CreateProperty();
            Assert.AreEqual("Points", prop.Name);
            Assert.IsTrue(prop.Initialized, prop.Code);
            Assert.IsTrue(prop.IsReadOnly, prop.Code);
            Console.WriteLine(prop);
        }
        [Test]
        [Trace(Verbose = true)]
        public void CreatePropertyOnMemberSimpleString()
        {
            var member = new Member { Name = "Name", Type = typeof(string), IsNullable = true, AllowMultiple = false };
            var prop = member.CreateProperty();
            Assert.AreEqual("Name", prop.Name);
            Assert.IsFalse(prop.Initialized, prop.Code);
            Assert.IsFalse(prop.IsReadOnly, prop.Code);
            Console.WriteLine(prop);
        }
        [Test]
        [Trace(Verbose = true)]
        public void CreatePropertyOnMemberSimpleInt()
        {
            var member = new Member { Name = "Age", Type = typeof(int), IsNullable = false, AllowMultiple = false };
            var prop = member.CreateProperty();
            Assert.AreEqual("Age", prop.Name);
            Assert.IsFalse(prop.Initialized, prop.Code);
            Assert.IsFalse(prop.IsReadOnly, prop.Code);
            Console.WriteLine(prop);
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
            Assert.AreEqual(1, new Regex("xxxx\\.domain\\." + CHILD_CODE + " = function").Matches(script).Count, script);


        }




        [Test]
        [Trace(Verbose = false)]
        public void WdWithClrVariable_CheckMembers()
        {
            var work = CreateWorkflowInstance();
            work.VariableDefinitionCollection.Add(new ClrTypeVariable
            {
                Type = typeof(Patient),
                Assembly = typeof(Patient).GetShortAssemblyQualifiedName(),
                Name = "Pesakit",
                CanInitiateWithDefaultConstructor = true,
            });

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("iteration : {0}", i + 1);
                var members = work.Members.ToList();
                Assert.AreEqual(1, members.Count);
                var pesakit = members.First();
                Assert.AreEqual("Pesakit", pesakit.Name);
                Assert.AreEqual(typeof(Patient), pesakit.Type);
                Assert.IsTrue(pesakit.IsComplex);
                Assert.IsTrue(string.IsNullOrWhiteSpace(pesakit.InferredType));
                Assert.AreEqual(6, pesakit.MemberCollection.Count);
            }
        }

        [Test]
        [Trace(Verbose = false)]
        public async Task WdWithClrVariable_GenerateJs()
        {
            var work = CreateWorkflowInstance();
            work.VariableDefinitionCollection.Add(new ClrTypeVariable
            {
                Type = typeof(Patient),
                Assembly = typeof(Patient).GetShortAssemblyQualifiedName(),
                Name = "Pesakit",
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

            StringAssert.Contains("_simplework.domain.SimpleWorkWorkflow = function(", results[0].Outputs[0]);
            StringAssert.Contains("bespoke.TestATM_simplework.domain.Patient = function(", results[0].Outputs[0]);
            StringAssert.Contains("bespoke.TestATM_simplework.domain.Address = function(", results[0].Outputs[0]);
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
