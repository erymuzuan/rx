using System.IO;
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

        public static dynamic GetCustomerInstance(this object testFixture)
        {
            var ed = testFixture.GetCustomerEntityDefinition();
            var type = CustomerEntityHelper.CompileEntityDefinition(ed);
            return CustomerEntityHelper.CreateCustomerInstance(type);
        }
    }
}
