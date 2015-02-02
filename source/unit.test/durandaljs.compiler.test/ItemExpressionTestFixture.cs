using System.Threading.Tasks;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    public class ItemExpressionTestFixture : ExpressionTestFixture
    {
        [Test]
        public async Task Name()
        {
            await AssertAsync<string>(
                "$data.Name()",
                "item.Name");

        }
        [Test]
        public async Task AddressCountry()
        {
            await AssertAsync<string>(
                "$data.Address().Country()",
                "item.Address.Country");
        }
    }
}