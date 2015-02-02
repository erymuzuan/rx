using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Bespoke.Sph.Templating;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class ExpressionTestFixture
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
            var compiler = new ExpressionCompiler();
            var js = await compiler.CompileAsync<T>(expression, patient);

            if (!js.Success)
            {
                Console.WriteLine(" BUILD ERROR !!!");
                js.DiagnosticCollection.ForEach(Console.WriteLine);
            }

            Assert.AreEqual(expected, js.Code, string.IsNullOrWhiteSpace(message) ? "\r\n Original C# Expression: "+expression : message);
        }

    
    }
}
