using System;

namespace Bespoke.Sph.Domain
{
    public interface IDesignerMetadata
    {
        string Description { get; }
        string Category { get; }
        string Name { get; }
        string Route { get; }
        Type Type { get; }
        Type RouteTableProvider { get; }
        string FontAwesomeIcon { get; }
        string BootstrapIcon { get; }
        string PngIcon { get;  }
    }

}