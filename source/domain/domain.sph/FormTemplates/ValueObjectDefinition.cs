using System.Diagnostics;
using System.IO;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Name = {Name}")]
    [StoreAsSource(HasDerivedTypes = true)]
    public partial class ValueObjectDefinition : Entity
    {


        public void Save()
        {
            string childJsonFile = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(ValueObjectDefinition)}\\{Id}.json";
            File.WriteAllText(childJsonFile, this.ToJsonString(true));
        }
    }
}