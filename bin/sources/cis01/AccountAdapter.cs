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
using Dev.Adapters.dbo.cis01;

namespace Dev.Adapters.dbo.cis01
{
    public class AccountAdapter
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

        public async Task<int> DeleteAsync(int accountId)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"DELETE FROM [dbo].[Account] WHERE
[AccountId] = @AccountId
", conn))
            {

                cmd.Parameters.AddWithValue("@AccountId", accountId); await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> InsertAsync(Account item)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"INSERT INTO [dbo].[Account] ([Dob],
[AccountNo],
[FirstName],
[LastName],
[Status]
)
VALUES(
@Dob,
@AccountNo,
@FirstName,
@LastName,
@Status
)
", conn))
            {
                cmd.Parameters.AddWithValue("@Dob", item.Dob);
                cmd.Parameters.AddWithValue("@AccountNo", item.AccountNo);
                cmd.Parameters.AddWithValue("@FirstName", item.FirstName);
                cmd.Parameters.AddWithValue("@LastName", item.LastName);
                cmd.Parameters.AddWithValue("@Status", item.Status.ToDbNull());
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> UpdateAsync(Account item)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"UPDATE  [dbo].[Account] SET [Dob] = @Dob,
[AccountNo] = @AccountNo,
[FirstName] = @FirstName,
[LastName] = @LastName,
[Status] = @Status
 WHERE 
[AccountId] = @AccountId
", conn))
            {
                cmd.Parameters.AddWithValue("@Dob", item.Dob);
                cmd.Parameters.AddWithValue("@AccountNo", item.AccountNo);
                cmd.Parameters.AddWithValue("@FirstName", item.FirstName);
                cmd.Parameters.AddWithValue("@LastName", item.LastName);
                cmd.Parameters.AddWithValue("@Status", item.Status.ToDbNull());
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<LoadOperation<Account>> LoadAsync(string sql, int page = 1, int size = 40, bool includeTotal = false)
        {
            if (!sql.ToString().Contains("ORDER"))
                sql += "\r\nORDER BY [AccountId]";
            var translator = new SqlPagingTranslator();
            sql = translator.Translate(sql, page, size);

            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                var lo = new LoadOperation<Account>
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
                        var item = new Account();
                        item.AccountId = (int)reader["AccountId"];
                        item.Dob = (DateTime)reader["Dob"];
                        item.AccountNo = (string)reader["AccountNo"];
                        item.FirstName = (string)reader["FirstName"];
                        item.LastName = (string)reader["LastName"];
                        item.Status = reader["Status"].ReadNullableString();

                        lo.ItemCollection.Add(item);
                    }
                }
                return lo;
            }
        }

        public async Task<Account> LoadOneAsync(int AccountId)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"SELECT * FROM dbo.Account WHERE 
[AccountId] = @AccountId
", conn))
            {
                cmd.Parameters.AddWithValue("@AccountId", AccountId);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new Account();
                        item.AccountId = (int)reader["AccountId"];
                        item.Dob = (DateTime)reader["Dob"];
                        item.AccountNo = (string)reader["AccountNo"];
                        item.FirstName = (string)reader["FirstName"];
                        item.LastName = (string)reader["LastName"];
                        item.Status = reader["Status"].ReadNullableString();

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
                var conn = ConfigurationManager.ConnectionStrings["cis01"];
                if (null != conn) return conn.ConnectionString;
                return @"Data Source=(localdb)\Projects;Initial Catalog=Cis;Integrated Security=True;MultipleActiveResultSets=True";
            }
        }

    }
}
