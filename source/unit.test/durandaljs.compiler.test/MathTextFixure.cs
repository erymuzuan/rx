using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace durandaljs.compiler.test
{
    [TestClass]
    public class MathTextFixure : ExpressionTestFixture
    {
        [TestMethod]
        public async Task Abs()
        {
            
            await AssertAsync<double>(
                "Math.abs(2)",
                "Math.Abs(2d)");

        }
    }
}