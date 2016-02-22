using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IReadonlyRepository<T> where T : Entity
    {
        Task<LoadData<T>> LoadOneAsync(string id);
    }

    public class LoadData<T> where T : Entity
    {
        public LoadData(T source, string version)
        {
            Source = source;
            Version = version;
        }

        public T Source { get;  }
        public string Version { get; }
        public string Json { get; set; }
    }
}