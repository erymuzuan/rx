using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace domain.test
{
    public static class DynamicEntityProvider
    {

        public static EntityDefinition GetCustomerEntityDefinition(this object textFixture)
        {
            var customerDefinition = File.ReadAllText(Path.Combine(ConfigurationManager.SphSourceDirectory, "EntityDefinition/Customer.json"));
            var ed = customerDefinition.DeserializeFromJson<EntityDefinition>();

            var attachment = ed.MemberCollection.Single(m => m.Name == "Contact")
                .MemberCollection.Single(m => m.Name == "AttachmentCollection");
            attachment.AllowMultiple = true;
            attachment.TypeName = null;

            return ed;

        }

        public static async Task<dynamic> GetCustomerInstanceAsync(this object testFixture)
        {
            var ed = testFixture.GetCustomerEntityDefinition();
            var type = await CustomerEntityHelper.CompileEntityDefinitionAsync(ed);
            return CustomerEntityHelper.CreateCustomerInstance(type);
        }
    }
}
