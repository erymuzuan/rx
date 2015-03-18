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
    public class InvoiceAdapter
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

        public async Task<int> DeleteAsync(int invoiceId)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"DELETE FROM [dbo].[Invoice] WHERE
[InvoiceId] = @InvoiceId
", conn))
            {

                cmd.Parameters.AddWithValue("@InvoiceId", invoiceId); await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> InsertAsync(Invoice item)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"INSERT INTO [dbo].[Invoice] ([AccountId],
[Date],
[Amount],
[No]
)
VALUES(
@AccountId,
@Date,
@Amount,
@No
)
", conn))
            {
                cmd.Parameters.AddWithValue("@AccountId", item.AccountId);
                cmd.Parameters.AddWithValue("@Date", item.Date);
                cmd.Parameters.AddWithValue("@Amount", item.Amount);
                cmd.Parameters.AddWithValue("@No", item.No);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> UpdateAsync(Invoice item)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"UPDATE  [dbo].[Invoice] SET [AccountId] = @AccountId,
[Date] = @Date,
[Amount] = @Amount,
[No] = @No
 WHERE 
[InvoiceId] = @InvoiceId
", conn))
            {
                cmd.Parameters.AddWithValue("@AccountId", item.AccountId);
                cmd.Parameters.AddWithValue("@Date", item.Date);
                cmd.Parameters.AddWithValue("@Amount", item.Amount);
                cmd.Parameters.AddWithValue("@No", item.No);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<LoadOperation<Invoice>> LoadAsync(string sql, int page = 1, int size = 40, bool includeTotal = false)
        {
            if (!sql.ToString().Contains("ORDER"))
                sql += "\r\nORDER BY [InvoiceId]";
            var translator = new SqlPagingTranslator();
            sql = translator.Translate(sql, page, size);

            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                var lo = new LoadOperation<Invoice>
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
                        var item = new Invoice();
                        item.InvoiceId = (int)reader["InvoiceId"];
                        item.AccountId = (int)reader["AccountId"];
                        item.Date = (DateTime)reader["Date"];
                        item.Amount = (decimal)reader["Amount"];
                        item.No = (string)reader["No"];

                        lo.ItemCollection.Add(item);
                    }
                }
                return lo;
            }
        }

        public async Task<Invoice> LoadOneAsync(int InvoiceId)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"SELECT * FROM dbo.Invoice WHERE 
[InvoiceId] = @InvoiceId
", conn))
            {
                cmd.Parameters.AddWithValue("@InvoiceId", InvoiceId);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new Invoice();
                        item.InvoiceId = (int)reader["InvoiceId"];
                        item.AccountId = (int)reader["AccountId"];
                        item.Date = (DateTime)reader["Date"];
                        item.Amount = (decimal)reader["Amount"];
                        item.No = (string)reader["No"];

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
