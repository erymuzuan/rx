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
    public class PatientDepartmentAdapter
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

        public async Task<int> DeleteAsync()
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"DELETE FROM [dbo].[PatientDepartment] WHERE

", conn))
            {
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> InsertAsync(PatientDepartment item)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"INSERT INTO [dbo].[PatientDepartment] ([PatientId],
[DepartmentId]
)
VALUES(
@PatientId,
@DepartmentId
)
", conn))
            {
                cmd.Parameters.AddWithValue("@PatientId", item.PatientId);
                cmd.Parameters.AddWithValue("@DepartmentId", item.DepartmentId);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> UpdateAsync(PatientDepartment item)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"UPDATE  [dbo].[PatientDepartment] SET [PatientId] = @PatientId,
[DepartmentId] = @DepartmentId
 WHERE 

", conn))
            {
                cmd.Parameters.AddWithValue("@PatientId", item.PatientId);
                cmd.Parameters.AddWithValue("@DepartmentId", item.DepartmentId);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<LoadOperation<PatientDepartment>> LoadAsync(string sql, int page = 1, int size = 40, bool includeTotal = false)
        {
            if (!sql.ToString().Contains("ORDER"))
                sql += "\r\nORDER BY [PatientId]";
            var translator = new SqlPagingTranslator();
            sql = translator.Translate(sql, page, size);

            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                var lo = new LoadOperation<PatientDepartment>
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
                        var item = new PatientDepartment();
                        item.PatientId = (int)reader["PatientId"];
                        item.DepartmentId = (int)reader["DepartmentId"];

                        lo.ItemCollection.Add(item);
                    }
                }
                return lo;
            }
        }

        public async Task<PatientDepartment> LoadOneAsync()
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"SELECT * FROM dbo.PatientDepartment WHERE 

", conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new PatientDepartment();
                        item.PatientId = (int)reader["PatientId"];
                        item.DepartmentId = (int)reader["DepartmentId"];

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
