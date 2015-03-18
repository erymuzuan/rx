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
    public class AddressAdapter
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

        public async Task<int> DeleteAsync(int addressId)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"DELETE FROM [dbo].[Address] WHERE
[AddressId] = @AddressId
", conn))
            {

                cmd.Parameters.AddWithValue("@AddressId", addressId); await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> InsertAsync(Address item)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"INSERT INTO [dbo].[Address] ([AccountId],
[Type],
[Street1],
[Street2],
[Postcode],
[City],
[State],
[Country]
)
VALUES(
@AccountId,
@Type,
@Street1,
@Street2,
@Postcode,
@City,
@State,
@Country
)
", conn))
            {
                cmd.Parameters.AddWithValue("@AccountId", item.AccountId);
                cmd.Parameters.AddWithValue("@Type", item.Type);
                cmd.Parameters.AddWithValue("@Street1", item.Street1.ToDbNull());
                cmd.Parameters.AddWithValue("@Street2", item.Street2.ToDbNull());
                cmd.Parameters.AddWithValue("@Postcode", item.Postcode.ToDbNull());
                cmd.Parameters.AddWithValue("@City", item.City.ToDbNull());
                cmd.Parameters.AddWithValue("@State", item.State.ToDbNull());
                cmd.Parameters.AddWithValue("@Country", item.Country.ToDbNull());
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<object> UpdateAsync(Address item)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"UPDATE  [dbo].[Address] SET [AccountId] = @AccountId,
[Type] = @Type,
[Street1] = @Street1,
[Street2] = @Street2,
[Postcode] = @Postcode,
[City] = @City,
[State] = @State,
[Country] = @Country
 WHERE 
[AddressId] = @AddressId
", conn))
            {
                cmd.Parameters.AddWithValue("@AccountId", item.AccountId);
                cmd.Parameters.AddWithValue("@Type", item.Type);
                cmd.Parameters.AddWithValue("@Street1", item.Street1.ToDbNull());
                cmd.Parameters.AddWithValue("@Street2", item.Street2.ToDbNull());
                cmd.Parameters.AddWithValue("@Postcode", item.Postcode.ToDbNull());
                cmd.Parameters.AddWithValue("@City", item.City.ToDbNull());
                cmd.Parameters.AddWithValue("@State", item.State.ToDbNull());
                cmd.Parameters.AddWithValue("@Country", item.Country.ToDbNull());
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<LoadOperation<Address>> LoadAsync(string sql, int page = 1, int size = 40, bool includeTotal = false)
        {
            if (!sql.ToString().Contains("ORDER"))
                sql += "\r\nORDER BY [AddressId]";
            var translator = new SqlPagingTranslator();
            sql = translator.Translate(sql, page, size);

            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                var lo = new LoadOperation<Address>
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
                        var item = new Address();
                        item.AddressId = (int)reader["AddressId"];
                        item.AccountId = (int)reader["AccountId"];
                        item.Type = (string)reader["Type"];
                        item.Street1 = reader["Street1"].ReadNullableString();
                        item.Street2 = reader["Street2"].ReadNullableString();
                        item.Postcode = reader["Postcode"].ReadNullableString();
                        item.City = reader["City"].ReadNullableString();
                        item.State = reader["State"].ReadNullableString();
                        item.Country = reader["Country"].ReadNullableString();

                        lo.ItemCollection.Add(item);
                    }
                }
                return lo;
            }
        }

        public async Task<Address> LoadOneAsync(int AddressId)
        {
            using (var conn = new SqlConnection(this.ConnectionString))
            using (var cmd = new SqlCommand(@"SELECT * FROM dbo.Address WHERE 
[AddressId] = @AddressId
", conn))
            {
                cmd.Parameters.AddWithValue("@AddressId", AddressId);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new Address();
                        item.AddressId = (int)reader["AddressId"];
                        item.AccountId = (int)reader["AccountId"];
                        item.Type = (string)reader["Type"];
                        item.Street1 = reader["Street1"].ReadNullableString();
                        item.Street2 = reader["Street2"].ReadNullableString();
                        item.Postcode = reader["Postcode"].ReadNullableString();
                        item.City = reader["City"].ReadNullableString();
                        item.State = reader["State"].ReadNullableString();
                        item.Country = reader["Country"].ReadNullableString();

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
