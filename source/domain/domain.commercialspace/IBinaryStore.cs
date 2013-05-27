using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface IBinaryStore
    {
        void Add(BinaryStore document);
        Task<BinaryStore> GetContentAsync(string stroreId);
        Task AddAsync(BinaryStore document);
        Task DeleteAsync(string storeId);
    }
}