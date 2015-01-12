using System;

namespace Bespoke.Sph.Domain
{
    public interface IDesignerMetadata
    {
        string Description { get; }
        string Category { get; }
        double Order { get; }
        string Name { get; }
        string TypeName { get; }
        string Route { get; }
        Type Type { get; }
        Type RouteTableProvider { get; }
        string FontAwesomeIcon { get; }
        string BootstrapIcon { get; }
        string PngIcon { get;  }
    }

}