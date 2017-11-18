using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Polly;

namespace Bespoke.Sph.SqlRepository
{
    public class CancelledMessageRepository : ICancelledMessageRepository
    {
        private readonly string m_connectionString;
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global


        public int RetryCount { get; set; } = 3;
        public TimeSpan WaitDuration { get; set; } = TimeSpan.FromMilliseconds(200);
        public TimeSpan CheckMessageBreakDuration { get; set; } = TimeSpan.FromSeconds(30);
        public int CheckMessageAllowedExceptionsBeforeBreaking { get; set; } = 3;

        public WaitAlgorithm WaitAlgorithm { get; set; } = WaitAlgorithm.Exponential;
        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global

        public CancelledMessageRepository()
        {
            m_connectionString = ConfigurationManager.SqlConnectionString;
        }

        public CancelledMessageRepository(string connectionString, bool useEnvironmentVariable = false)
        {
            m_connectionString = useEnvironmentVariable ? ConfigurationManager.GetEnvironmentVariable(connectionString) : connectionString;
        }

        public async Task<bool> CheckMessageAsync(string messageId, string worker)
        {
            var pr = await Policy.Handle<SqlException>()
                .WaitAndRetryAsync(RetryCount, Wait)
                .ExecuteAndCaptureAsync(async () =>
                {
                    using (var conn = new SqlConnection(m_connectionString))
                    using (var cmd = new SqlCommand($"[{Schema}].[CheckCancelledMessage]", conn){CommandType = CommandType.StoredProcedure})
                    {
                        cmd.Parameters.Add(new SqlParameter("@MessageId", SqlDbType.VarChar, 255)).Value = messageId;
                        cmd.Parameters.Add(new SqlParameter("@Worker", SqlDbType.VarChar, 255)).Value = worker;

                        await conn.OpenAsync();
                        var total = (int)await cmd.ExecuteScalarAsync();
                        return total > 0;
                    }

                });
            return pr.Result;
        }

        public string Schema { get; set; } = "Sph";

        TimeSpan Wait(int c)
        {
            switch (WaitAlgorithm)
            {
                case WaitAlgorithm.Linear:
                    return TimeSpan.FromMilliseconds(this.WaitDuration.TotalMilliseconds * c);
                case WaitAlgorithm.Exponential:
                    return TimeSpan.FromMilliseconds(this.WaitDuration.TotalMilliseconds * Math.Pow(2, c));
                case WaitAlgorithm.Constant:
                    return this.WaitDuration;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public async Task PutAsync(string messageId, string worker)
        {

            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(RetryCount, Wait)
                .ExecuteAsync(async () =>
                {
                    using (var conn = new SqlConnection(m_connectionString))
                    using (var cmd = new SqlCommand($"[{Schema}].[PutCancelledMessage]", conn) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@MessageId", SqlDbType.VarChar, 255)).Value = messageId;
                        cmd.Parameters.Add(new SqlParameter("@Worker", SqlDbType.VarChar, 255)).Value = worker;
                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                });
        }

        public async Task RemoveAsync(string messageId, string worker)
        {
            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(RetryCount, Wait)
                .ExecuteAsync(async () =>
                {
                    using (var conn = new SqlConnection(m_connectionString))
                    using (var cmd = new SqlCommand($"[{Schema}].[RemoveCancelledMessage]", conn) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@MessageId", SqlDbType.VarChar, 255)).Value = messageId;
                        cmd.Parameters.Add(new SqlParameter("@Worker", SqlDbType.VarChar, 255)).Value = worker;
                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                });
        }

    }
}
