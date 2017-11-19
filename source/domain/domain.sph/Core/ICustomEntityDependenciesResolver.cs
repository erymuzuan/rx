using System;

namespace Bespoke.Sph.Domain
{
    public interface ICustomEntityDependenciesResolver
    {
        (Type DependencyType, dynamic Implementation) ResolveRepository(EntityDefinition ed);
        (Type DependencyType, dynamic Implementation) ResolveReadOnlyRepository(EntityDefinition ed);
    }
}