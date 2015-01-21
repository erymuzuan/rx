using System;
using System.Linq;
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
        // ReSharper restore InconsistentNaming
        [TestFixtureSetUp]
        public void SetUp()
        {
            course = new EntityDefinition { Name = "Course", Id = "course", RecordName = "No" };
            course.MemberCollection.Add(new Member { Type = typeof(string), Name = "No" });
            course.MemberCollection.Add(new Member { Type = typeof(string), Name = "Name" });
            course.MemberCollection.Add(new Member { Type = typeof(int), Name = "Rating" });
            course.MemberCollection.Add(new Member { Type = typeof(string), Name = "Author" });

            var repos = new MockRepository<EntityDefinition>();
            repos.AddToDictionary("x.Name Equal pm.Name", course);
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(repos);
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());

        }

        [Test]
        [Trace(Verbose = false)]
        public async Task CompileSimpleSolution()
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

            StringAssert.Contains("domain.Course",results[0].Outputs[0]);
        }
    }
}
