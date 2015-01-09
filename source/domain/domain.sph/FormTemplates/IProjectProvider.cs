using System.Collections.Generic;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public interface IProjectProvider
    {
        string DefaultNamespace { get; }
        string Name { get; }
        IEnumerable<Class> GenerateCode();
    }
}