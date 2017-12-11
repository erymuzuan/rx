namespace Bespoke.Sph.Domain.Compilers
{
    public interface IProjectDefinition
    {
        string AssemblyName { get; }
        string CodeNamespace { get; }
        string Name { get; }
        string Id { get; set; }

        //Task<IEnumerable<IProjectDefinition>> GetDependentProjectsAsync();
        //Task<IEnumerable<IProjectDefinition>> GetChildProjectsAsync();
        //IList<AttachedProperty> AttachedProperties { get; }
    }
}