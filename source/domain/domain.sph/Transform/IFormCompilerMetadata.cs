using System;

namespace Bespoke.Sph.Domain
{
    public interface IFormCompilerMetadata
    {
        Type Type { get; }
        string Description { get; }
        string Name { get; }
        bool IsSupported{ get;  }
    }
}