using System.Windows.Threading;
using Bespoke.Cycling.Windows.Infrastructure;

namespace Bespoke.Sph.Windows.Infrastructure
{
    public interface IView
    {
        DispatcherObject View { get; set; }
        bool Confirm(string message);
        bool Alert(string message, AlertImage image);
    }
}
