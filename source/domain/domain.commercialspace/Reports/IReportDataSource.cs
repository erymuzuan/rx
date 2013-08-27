using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface IReportDataSource
    {
        Task<ObjectCollection<ReportColumn>> GetColumnsAsync(DataSource dataSource);
        Task<ObjectCollection<ReportRow>> GetRowsAsync(DataSource dataSource);
    }
}