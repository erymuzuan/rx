using System.Collections.Generic;

namespace Bespoke.Sph.Windows.Infrastructure
{
    public interface ICommandViewModel
    {
        string Name { get; }
        IEnumerable<CommandItem<object>> Commands { get; } 
    }
}
