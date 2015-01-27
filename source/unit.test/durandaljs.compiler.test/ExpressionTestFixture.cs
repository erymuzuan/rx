﻿using System;
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

            Assert.AreEqual(expected, js.Code, string.IsNullOrWhiteSpace(message) ? "\r\n Original C# Expression: "+expression : message);
        }

        [Test]
        public async Task ItemAndString()
        {
            await AssertAsync<string>(
                "$data.Name()",
                "item.Name"
                );
        }

        [Test]
        public async Task DateTimeToString()
        {

            await AssertAsync<string>(
                "moment().format($data.Name())",
                "DateTime.Now.ToString(item.Name)"
                );
        }

        [Test]
        public async Task DefaultString()
        {

            await AssertAsync<string>(
                "null",
                ""
                );
        }
        [Test]
        public async Task DefaultInt32()
        {

            await AssertAsync<int>(
                "0",
                ""
                );
        }
        [Test]
        public async Task DefaultDecimal()
        {

            await AssertAsync<decimal>(
                "0",
                ""
                );
        }
        [Test]
        public async Task DefaultDouble()
        {

            await AssertAsync<decimal>(
                "0",
                ""
                );
        }
        [Test]
        public async Task DefaulSingle()
        {

            await AssertAsync<float>(
                "0",
                ""
                );
        }
        [Test]
        public async Task DefaulInt64()
        {

            await AssertAsync<long>(
                "0",
                ""
                );
        }
        [Test]
        public async Task DefaulInt16()
        {

            await AssertAsync<short>(
                "0",
                ""
                );
        }
        [Test]
        public async Task DefaultBoolean()
        {

            await AssertAsync<bool>(
                "false",
                ""
                );
        }
        [Test]
        public async Task DefaultDateTime()
        {

            await AssertAsync<DateTime>(
                "moment('0001-01-01')",
                ""
                );
        }

        [Test]
        public async Task MathAbs()
        {

            await AssertAsync<int>(
                "Math.abs($data.Age() || 0)",
                "Math.Abs(item.Age ?? 0)"
                );
        }
    }
}