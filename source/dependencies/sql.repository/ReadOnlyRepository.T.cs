using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
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
        public ReadOnlyRepository() : this(ConfigurationManager.SqlConnectionString, false)
        {

        }
        public ReadOnlyRepository(string connectionString, bool readFromEnvironmentVariable = true, EntityDefinition entityDefinition = null)
        {
            ConnectionString = readFromEnvironmentVariable ? ConfigurationManager.GetEnvironmentVariable(connectionString) : connectionString;
            if (null == entityDefinition)
            {
                var context = new SphDataContext();
                this.EntityDefinition = context.LoadOneFromSources<EntityDefinition>(x => x.Name == typeof(T).Name);
            }
            else
            {
                EntityDefinition = entityDefinition;
            }
        }
        public Task<int> GetCountAsync(Filter[] filters)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMaxAsync<TResult>(QueryDsl queryDsl)
        {
            throw new NotImplementedException();
        }

        public Task<LoadData<T>> LoadOneAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<LoadData<T>> LoadOneAsync(string field, string value)
        {
            throw new NotImplementedException();
        }

        public async Task<LoadOperation<T>> SearchAsync(QueryDsl queryDsl)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();

            logger.WriteDebug($"SearchAsync for {typeof(T).Name}");
            var sql = queryDsl.CompileToSql<T>();
            logger.WriteDebug($"QueryDsl: {queryDsl}\r\nSQL: \r\n{sql}");

            var lo = new LoadOperation<T>(queryDsl);
            using (var conn = new SqlConnection(this.ConnectionString))
            {
                await conn.OpenAsync();
                using (var count = new SqlCommand(queryDsl.CompileToSqlCount<T>(), conn))
                {
                    lo.TotalRows = (int)await count.ExecuteScalarAsync();
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
                                row.Add("Id", reader.GetValue("Id",this.EntityDefinition ));
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
