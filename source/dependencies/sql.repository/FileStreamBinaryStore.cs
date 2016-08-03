using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Threading.Tasks;
using System.Transactions;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository
{
    public class FileStreamBinaryStore : IBinaryStore
    {
        private readonly string m_connectionString;

        public FileStreamBinaryStore()
        {
            m_connectionString = ConfigurationManager.SqlConnectionString;
        }

        public FileStreamBinaryStore(string connectionString)
        {
            m_connectionString = connectionString;
        }
        public void Add(BinaryStore document)
        {
            this.AddAsync(document).Wait(TimeSpan.FromSeconds(5));
        }

        public BinaryStore GetContent(string id)
        {
            return this.GetContentAsync(id).Result;
        }

        public async Task<BinaryStore> GetContentAsync(string id)
        {
            var doc = new BinaryStore();

            const string SQL = @"
				SELECT [Extension], [FileName], [Tag], Content.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
					FROM [BinaryStore]
					WHERE [Id] = @id";

            using (var conn = new SqlConnection(m_connectionString))
            {
                using (var cmd = new SqlCommand(SQL, conn))
                {
                    using (var ts = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        string serverPathName;       // string to hold the BLOB pathname
                        byte[] serverTxnContext; // byte array to hold the txn context

                        await conn.OpenAsync();
                        using (var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
                        {
                            if (await rdr.ReadAsync())
                            {
                            doc.Extension = rdr.GetString(0);
                            doc.FileName = rdr.GetString(1);
                            serverPathName = rdr.GetSqlString(3).Value;
                            serverTxnContext = rdr.GetSqlBinary(4).Value;
                            rdr.Close();
                                
                            }
                            else
                            {
                                return null;
                            }
                        }
                        conn.Close();
                        using (var source = new SqlFileStream(serverPathName, serverTxnContext, FileAccess.Read))
                        {
                            using (var dest = new MemoryStream())
                            {
                                await source.CopyToAsync(dest, 4096);
                                dest.Close();
                                doc.Content = dest.ToArray();
                            }
                            source.Close();
                        }

                        ts.Complete();
                    }
                }
            }

            return doc;
        }

        public async Task AddAsync(BinaryStore document)
        {
            const string SQL = @"
				INSERT INTO BinaryStore([Id], [Extension], [FileName], [Content])
					OUTPUT inserted.Content.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
					SELECT @Id, @Extension, @FileName, 0x";

            using (var conn = new SqlConnection(m_connectionString))
            {
                using (var cmd = new SqlCommand(SQL, conn))
                {
                    using (var ts = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
                    {
                        cmd.Parameters.AddWithValue("@Id", document.Id);
                        cmd.Parameters.AddWithValue("@Extension", document.Extension);
                        cmd.Parameters.AddWithValue("@FileName", document.FileName);

                        string serverPathName;
                        byte[] serverTxnContext;
                        await conn.OpenAsync();
                        using (var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
                        {
                            await rdr.ReadAsync();
                            serverPathName = rdr.GetSqlString(0).Value;
                            serverTxnContext = rdr.GetSqlBinary(1).Value;
                            rdr.Close();
                        }
                        conn.Close();

                        using (var source = new MemoryStream(document.Content))
                        {
                            using (var dest = new SqlFileStream(serverPathName, serverTxnContext, FileAccess.Write))
                            {
                                await source.CopyToAsync(dest, 4096);
                                dest.Close();
                            }
                            source.Close();
                        }

                        ts.Complete();
                    }
                }
            }
        }
        private void DeleteSource(string id)
        {
            var folder = $"{ConfigurationManager.SphSourceDirectory}\\BinaryStores";
            string json = $"{folder}\\{id}.json";
            if (File.Exists(json))
                File.Delete(json);

            string path = $"{folder}\\{id}";
            if (Directory.Exists(path))
                Directory.Delete(path, true);

        }

        public async Task DeleteAsync(string storeId)
        {
            DeleteSource(storeId);
            const string SQL = "DELETE FROM [dbo].[BinaryStore]" +
                               " WHERE [Id] =  @Id";
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(SQL, conn))
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
