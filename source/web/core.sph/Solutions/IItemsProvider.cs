using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.Sph.Web.Solutions
{
    public interface IItemsProvider
    {
        Task<SolutionItem> GetItemAsync();
        Task<IEnumerable<SolutionItem>> GetItemsAsync(SolutionItem parent);
    }
}