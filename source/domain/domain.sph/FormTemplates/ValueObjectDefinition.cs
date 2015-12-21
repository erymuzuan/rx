using System.Diagnostics;
using System.IO;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Name = {Name}")]
    [StoreAsSource(HasDerivedTypes = true)]
    public class ValueObjectDefinition : Entity
    {
        public string Name { get; set; }
        public ObjectCollection<Member> MemberCollection { get; } = new ObjectCollection<Member>();

        public void Save()
        {
            string childJsonFile = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(ValueObjectDefinition)}\\{Id}.json";
            File.WriteAllText(childJsonFile, this.ToJsonString(true));
        }
    }
}