using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class ReportDefinition : Entity
    {
        public async Task<ObjectCollection<ReportColumn>> GetAvailableColumnsAsync()
        {
            var repository = ObjectBuilder.GetObject<IReportDataSource>();
            var columns = await repository.GetColumnsAsync(this.DataSource);

            return columns;
        }

        public async Task<ObjectCollection<ReportRow>> ExecuteResultAsync()
        {
            var repository = ObjectBuilder.GetObject<IReportDataSource>();

            var rows = await repository.GetRowsAsync(this.DataSource);
            return rows;
        }
    }

    public interface IReportDataSource
    {
        Task<ObjectCollection<ReportColumn>> GetColumnsAsync(DataSource dataSource);
        Task<ObjectCollection<ReportRow>> GetRowsAsync(DataSource dataSource);
    }

    public partial class ReportColumn : DomainObject
    {
        public override string ToString()
        {
            return string.Format("{0}", this.Value);
        }
    }

    public partial class ReportRow : DomainObject
    {
       
        public ReportColumn this[string name]
        {
            get
            {
                var col = this.ReportColumnCollection.SingleOrDefault(c => c.Name == name);
                return col;
            }
        }
    }
}
