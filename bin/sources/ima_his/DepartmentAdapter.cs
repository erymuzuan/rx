using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Serialization;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using DevV1.Adapters.dbo.ima_his;

namespace DevV1.Adapters.dbo.ima_his
{
    public class DepartmentAdapter
    {
        public async Task<T> ExecuteScalarAsync<T>(string sql)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();
                var dbval = await cmd.ExecuteScalarAsync();
                if (dbval == System.DBNull.Value)
                    return default(T);
                return (T)dbval;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"DELETE FROM [dbo].[Department] WHERE
[Id] = @Id
", conn))
            {

                cmd.Parameters.AddWithValue("@Id", id); await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> InsertAsync(Department item)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"INSERT INTO [dbo].[Department] ([Name]
)
VALUES(
@Name
)
", conn))
            {
                cmd.Parameters.AddWithValue("@Name", item.Name);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> UpdateAsync(Department item)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"UPDATE  [dbo].[Department] SET [Name] = @Name
 WHERE 
[Id] = @Id
", conn))
            {
                cmd.Parameters.AddWithValue("@Name", item.Name);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<LoadOperation<Department>> LoadAsync(string sql, int page = 1, int size = 40, bool includeTotal = false)
        {
            if (!sql.ToString().Contains("ORDER"))
                sql += "\r\nORDER BY [Id]";
            var translator = new SqlPagingTranslator();
            sql = translator.Translate(sql, page, size);

            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                var lo = new LoadOperation<Department>
                {
                    CurrentPage = page,
                    Filter = sql,
                    PageSize = size,
                };
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new Department();
                        item.Id = (int)reader["Id"];
                        item.Name = (string)reader["Name"];

                        lo.ItemCollection.Add(item);
                    }
                }
                return lo;
            }
        }

        public async Task<Department> LoadOneAsync(int Id)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"SELECT * FROM dbo.Department WHERE 
[Id] = @Id
", conn))
            {
                cmd.Parameters.AddWithValue("@Id", Id);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new Department();
                        item.Id = (int)reader["Id"];
                        item.Name = (string)reader["Name"];

                        return item;
                    }
                }
                return null;
            }
        }

        public string ConnectionString
        {
            get
            {
                var conn = ConfigurationManager.ConnectionStrings["ima_his"];
                if (null != conn) return conn.ConnectionString;
                return @"Data Source=(localdb)\ProjectsV12;Initial Catalog=His;Integrated Security=True;MultipleActiveResultSets=True";
            }
        }

    }
}
