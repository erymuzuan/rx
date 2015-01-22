using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IForm
    {
        string Name { get; }
        string Id { get; }
        FormDesign FormDesign { get; }
        string WebId { get; }
        string Route { get;}
        Task<IProjectProvider> LoadProjectAsync();
    }
}