using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class SolutionCompilerTestFixture
    {
        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public async Task CompileSimpleSolution()
        {
            var sol = new Solution { Id = "dev" };
            ObjectBuilder.ComposeMefCatalog(sol);
            var results = await sol.CompileAsync("DurandalJs");
            Console.WriteLine(results);
        }
    }
}
