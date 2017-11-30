using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;
using System.Linq;
using Bespoke.Sph.SqlRepository.Extensions;

namespace Bespoke.Sph.SqlRepository
{
    public class LoggerRepository : ILoggerRepository
    {
        private string ConnectionString { get; }
        private ILogger Logger { get; }
        private readonly string m_table = $"[Sph].[Log]";

        public LoggerRepository(string connectionString, bool fromEnvironmentVariable = true,
            ILogger logger = null)
        {
            Logger = logger ?? ObjectBuilder.GetObject<ILogger>();
            ConnectionString = fromEnvironmentVariable
                ? ConfigurationManager.GetEnvironmentVariable(connectionString)
                : connectionString;
        }


        public async Task<LogEntry> LoadOneAsync(string id)
        {    using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand($"SELECT [Json] FROM [{m_table}] WHERE [Id] = @Id",
                conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await conn.OpenAsync();
                var json = await cmd.ExecuteScalarAsync();
                if (json == DBNull.Value) return default;
                if (string.IsNullOrWhiteSpace($"{json}")) return default;
                var src = json.ToString().DeserializeFromJson<LogEntry>();
                return src ;
            }
        }

        public async Task<LoadOperation<LogEntry>> SearchAsync(QueryDsl query)
        {
            var logSchema = new EntityDefinition {Name = "LogEntry", Plural = "LogEntries", Id = "log-entry"};
            var sql = query.CompileToSql();
            Logger.WriteDebug($"QueryDsl: {query}\r\nSQL: \r\n{sql}");

            var lo = new LoadOperation<LogEntry>(query);
            using (var conn = new SqlConnection(this.ConnectionString))
            {
                await conn.OpenAsync();
                var countSqlText = query.CompileToSqlCount<LogEntry>();
                Logger.WriteDebug($"Count SQL : {countSqlText}\r\n=========");
                using (var count = new SqlCommand(countSqlText, conn))
                {
                    lo.TotalRows = (int) await count.ExecuteScalarAsync();
                }

                if (query.Aggregates.Any())
                {
                    var aggregateSqlText = query.CompileToSql<LogEntry>();
                    Logger.WriteDebug($"Aggregate SQL : \r\n{aggregateSqlText}\r\n=================");
                    using (var aggregateCommand = new SqlCommand(aggregateSqlText, conn))
                    {
                        foreach (var agg in query.Aggregates)
                        {
                            await agg.ReadAsync<LogEntry>(aggregateCommand);
                        }
                    }
                }

                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (query.Fields.Any())
                        {
                            while (await reader.ReadAsync())
                            {
                                var row = new Dictionary<string, object>();
                                foreach (var field in query.Fields)
                                {
                                    row.Add(field, reader.GetValue(field, logSchema));
                                }
                                row.Add("Id", reader.GetValue("Id", logSchema));
                                lo.Readers.Add(row);
                            }
                        }
                        else
                        {
                            while (await reader.ReadAsync())
                            {
                                var item = reader["Json"].ReadNullableString().DeserializeFromJson<LogEntry>();
                                item.Id = reader["Id"].ReadNullableString();
                                lo.ItemCollection.Add(item);
                            }
                        }
                    }
                }
            }

            return lo;
        }
    }
}