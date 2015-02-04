using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class StringTestFixture : ExpressionTestFixture
    {
       

        [Test]
        public async Task NotIsNullOrWhiteSpace()
        {

            await AssertAsync<bool>(
                "!String.isNullOrWhiteSpace($data.Name())",
                "!string.IsNullOrWhiteSpace(item.Name)");
        }

        [Test]
        public async Task Concat()
        {
           await AssertAsync<string>(
               "'test' + $data.Mrn()",
               "\"test\" + item.Mrn");
        }
        [Test]
        [Trace(Verbose = false)]
        public async Task Empty()
        {
            await AssertAsync<string>("''", @"string.Empty");
        }
        [Test]
        [Trace(Verbose = false)]
        public async Task IsNullOrWhiteSpace()
        {
            await AssertAsync<bool>("String.isNullOrWhiteSpace($data.Name())", @"string.IsNullOrWhiteSpace(item.Name)");
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

            StringAssert.Contains("return String.format(' Created on {0:d}', $data.CreatedDate().moment());", cr.Code);
        }
    }
}