using System;

namespace Bespoke.Sph.Domain
{
    public interface IDesignerMetadata
    {
        string Description { get; }
        string Category { get; }
        string Name { get; }
        Type Type { get; }
        string FontAwesomeIcon { get; }
        string BootstrapIcon { get; }
        string PngIcon { get;  }
    }

}