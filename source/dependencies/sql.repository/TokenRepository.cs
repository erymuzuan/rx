﻿using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.SqlRepository
{
    public class TokenRepository : ITokenRepository
    {
        public async Task SaveAsync(AccessToken token)
        {
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand(@"INSERT INTO [Sph].[AccessToken]
           ([Subject]
           ,[Email]
           ,[UserName]
           ,[Payload]
           ,[ExpiryDate])
     VALUES
           (@Subject
           ,@Email
           ,@UserName
           ,@Payload
           ,@ExpiryDate)", conn))
            {
                cmd.Parameters.AddWithValue("@Subject", token.Subject);
                cmd.Parameters.AddWithValue("@Email", token.Email.ToDbNull());
                cmd.Parameters.AddWithValue("@UserName", token.Username);
                cmd.Parameters.AddWithValue("@Payload", token.ToJson());
                cmd.Parameters.AddWithValue("@ExpiryDate", token.ExpiryDate);
                await conn.OpenAsync();
                var json = await cmd.ExecuteNonQueryAsync();
                System.Diagnostics.Debug.Assert(json == 1, "1 row must be added");
            }
        }

        public Task<LoadOperation<AccessToken>> LoadAsync(string user, DateTime expiry, int page = 1, int size = 20)
        {
            throw new NotImplementedException();
        }

        public async Task<LoadOperation<AccessToken>> LoadAsync(DateTime expiry, int page = 1, int size = 20)
        {
            var lo = new LoadOperation<AccessToken>
            {
                CurrentPage = page,
                PageSize = size
            };
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand($@"SELECT [Payload] FROM [Sph].[AccessToken] WHERE [ExpiryDate] >= @ExpiryDate
ORDER BY [Subject]
OFFSET {(page -1)*size} ROWS FETCH NEXT { size} ROWS ONLY", conn))
            {
                await conn.OpenAsync();
                using (var count =
                    new SqlCommand("SELECT COUNT(*) FROM  [Sph].[AccessToken] WHERE [ExpiryDate] >= @ExpiryDate", conn))
                {
                    count.Parameters.AddWithValue("@ExpiryDate", expiry);
                    lo.TotalRows = (int) await count.ExecuteScalarAsync();
                }
                cmd.Parameters.AddWithValue("@ExpiryDate", expiry);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var token = reader.GetString(0).DeserializeFromJson<AccessToken>();
                        lo.ItemCollection.Add(token);
                    }
                }
            }

            return lo;
        }

        public async Task<AccessToken> LoadOneAsync(string subject)
        {
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand("SELECT [Payload] FROM [Sph].[AccessToken] WHERE [Subject] = @Subject",
                conn))
            {
                cmd.Parameters.AddWithValue("@Subject", subject);
                await conn.OpenAsync();
                var json = await cmd.ExecuteScalarAsync();
                if (json == DBNull.Value) return default;
                if (string.IsNullOrWhiteSpace($"{json}")) return default;

                return ((string) json).DeserializeFromJson<AccessToken>();
            }
        }

        public async Task RemoveAsync(string subject)
        {
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand("DELETE FROM [Sph].[AccessToken] WHERE [Subject] = @Subject", conn))
            {
                cmd.Parameters.AddWithValue("@Subject", subject);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public Task<LoadOperation<AccessToken>> SearchAsync(string query, int page = 1, int size = 20)
        {
            throw new NotImplementedException();
        }
    }
}