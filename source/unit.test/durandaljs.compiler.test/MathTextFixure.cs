using System.Threading.Tasks;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class MathTextFixure : ExpressionTestFixture
    {
      


        [Test]
        public async Task Abs()
        {
            
            await AssertAsync<double>(
                "Math.abs(2)",
                "Math.Abs(2d)");

        }
    }
}