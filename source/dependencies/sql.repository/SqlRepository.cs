﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository
{
    public partial class SqlRepository<T> : IRepository<T> where T : Entity
    {
        private readonly string m_connectionString;

        public SqlRepository()
        {
            m_connectionString = ConfigurationManager.SqlConnectionString;
        }

        public SqlRepository(string connectionString)
        {
            m_connectionString = connectionString;
        }

        public SqlRepository(string connectionString, bool useEnvironmentVariable)
        {
            m_connectionString = useEnvironmentVariable ? ConfigurationManager.GetEnvironmentVariable(connectionString) : connectionString;
        }

        private string Schema
        {
            get
            {
                var elementType = typeof(T);
                var sph = elementType.Namespace?.StartsWith(typeof(Entity).Namespace ?? "") ?? false;
                var schema = sph
                    ? "Sph"
                    : ConfigurationManager.ApplicationName;

                return schema;
            }
        }

        public async Task<T> LoadOneAsync(string id)
        {
            var elementType = typeof(T);
            var sql = string.Format("SELECT [Id],{1} FROM [{2}].[{0}] WHERE [Id] = @id"
                , elementType.Name
                , "[Json]"
                , this.Schema);



            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                await conn.OpenAsync().ConfigureAwait(false);
                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        dynamic t1 = reader.GetString(1).DeserializeFromJson<T>();
                        t1.Id = id;
                        return t1;
                    }
                }
            }
            return null;
        }

        public async Task<T> LoadOneAsync(IQueryable<T> query)
        {
            var elementType = typeof(T);
            var sql = query.ToString().Replace("[Json]", "[Id], [Json]");
            if (!elementType.Namespace?.StartsWith(typeof(Entity).Namespace ?? "") ?? true)// custom entity
            {
                sql = sql.Replace("[Sph].", $"[{ConfigurationManager.ApplicationName}].");
            }

            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {

                        dynamic t1 = reader.GetString(1).DeserializeFromJson<T>();
                        t1.Id = reader.GetString(0);
                        return t1;

                    }
                }
            }
            return null;
        }

        public T LoadOne(IQueryable<T> query)
        {
            var sql = query.ToString().Replace("[Json]", "[Id], [Json]");

            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dynamic t1 = reader.GetString(1).DeserializeFromJson<T>();
                        t1.Id = reader.GetString(0);
                        return t1;

                    }
                }
            }
            return null;
        }

        public async Task<IEnumerable<TResult>> GetListAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
        {
            var column = GetMemberName(selector);
            if (string.IsNullOrWhiteSpace(column)) throw new ArgumentException("Cannot determine the scalar column name");
            var sql = SanitizeDatabaseSchema(query.ToString().Replace("[Json]", $"[{column}]"));
            var connectionString = ConfigurationManager.SqlConnectionString;
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                var list = new ObjectCollection<TResult>();
                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (reader.Read())
                    {
                        list.Add((TResult)reader[0]);
                    }
                }
                return list;
            }
        }

        public async Task<IEnumerable<Tuple<TResult, TResult2>>> GetList2Async<TResult, TResult2>(IQueryable<T> query, Expression<Func<T, TResult>> selector, Expression<Func<T, TResult2>> selector2)
        {
            var column = GetMemberName(selector);
            var column2 = GetMemberName(selector2);
            if (string.IsNullOrWhiteSpace(column)) throw new ArgumentException("Cannot determine the scalar column name");
            var sql = query.ToString().Replace("[Json]", $"[{column}], [{column2}]");
            var connectionString = ConfigurationManager.SqlConnectionString;
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                var list = new ObjectCollection<Tuple<TResult, TResult2>>();
                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (reader.Read())
                    {
                        var tuple = new Tuple<TResult, TResult2>((TResult)reader[0], (TResult2)reader[1]);
                        list.Add(tuple);
                    }
                }
                return list;
            }
        }


        public async Task<LoadOperation<T>> LoadAsync(IQueryable<T> query, int page, int size, bool includeTotalRows)
        {
            var elementType = typeof(T);
            var sql = new StringBuilder(query.ToString());
            sql.Replace("[Json]", "[Id],[Json]");
            if (!sql.ToString().Contains("ORDER"))
            {
                sql.AppendLine();
                sql.AppendLine("ORDER BY [Id]");
            }

            if (!elementType.Namespace?.StartsWith(typeof(Entity).Namespace ?? "") ?? false)
                sql.Replace("FROM [Sph].", $"FROM [{ConfigurationManager.ApplicationName}].");

            var translator = ObjectBuilder.GetObject<IPagingTranslator>();
            sql = new StringBuilder(translator.Tranlate(sql.ToString(), page, size));

            var specificType = typeof(List<>).MakeGenericType(elementType);
            dynamic list = Activator.CreateInstance(specificType);

            try
            {
                using (var conn = new SqlConnection(m_connectionString))
                using (var cmd = new SqlCommand(sql.ToString(), conn))
                {
                    await conn.OpenAsync().ConfigureAwait(false);
                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            dynamic t1 = reader.GetString(1).DeserializeFromJson<T>();
                            t1.Id = reader.GetString(0);
                            list.Add(t1);

                        }
                    }
                }
            }
            catch (SqlException e)
            {

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(elementType.Name);
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
            var lo = new LoadOperation<T>
            {
                CurrentPage = page,
                Query = query,
                PageSize = size,
                NextSkipToken = (page) * size

            };
            lo.ItemCollection.AddRange(list);

            if (includeTotalRows)
            {
                var sql2 = query.ToString().Replace("[Json]", "COUNT(*)");
                var order = sql2.IndexOf("ORDER", StringComparison.Ordinal);
                var count = order == -1 ? sql2 : sql2.Substring(0, order);
                lo.TotalRows = await GetCountAsync(count).ConfigureAwait(false);
            }
            return lo;
        }

    }

}
