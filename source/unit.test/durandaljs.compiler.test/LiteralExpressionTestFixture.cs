using System.Threading.Tasks;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    public class LiteralExpressionTestFixture : ExpressionTestFixture
    {
        [Test]
        public async Task ConstantDouble()
        {
            
            await AssertAsync<double>(
                "2",
                "2d");

        }

        [Test]
        public async Task String()
        {
            
            await AssertAsync<string>(
                "'Test 123'",
                "\"Test 123\"");

        }
    }
}