using System.Threading.Tasks;

namespace Bespoke.CommercialSpace.Domain
{
    public interface IBinaryStore
    {
       void Add(BinaryStore document);
       Task<BinaryStore> GetContent(string stroreid);
    }
}