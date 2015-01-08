using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Bespoke.Sph.Templating;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace durandaljs.compiler.test
{
    [TestClass]
    public class ExpressionTestFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
        }

        private async Task AssertAsync(string expected, string expression, string message = "")
        {

            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new ExpressionCompiler();
            var js = await compiler.CompileAsync(expression, patient);

            Assert.AreEqual(expected, js.Code, string.IsNullOrWhiteSpace(message) ? expression : message);
        }

        [TestMethod]
        public async Task ItemAndString()
        {
            await AssertAsync(
                "$data.Name()",
                "item.Name"
                );
        }

        [TestMethod]
        public async Task DateTimeToString()
        {

            await AssertAsync(
                "moment().format($data.Name())",
                "DateTime.Now.ToString(item.Name)"
                );
        }
    }
}
