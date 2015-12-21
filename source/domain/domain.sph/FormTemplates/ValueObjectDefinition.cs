using System.Diagnostics;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Name = {Name}")]
    [StoreAsSource(HasDerivedTypes = true)]
    public class ValueObjectDefinition : Entity
    {
        public string Name { get; set; }
        public ObjectCollection<Member> MemberCollection { get; } = new ObjectCollection<Member>();
    }
}