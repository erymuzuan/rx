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
using DevV1.Adapters.dbo.His;

namespace DevV1.Adapters.dbo.His
{
    public class PatientAdapter
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
            using (var cmd = new SqlCommand(@"DELETE FROM [dbo].[Patient] WHERE
[Id] = @Id
", conn))
            {

                cmd.Parameters.AddWithValue("@Id", id); await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> InsertAsync(Patient item)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"INSERT INTO [dbo].[Patient] ([Dob],
[Income],
[Name],
[Mrn],
[Gender]
)
VALUES(
@Dob,
@Income,
@Name,
@Mrn,
@Gender
)
", conn))
            {
                cmd.Parameters.AddWithValue("@Dob", item.Dob);
                cmd.Parameters.AddWithValue("@Income", item.Income);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Mrn", item.Mrn);
                cmd.Parameters.AddWithValue("@Gender", item.Gender);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> UpdateAsync(Patient item)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"UPDATE  [dbo].[Patient] SET [Dob] = @Dob,
[Income] = @Income,
[Name] = @Name,
[Mrn] = @Mrn,
[Gender] = @Gender
 WHERE 
[Id] = @Id
", conn))
            {
                cmd.Parameters.AddWithValue("@Dob", item.Dob);
                cmd.Parameters.AddWithValue("@Income", item.Income);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Mrn", item.Mrn);
                cmd.Parameters.AddWithValue("@Gender", item.Gender);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<LoadOperation<Patient>> LoadAsync(string sql, int page = 1, int size = 40, bool includeTotal = false)
        {
            if (!sql.ToString().Contains("ORDER"))
                sql += "\r\nORDER BY [Id]";
            var translator = new SqlPagingTranslator();
            sql = translator.Translate(sql, page, size);

            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                var lo = new LoadOperation<Patient>
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
                        var item = new Patient();
                        item.Id = (int)reader["Id"];
                        item.Dob = (DateTime)reader["Dob"];
                        item.Income = (decimal)reader["Income"];
                        item.Name = (string)reader["Name"];
                        item.Mrn = (string)reader["Mrn"];
                        item.Gender = (string)reader["Gender"];

                        lo.ItemCollection.Add(item);
                    }
                }
                return lo;
            }
        }

        public async Task<Patient> LoadOneAsync(int Id)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"SELECT * FROM dbo.Patient WHERE 
[Id] = @Id
", conn))
            {
                cmd.Parameters.AddWithValue("@Id", Id);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new Patient();
                        item.Id = (int)reader["Id"];
                        item.Dob = (DateTime)reader["Dob"];
                        item.Income = (decimal)reader["Income"];
                        item.Name = (string)reader["Name"];
                        item.Mrn = (string)reader["Mrn"];
                        item.Gender = (string)reader["Gender"];

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
                var conn = ConfigurationManager.ConnectionStrings["His"];
                if (null != conn) return conn.ConnectionString;
                return @"Data Source=(localdb)\ProjectsV12;Initial Catalog=His;Integrated Security=True;MultipleActiveResultSets=True";
            }
        }

    }
}
