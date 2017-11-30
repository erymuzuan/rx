using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Management;

namespace Bespoke.Sph.SqlRepository.Management
{
    public class DeploymentMetadataRepository : IDeploymentMetadataRepository
    {
        public async Task InitializeAsync()
        {
            const string SQL = @"
CREATE TABLE [Sph].[DeploymentMetadata](
     [Id] INT PRIMARY KEY NOT NULL IDENTITY(1,1)
    ,[Name] VARCHAR(255)  NULL
    ,[EdId] VARCHAR(255) NOT NULL
    ,[Tag] VARCHAR(255) NOT NULL
    ,[Revision] VARCHAR(255)  NULL
    ,[DateTime] SMALLDATETIME NOT NULL DEFAULT GETDATE()
    ,[Source] VARCHAR(MAX)
)

                ";
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var checkTableCommand = new SqlCommand(@"SELECT COUNT(*)
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Sph' 
                 AND  TABLE_NAME = 'DeploymentMetadata'", conn))
            {
                await conn.OpenAsync();
                var exist = (int)await checkTableCommand.ExecuteScalarAsync() == 1;
                if (!exist)
                {
                    using (var createTableCommand = new SqlCommand(SQL, conn))
                    {
                        await createTableCommand.ExecuteNonQueryAsync();
                    }
                }
            }
        }
    }
}
