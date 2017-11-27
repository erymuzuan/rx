using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Bespoke.Sph.Extensions;

namespace Bespoke.Sph.SqlRepository
{
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : Entity, new()
    {
        public string ConnectionString { get; }
        public EntityDefinition EntityDefinition { get; }
        private readonly string m_table = $"{ConfigurationManager.ApplicationName}].[{typeof(T).Name}";
        private readonly ILogger m_logger;
        private ILogger Logger => m_logger ?? ObjectBuilder.GetObject<ILogger>();

        public ReadOnlyRepository() : this(ConfigurationManager.SqlConnectionString, false)
        {
        }

        public ReadOnlyRepository(string connectionString, bool readFromEnvironmentVariable = true,
            EntityDefinition entityDefinition = null,
            ILogger logger = null)
        {
            ConnectionString = readFromEnvironmentVariable
                ? ConfigurationManager.GetEnvironmentVariable(connectionString)
                : connectionString;
            if (null == entityDefinition)
            {
                var context = new SphDataContext();
                this.EntityDefinition = context.LoadOneFromSources<EntityDefinition>(x => x.Name == typeof(T).Name);
            }
            else
            {
                EntityDefinition = entityDefinition;
            }
            
            m_logger = logger;

        }

        public Task<int> GetCountAsync(Filter[] filters)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException("");
        }

        public async Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector)
        {
            var @where = new PredicateExpressionVisitor<T>(Logger).Visit(predicate);
            dynamic sel = selector.Body;
            string field = sel.Member.Name;
            using (var conn = new SqlConnection(ConnectionString))
            {
                var sql = $"SELECT MAX([{field}]) FROM [{m_table}] {@where}";
                Logger.WriteDebug(sql);
                using (var cmd = new SqlCommand(sql,
                    conn))
                {
                    await conn.OpenAsync();
                    var scalar = await cmd.ExecuteScalarAsync();
                    if (scalar == DBNull.Value) return default;
                
                    return (TResult)scalar;
                }
            }
        }

        public Task<TResult> GetMaxAsync<TResult>(QueryDsl queryDsl)
        {
            throw new NotImplementedException("GetMaxAsync<TResult>(QueryDsl queryDsl)");
        }
        static string GetMd5Hash(string input)
        {
            using (var md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                return data.ToString("", x => x.ToString("x2"));

            }
        }

        public async Task<LoadData<T>> LoadOneAsync(string id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand($"SELECT [Json] FROM [{m_table}] WHERE [Id] = @Id",
                conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await conn.OpenAsync();
                var json = await cmd.ExecuteScalarAsync();
                if (json == DBNull.Value) return default;
                if (string.IsNullOrWhiteSpace($"{json}")) return default;
                var src = json.ToString().DeserializeFromJson<T>();
                var lo = new LoadData<T>(src, GetMd5Hash($"{src.ChangedDate:O}"));
                return lo;
            }
        }

        public async Task<LoadData<T>> LoadOneAsync(string field, string value)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand($"SELECT [Json] FROM [{m_table}] WHERE [{field}] = @FieldValue",
                conn))
            {
                cmd.Parameters.AddWithValue("@FieldValue", value);
                await conn.OpenAsync();
                var json = await cmd.ExecuteScalarAsync();
                if (json == DBNull.Value) return default;
                if (string.IsNullOrWhiteSpace($"{json}")) return default;
                var src = json.ToString().DeserializeFromJson<T>();
                var lo = new LoadData<T>(src, GetMd5Hash($"{src.ChangedDate:O}"));
                return lo;
            }
        }

        public async Task<LoadOperation<T>> SearchAsync(QueryDsl queryDsl)
        {
            Logger.WriteDebug($"SearchAsync for {typeof(T).Name}");


            var querySansAggregates = queryDsl.Clone();
            querySansAggregates.Aggregates.Clear();

            var sql = querySansAggregates.CompileToSql<T>();
            Logger.WriteDebug($"QueryDsl: {queryDsl}\r\nSQL: \r\n{sql}");

            var lo = new LoadOperation<T>(queryDsl);
            using (var conn = new SqlConnection(this.ConnectionString))
            {
                await conn.OpenAsync();
                var countSqlText = queryDsl.CompileToSqlCount<T>();
                Logger.WriteDebug($"Count SQL : {countSqlText}\r\n=========");
                using (var count = new SqlCommand(countSqlText, conn))
                {
                    lo.TotalRows = (int) await count.ExecuteScalarAsync();
                }

                if (queryDsl.Aggregates.Any())
                {
                    var aggregateSqlText = queryDsl.CompileToSql<T>();
                    Logger.WriteDebug($"Aggregate SQL : \r\n{aggregateSqlText}\r\n=================");
                    using (var aggregateCommand = new SqlCommand(aggregateSqlText, conn))
                    {
                        foreach (var agg in queryDsl.Aggregates)
                        {
                            await agg.ReadAsync<T>(aggregateCommand);
                        }
                    }
                }

                using (var cmd = new SqlCommand(sql, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                    {
                        if (queryDsl.Fields.Any())
                        {
                            while (await reader.ReadAsync())
                            {
                                var row = new Dictionary<string, object>();
                                foreach (var field in queryDsl.Fields)
                                {
                                    row.Add(field, reader.GetValue(field, this.EntityDefinition));
                                }
                                row.Add("Id", reader.GetValue("Id", this.EntityDefinition));
                                lo.Readers.Add(row);
                            }
                        }
                        else
                        {
                            while (await reader.ReadAsync())
                            {
                                var item = reader["Json"].ReadNullableString().DeserializeFromJson<T>();
                                item.Id = reader["Id"].ReadNullableString();
                                lo.ItemCollection.Add(item);
                            }
                        }
                    }
                }
            }

            return lo;
        }

        public Task<LoadOperation<T>> SearchAsync(string odataUri)
        {
            throw new NotImplementedException();
        }
    }
}