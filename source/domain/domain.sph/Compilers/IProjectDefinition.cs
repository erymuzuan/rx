using System.Collections.Generic;

namespace Bespoke.Sph.Domain.Compilers
{
    public interface IProjectDefinition
    {
        string CodeNamespace { get; }
        string Name { get; }
        string Id { get; set; }
        //IList<AttachProperty> AttachedProperties { get; }
    }
}