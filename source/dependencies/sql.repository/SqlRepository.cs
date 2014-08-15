using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
            m_connectionString = ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;
        }

        public SqlRepository(string connectionString)
        {
            m_connectionString = connectionString;
        }

        private string Schema
        {
            get
            {
                var elementType = typeof(T);
                var schema = elementType.Namespace.StartsWith(typeof(Entity).Namespace)
                    ? "Sph"
                    : ConfigurationManager.ApplicationName;

                return schema;
            }
        }

        public async Task<T> LoadOneAsync(int id)
        {
            var elementType = typeof(T);
            var sql = string.Format("SELECT [{0}Id],{1} FROM [{2}].[{0}] WHERE [{0}Id] = @id"
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
                        t1.SetId(id);
                        return t1;
                    }
                }
            }
            return null;
        }

        public async Task<T> LoadOneAsync(IQueryable<T> query)
        {
            var elementType = typeof(T);
            var sql = query.ToString().Replace("[Data]", string.Format("[{0}Id]," + "[Json]", elementType.Name));
            if (elementType.Namespace != typeof(Entity).Namespace)// custom entity
            {
                sql = sql.Replace("[Sph].", string.Format("[{0}].", ConfigurationManager.ApplicationName));
            }


            var id = elementType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Single(p => p.Name == elementType.Name + "Id");


            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {

                        dynamic t1 = reader.GetString(1).DeserializeFromJson<T>();
                        id.SetValue(t1, reader.GetInt32(0), null);
                        return t1;

                    }
                }
            }
            return null;
        }

        public T LoadOne(IQueryable<T> query)
        {
            var elementType = typeof(T);
            var sql = query.ToString().Replace("[Data]", string.Format("[{0}Id], [Json]", elementType.Name));

            var id = elementType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Single(p => p.Name == elementType.Name + "Id");

            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {


                        dynamic t1 = reader.GetString(1).DeserializeFromJson<T>();
                        id.SetValue(t1, reader.GetInt32(0), null);
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
            var sql = query.ToString().Replace("[Json]", string.Format("[{0}]", column));
            var connectionString = ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;
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
            var sql = query.ToString().Replace("[Json]", string.Format("[{0}], [{1}]", column, column2));
            var connectionString = ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;
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
            sql.Replace("[Data]", string.Format("[{0}Id],[Json]", elementType.Name));
            if (!sql.ToString().Contains("ORDER"))
            {
                sql.AppendLine();
                sql.AppendFormat("ORDER BY [{0}Id]", elementType.Name);
            }

            if (!elementType.Namespace.StartsWith(typeof(Entity).Namespace))
                sql.Replace("FROM [Sph].", string.Format("FROM [{0}].", ConfigurationManager.ApplicationName));

            var translator = ObjectBuilder.GetObject<IPagingTranslator>();
            sql = new StringBuilder(translator.Tranlate(sql.ToString(), page, size));

            var id = elementType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Single(p => p.Name == elementType.Name + "Id");
            Type specificType = typeof(List<>).MakeGenericType(new[] { elementType });
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
                            id.SetValue(t1, reader.GetInt32(0), null);
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
                var sql2 = query.ToString().Replace("[Data]", "COUNT(*)");
                var order = sql2.IndexOf("ORDER", StringComparison.Ordinal);
                var count = order == -1 ? sql2 : sql2.Substring(0, order);
                lo.TotalRows = await GetCountAsync(count).ConfigureAwait(false);
            }
            return lo;
        }

    }

}
