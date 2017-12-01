using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Management;

namespace Bespoke.Sph.SqlRepository.Management
{
    [Export(typeof(IRepositoryManagement))]
    public class RepositoryManagement : SqlServerManagement, IRepositoryManagement
    {
        public async Task TruncateDataAsync(EntityDefinition ed)
        {
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var truncateCommand = new SqlCommand($"TRUNCATE TABLE [{ConfigurationManager.ApplicationName}].[{ed.Name}]", conn))
            using (var dbccCommand = new SqlCommand($"DBCC SHRINKDATABASE ({ConfigurationManager.ApplicationName})", conn))
            {
                await conn.OpenAsync();

                await truncateCommand.ExecuteNonQueryAsync();
                await dbccCommand.ExecuteNonQueryAsync();
            }
        }
    }
}