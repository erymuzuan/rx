using System.Windows.Threading;

namespace Bespoke.Sph.Mangements
{
    public interface IView
    {
        DispatcherObject View { get; set; }
    }
}