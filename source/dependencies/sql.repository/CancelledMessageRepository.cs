using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository
{
    public class CancelledMessageRepository : ICancelledMessageRepository
    {
        public string ConnectionString { get; }

        public CancelledMessageRepository()
        {
            ConnectionString = ConfigurationManager.GetEnvironmentVariable("CancelledMessageSqlConnectionString") ?? ConfigurationManager.SqlConnectionString;
        }
       
        public async Task<bool> CheckMessageAsync(string messageId, string worker)
        {
            const string SQL = @"SELECT COUNT(*) FROM [Sph].[CancelledMessage] WHERE [MessageId] = @messageId AND [Worker] = @worker";
            return await SQL.GetNullableScalarValueAsync<int>(
                       ConnectionString,
                       new SqlParameter("@messageId", SqlDbType.VarChar, 50) { Value = messageId },
                       new SqlParameter("@worker", SqlDbType.VarChar, 500) { Value = worker })
                  > 0;
        }

        public Task PutAsync(string messageId, string worker)
        {
            return ConnectionString.ExecuteNonQueryAsync(
                       @"INSERT INTO [Sph].[CancelledMessage] ([MessageId],  [Worker]) VALUE(@messageId ,@worker )",
                       new SqlParameter("@messageId", SqlDbType.VarChar, 50) { Value = messageId },
                       new SqlParameter("@worker", SqlDbType.VarChar, 500) { Value = worker });
        }

        public Task RemoveAsync(string messageId, string worker)
        {
            return ConnectionString.ExecuteNonQueryAsync(
                @"DELETE FROM [Sph].[CancelledMessage] WHERE [MessageId] = @messageId AND [Worker] = @worker",
                new SqlParameter("@messageId", SqlDbType.VarChar, 50) { Value = messageId },
                new SqlParameter("@worker", SqlDbType.VarChar, 500) { Value = worker });
        }
    }
}
