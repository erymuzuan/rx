using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface ITemplateEngine
    {
        Task<string> GenerateAsync(string template, dynamic model);
    }
}