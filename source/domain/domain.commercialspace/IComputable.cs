using System.ComponentModel;
using System.Threading.Tasks;

namespace Bespoke.Station.Domain
{
    public interface IComputable<T> : INotifyPropertyChanged
    {
       Task<T> Compute(string expression);
    }
}