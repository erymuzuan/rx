using System.ComponentModel;

namespace Bespoke.Cycling.Windows.Infrastructure
{
    public interface IViewMetadata
    {
        string Name { get; }
        string Caption { get; }
        string Image { get; }
        string Uri { get; }
        string Module { get; }
        [DefaultValue(false)]
        bool IsHidden { get; }
        int Order { get; }
        string Role { get; }
        ViewGroup Group { get; }
        bool IsHome { get; }
        string Description { get; }
        string SubGroup { get; }
    }
}