using System.Threading.Tasks;

namespace Bespoke.CommercialSpace.Domain
{
    public interface IDocumentExport
    {
        Task GenerateSs3(int year, int month, string output);
    }
}