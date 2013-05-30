using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.SqlRepository
{
    public class SqlBinaryStore : IBinaryStore
    {
        private readonly string m_connectionString;

        public SqlBinaryStore()
        {
            m_connectionString = ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;
        }

        public SqlBinaryStore(string connectionString)
        {
            m_connectionString = connectionString;
        }

        public void Add(BinaryStore document)
        {
            this.AddAsync(document).Wait(TimeSpan.FromSeconds(5));
        }

       

        public async Task<BinaryStore> GetContentAsync(string stroreid)
        {
            const string sql = "SELECT [StoreId],[Content],[Extension],[FileName] FROM [Sph].[BinaryStore]" +
                               " WHERE [StoreId] =  @StoreId";
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@StoreId", stroreid);

                await conn.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var document = new BinaryStore
                        {
                            Extension = reader.GetString(2),
                            StoreId = reader.GetString(0),
                            Content = (byte[])reader[1],
                            FileName = reader.GetString(3)
                        };
                    return document;
                }


            }

            return null;
        }

        public async Task AddAsync(BinaryStore document)
        {
            const string sql = "INSERT INTO [Sph].[BinaryStore]" +
                               " ([StoreId],[Content],[Extension],[FileName])" +
                               " VALUES" +
                               " (@StoreId, @Content,@Extension, @FileName)";
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@StoreId", document.StoreId);
                cmd.Parameters.AddWithValue("@Content", document.Content);
                cmd.Parameters.AddWithValue("@Extension", document.Extension);
                cmd.Parameters.AddWithValue("@FileName", document.FileName);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

            }
        }

        public async Task DeleteAsync(string storeId)
        {
            const string sql = "DELETE FROM [Sph].[BinaryStore]" +
                               " WHERE [StoreId] =  @StoreId";
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@StoreId", storeId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

            }
        }
    }
}
