using System.ComponentModel;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IComputable<T> : INotifyPropertyChanged
    {
       Task<T> Compute(string expression);
    }
}