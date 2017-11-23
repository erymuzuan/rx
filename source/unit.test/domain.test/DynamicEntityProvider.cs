using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace domain.test
{
    public static class DynamicEntityProvider
    {

        public static EntityDefinition GetCustomerEntityDefinition(this object textFixture)
        {
            var customerDefinition = File.ReadAllText($@"{ConfigurationManager.SphSourceDirectory}\EntityDefinition\Customer.json");
            return customerDefinition.DeserializeFromJson<EntityDefinition>();

        }

        public static async Task<dynamic> GetCustomerInstanceAsync(this object testFixture)
        {
            var ed = testFixture.GetCustomerEntityDefinition();
            var type = await CustomerEntityHelper.CompileEntityDefinitionAsync(ed);
            return CustomerEntityHelper.CreateCustomerInstance(type);
        }
    }
}
