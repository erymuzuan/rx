using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository
{
    public partial class SqlRepository<T> : IRepository<T> where T : Entity
    {
        private readonly string m_connectionString;
        public bool IsJson { get; set; }
        public string DataColumn
        {
            get
            {
               return this.IsJson ? "[Json]" : "[Data]";
            }
        }

        public SqlRepository(bool useJson)
        {
            this.IsJson = useJson;
            m_connectionString = ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;
        }
        public SqlRepository()
        {
            m_connectionString = ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;
        }

        public SqlRepository(string connectionString)
        {
            m_connectionString = connectionString;
        }


        public async Task<T> LoadOneAsync(IQueryable<T> query)
        {
            var elementType = typeof(T);
            var sql = query.ToString().Replace("[Data]", string.Format("[{0}Id]," + this.DataColumn, elementType.Name));

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
                        if (this.IsJson)
                        {
                            dynamic t1 = reader.GetString(1).DeserializeFromJson<T>();
                            id.SetValue(t1, reader.GetInt32(0), null);
                            return t1;
                        }
                        var xml = XElement.Parse(reader.GetString(1));
                        dynamic t = xml.DeserializeFromXml(elementType);
                        id.SetValue(t, reader.GetInt32(0), null);
                        return t;
                    }
                }
            }
            return null;
        }

        public T LoadOne(IQueryable<T> query)
        {
            var elementType = typeof(T);
            var sql = query.ToString().Replace("[Data]", string.Format("[{0}Id]," + this.DataColumn, elementType.Name));

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

                        if (this.IsJson)
                        {
                            dynamic t1 = reader.GetString(1).DeserializeFromJson<T>();
                            id.SetValue(t1, reader.GetInt32(0), null);
                            return t1;
                        }

                        var xml = XElement.Parse(reader.GetString(1));
                        dynamic t = xml.DeserializeFromXml(elementType);
                        id.SetValue(t, reader.GetInt32(0), null);
                        return t;
                    }
                }
            }
            return null;
        }

        public async Task<IEnumerable<TResult>> GetListAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
        {
            var column = GetMemberName(selector);
            if (string.IsNullOrWhiteSpace(column)) throw new ArgumentException("Cannot determine the scalar column name");
            var sql = query.ToString().Replace(this.DataColumn, string.Format("[{0}]", column));
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
            var sql = query.ToString().Replace(this.DataColumn, string.Format("[{0}], [{1}]", column, column2));
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
            sql.Replace("[Data]", string.Format("[{0}Id]," + this.DataColumn, elementType.Name));
            if (!sql.ToString().Contains("ORDER"))
            {
                sql.AppendLine();
                sql.AppendFormat("ORDER BY [{0}Id]", elementType.Name);
            }
            var translator = ObjectBuilder.GetObject<IPagingTranslator>();
            sql = new StringBuilder(translator.Tranlate(sql.ToString(), page, size));

            var id = elementType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Single(p => p.Name == elementType.Name + "Id");
            Type specificType = typeof(List<>).MakeGenericType(new[] { elementType });
            dynamic list = Activator.CreateInstance(specificType);

            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql.ToString(), conn))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {

                        if (this.IsJson)
                        {
                            dynamic t1 = reader.GetString(1).DeserializeFromJson<T>();
                            id.SetValue(t1, reader.GetInt32(0), null);
                            list.Add(t1);
                            continue;
                        }

                        var xml = XElement.Parse(reader.GetString(1));
                        dynamic t = xml.DeserializeFromXml(elementType);
                        id.SetValue(t, reader.GetInt32(0));
                        list.Add(t);
                    }
                }
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
