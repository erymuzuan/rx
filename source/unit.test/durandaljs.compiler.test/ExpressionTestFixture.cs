using System.Diagnostics;
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

        private async Task AssertAsync<T>(string expected, string expression, string message = "")
        {

            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new ExpressionCompiler();
            var js = await compiler.CompileAsync<T>(expression, patient);

            Assert.AreEqual(expected, js.Code, string.IsNullOrWhiteSpace(message) ? expression : message);
        }

        [TestMethod]
        public async Task ItemAndString()
        {
            await AssertAsync<string>(
                "$data.Name()",
                "item.Name"
                );
        }

        [TestMethod]
        public async Task DateTimeToString()
        {

            await AssertAsync<string>(
                "moment().format($data.Name())",
                "DateTime.Now.ToString(item.Name)"
                );
        }


        [TestMethod]
        [Ignore]
        public async Task DateTimeToStringFormatd()
        {
            // TODO: need to translate .Net date format string to moment format string
            Process.Start("http://momentjs.com/docs/#/displaying/");
            await AssertAsync<string>(
                "moment().format('DD/MM/YYYY')",
                "DateTime.Now.ToString(\"d\")"
                );
        }
    }
}
