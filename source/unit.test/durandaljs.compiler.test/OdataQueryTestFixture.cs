using System;
using System.Threading.Tasks;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Bespoke.Sph.OdataQueryCompilers;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class OdataQueryTestFixture : StatementTestFixture
    {

        private new async Task AssertAsync<T>(string expected, string code, string message = "")
        {
            var patient = HtmlCompileHelper.CreatePatientDefinition();
            var compiler = new ExpressionCompiler();
            var cr = await compiler.CompileAsync<T>(code, patient);

            var odataCompiler = new OdataQueryExpressionCompiler();

            var crr = odataCompiler.CompileExpression(cr.Tag.Statement, cr.Tag.SemanticModel);
            Assert.AreEqual(expected, crr, message);
        }

        [Test]
        public async Task Eq()
        {
            await AssertAsync<bool>("Mrn eq '123'", "item.Mrn == \"123\"");
        }
        [Test]
        public async Task Ne()
        {
            await AssertAsync<bool>("Mrn ne '123'", "item.Mrn != \"123\"");
        }
        [Test]
        public async Task Ge()
        {
            await AssertAsync<bool>("Age ge 25", "item.Age >= 25");
        }
        [Test]
        public async Task Gt()
        {
            await AssertAsync<bool>("Age gt 25", "item.Age > 25");
        }
        [Test]
        public async Task Lt()
        {
            await AssertAsync<bool>("Age lt 25", "item.Age < 25");
        }
        [Test]
        public async Task Le()
        {
            await AssertAsync<bool>("Age le 25", "item.Age <= 25");
        }

        [Test]
        public async Task DateTimeLe()
        {
            var date = new DateTime(2000, 1, 1);
            await AssertAsync<bool>("CreatedDate le DateTime'" + date.ToString("s") + "'", "item.CreatedDate <= new DateTime(2000,1,1)");
        }

        [Test]
        public async Task DateWithTime()
        {
            var date = new DateTime(2000, 1, 1, 10, 5, 5);
            await AssertAsync<bool>("CreatedDate le DateTime'" + date.ToString("s") + "'", "item.CreatedDate <= new DateTime(2000,1,1, 10,5,5)");
        }
    }
}