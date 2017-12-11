namespace Bespoke.Sph.Domain.Compilers
{

    public interface IProjectDefinitionWithMembers : IProjectDefinition
    {
        ObjectCollection<Member> MemberCollection { get; }
    }
}