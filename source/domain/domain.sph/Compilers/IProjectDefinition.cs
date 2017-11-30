using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Compilers
{
    public interface IProjectDefinition
    {
        string AssemblyName { get; }
        string CodeNamespace { get; }
        string Name { get; }
        string Id { get; set; }

        //Task<IEnumerable<IProjectDefinition>> GetDependentProjectAsync();
        //Task<IEnumerable<IProjectDefinition>> GetChildProjectsAsync();
        //IList<AttachProperty> AttachedProperties { get; }
    }
}