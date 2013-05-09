using System.ComponentModel;
using System.Threading.Tasks;

namespace Bespoke.CommercialSpace.Domain
{
    public interface IComputable<T> : INotifyPropertyChanged
    {
       Task<T> Compute(string expression);
    }
}