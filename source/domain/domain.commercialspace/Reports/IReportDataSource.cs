using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface IReportDataSource
    {
        Task<ObjectCollection<ReportColumn>> GetColumnsAsync(ReportDefinition rdl);
        Task<ObjectCollection<ReportRow>> GetRowsAsync(ReportDefinition rdl);
    }
}