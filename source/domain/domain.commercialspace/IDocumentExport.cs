using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface IDocumentExport
    {
        Task GenerateSs3(int year, int month, string output);
    }
}