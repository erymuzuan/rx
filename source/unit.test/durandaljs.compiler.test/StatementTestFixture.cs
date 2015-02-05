using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Bespoke.Sph.Templating;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class StatementTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
        }

        public async Task AssertAsync<T>(string expected, string expression, string message = "")
        {

            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            var js = await compiler.CompileAsync<T>(expression, patient);

            if (!js.Success)
            {
                Console.WriteLine(" BUILD ERROR !!!");
                js.DiagnosticCollection.ForEach(Console.WriteLine);
            }

            AssertCodes(expected, js, string.IsNullOrWhiteSpace(message) ? "\r\n Original C# Expression: " + expression : message);
        }

        private static void AssertCodes(string expected, SnippetCompilerResult cr, string message)
        {
            Assert.IsTrue(cr.Success, message);
            expected.Split(new[] { "\r\n","\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList()
                .ForEach(x => StringAssert.Contains(x, cr.Code, message));
        }


    }
}