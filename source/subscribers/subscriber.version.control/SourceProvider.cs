using System.Threading.Tasks;

namespace subscriber.version.control
{
    public abstract class SourceProvider<T>
    {
        public abstract Task ProcessItem(T item);
    }
}