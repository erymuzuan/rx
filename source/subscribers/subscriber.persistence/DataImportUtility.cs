using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Polly;

namespace Bespoke.Sph.Persistence
{
    internal static class DataImportUtility
    {
        public static async Task<bool> InsertImportDataAsync(Entity[] entities, int retry, int wait, Action<string> writeMessage)
        {
            var persistence = ObjectBuilder.GetObject<IPersistence>();
            try
            {
                var policy = Policy.Handle<SqlException>(ex => ex.Message.Contains("deadlocked"))
                    .WaitAndRetryAsync(retry, c => TimeSpan.FromMilliseconds(wait * c),
                        (ex, ts) =>
                        {
                            writeMessage($"Waiting for retry in {ts.Seconds} seconds : \r\n{ex.Message}");
                        })
                    .ExecuteAsync(() => persistence.BulkInsertAsync(entities))
                    .ConfigureAwait(false);
                await policy;
                return true;
            }
            catch (Exception e)
            {
                ObjectBuilder.GetObject<ILogger>().Log(new LogEntry(e));
            }
            return false;
        }
    }
}