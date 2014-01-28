using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IReportDataSource
    {
        Task<ObjectCollection<ReportColumn>> GetColumnsAsync(string type);
        Task<ObjectCollection<ReportRow>> GetRowsAsync(ReportDefinition rdl);
    }
}