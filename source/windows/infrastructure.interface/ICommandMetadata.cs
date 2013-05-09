using Bespoke.Cycling.Windows.Infrastructure;

namespace Bespoke.Sph.Windows.Infrastructure
{
    public interface ICommandMetadata
    {
        bool IsHidden { get; set; }
        string Image { get; set; }
        string Caption { get; set; }
        string Name { get; set; }
        ViewGroup Group { get; set; }
        string SubGroup { get; set; }
        string Command { get; set; }
        int Order { get; set; }
        string Description { get; set; }
    }
}