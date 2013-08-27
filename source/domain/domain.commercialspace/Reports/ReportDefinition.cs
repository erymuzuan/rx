using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class ReportDefinition : Entity
    {
        public async Task<ObjectCollection<ReportColumn>> GetAvailableColumnsAsync()
        {
            var repository = ObjectBuilder.GetObject<IReportDataSource>();
            var columns = await repository.GetColumnsAsync(this);

            return columns;
        }

        public async Task<ObjectCollection<ReportRow>> ExecuteResultAsync()
        {
            var repository = ObjectBuilder.GetObject<IReportDataSource>();

            var rows = await repository.GetRowsAsync(this);
            return rows;
        }
    }
}
