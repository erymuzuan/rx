using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

using ConfigurationManager = System.Configuration.ConfigurationManager;
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

       

        public BinaryStore GetContent(string id)
        {
            const string sql = "SELECT [Id],[Content],[Extension],[FileName] FROM [Sph].[BinaryStore]" +
                               " WHERE [Id] =  @Id";
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var document = new BinaryStore
                        {
                            Extension = reader.GetString(2),
                            Id = reader.GetString(0),
                            Content = (byte[])reader[1],
                            FileName = reader.GetString(3)
                        };
                    return document;
                }


            }

            return null;
        }
        public async Task<BinaryStore> GetContentAsync(string id)
        {
            const string sql = "SELECT [Id],[Content],[Extension],[FileName] FROM [Sph].[BinaryStore]" +
                               " WHERE [Id] =  @Id";
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var document = new BinaryStore
                        {
                            Extension = reader.GetString(2),
                            Id = reader.GetString(0),
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
                               " ([Id],[Content],[Extension],[FileName])" +
                               " VALUES" +
                               " (@Id, @Content,@Extension, @FileName)";
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", document.Id);
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
                               " WHERE [Id] =  @Id";
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", storeId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

            }
        }
    }
}
