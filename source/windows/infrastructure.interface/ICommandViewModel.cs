using System.Collections.Generic;

namespace Bespoke.Cycling.Windows.Infrastructure
{
    public interface ICommandViewModel
    {
        string Name { get; }
        IEnumerable<CommandItem<object>> Commands { get; } 
    }
}
