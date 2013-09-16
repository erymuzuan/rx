using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IReportDataSource
    {
        Task<ObjectCollection<ReportColumn>> GetColumnsAsync(Type type);
        Task<ObjectCollection<ReportRow>> GetRowsAsync(ReportDefinition rdl);
    }
}