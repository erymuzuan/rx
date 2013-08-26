using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class ReportDefinition : Entity
    {
        public async Task<ObjectCollection<ReportRow>> ExecuteResultAsync()
        {
            var repository = ObjectBuilder.GetObject<IReportDataSource>();
            var columns = await repository.GetColumnsAsync(this.DataSource);

            var rows = await repository.GetRowsAsync(this.DataSource);
            return rows;
        }
    }

    public interface IReportDataSource
    {
        Task<ObjectCollection<ReportColumn>> GetColumnsAsync(DataSource dataSource);
        Task<ObjectCollection<ReportRow>> GetRowsAsync(DataSource dataSource);
    }

    public class ReportColumn
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", this.Value);
        }
    }

    public class ReportRow
    {
        private readonly ObjectCollection<ReportColumn> m_columnCollection = new ObjectCollection<ReportColumn>();
        public ObjectCollection<ReportColumn> ColumnCollection
        {
            get { return m_columnCollection; }
        }
        public ReportColumn this[string name]
        {
            get
            {
                var col = this.ColumnCollection.SingleOrDefault(c => c.Name == name);
                return col;
            }
        }
    }
}
