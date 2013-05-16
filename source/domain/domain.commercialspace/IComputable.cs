using System.ComponentModel;
using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface IComputable<T> : INotifyPropertyChanged
    {
       Task<T> Compute(string expression);
    }
}