using NUnit.Framework;
using System.Threading.Tasks;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class DecimalExpressionTestFixture : ExpressionTestFixture
    {

        [Test]
        public async Task ParseInt()
        {
            await AssertAsync<decimal>(
                "parseInt('25')",
                "int.Parse(\"25\")"
                    );
        }


        [Test]
        public async Task DecimalZero()
        {
            await AssertAsync<decimal>(
                "0",
                "decimal.Zero"
                    );
        }
        [Test]
        public async Task DecimalOne()
        {
            await AssertAsync<decimal>(
                "1",
                "decimal.One"
                    );
        }

        [Test]
        public async Task DecimalRound()
        {
            await AssertAsync<decimal>(
                "1.24879889798897898795.toFixed(0)",
                "decimal.Round(1.24879889798897898795m)"
                    );
        }

        [Test]
        public async Task DecimalRound2()
        {
            await AssertAsync<decimal>(
                "1.24879889798897898795.toFixed(2)",
                "decimal.Round(1.24879889798897898795m, 2)"
                    );
        }
        [Test]
        public async Task Add()
        {
            await AssertAsync<decimal>(
                "1 + 2",
                "1 + 2"
                    );
        }
        [Test]
        public async Task Substract()
        {
            await AssertAsync<decimal>(
                "1 - 2",
                "1 - 2"
                    );
        }


        [Test]
        public async Task DecimalMinusOne()
        {
            await AssertAsync<decimal>(
                "-1",
                "decimal.MinusOne"
                    );
        }

        [Test]
        public async Task ParseInt32()
        {
            await AssertAsync<decimal>(
                "parseInt('25')",
                "Int32.Parse(\"25\")"
                    );
        }

        [Test]
        public async Task IntMax()
        {
            await AssertAsync<decimal>(
                "Infinity",
                "int.MaxValue"
                    );
           
        }

        [Test]
        public async Task IntMinValue()
        {
            await AssertAsync<decimal>(
                "-Infinity",
                "int.MinValue"
                    );
           
        }
        

    }
}