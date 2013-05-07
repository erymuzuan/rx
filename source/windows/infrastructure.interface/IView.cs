using System.Windows.Threading;

namespace Bespoke.Cycling.Windows.Infrastructure
{
    public interface IView
    {
        DispatcherObject View { get; set; }
        bool Confirm(string message);
        bool Alert(string message, AlertImage image);
    }
}
