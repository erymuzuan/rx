using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class MethodTestFixture
    {

        [Test]
        [Trace(Verbose = true)]
        public async Task LoggerInfoWithReturnNull()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"logger.Info(""The name is \""Slash\"" "");
return null;";
            var cr = await compiler.CompileAsync<string>(CODE, patient);

            Assert.IsTrue(cr.Success);
            Assert.AreEqual(@"logger.info('The name is ""Slash"" ');
return null;
", cr.Code);
        }



        [Test]
        [Trace(Verbose = true)]
        public async Task LoggerWithItem()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"logger.Info(""The name is "" + item.Name);
return null;";
            var cr = await compiler.CompileAsync<string>(CODE, patient);

            Assert.IsTrue(cr.Success);
            Assert.AreEqual(@"logger.info('The name is ' + $data.Name());
return null;
", cr.Code);
        }


        [Test]
        [Trace(Verbose = false)]
        public async Task StringFormat()
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new StatementCompiler();
            const string CODE = @"
            return string.Format("" Created on {0:d}"", item.CreatedDate);

";
            var cr = await compiler.CompileAsync<object>(CODE, patient);

            Assert.IsTrue(cr.Success);

            StringAssert.Contains("return String.format(' Created on {0:d}', $data.CreatedDate());", cr.Code);
        }
    }
}
