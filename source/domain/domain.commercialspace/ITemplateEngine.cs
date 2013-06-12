using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface ITemplateEngine
    {
        Task<string> GenerateAsync(string template, dynamic model);
    }
}