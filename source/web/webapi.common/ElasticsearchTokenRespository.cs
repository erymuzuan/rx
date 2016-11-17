using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;

namespace Bespoke.Sph.WebApi
{
    public class ElasticsearchTokenRespository : ITokenRepository
    {
        private readonly HttpClient m_client;
        public ElasticsearchTokenRespository()
        {
            m_client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) };
        }
        public async Task SaveAsync(AccessToken token)
        {
            var content = new StringContent(token.ToJson());
            var index = ConfigurationManager.ElasticSearchSystemIndex;
            var url = $"{index}/access_token/{token.WebId}";

            var response = await m_client.PostAsync(url, content);
            await this.AddSqlServerAsync(token);
            response.EnsureSuccessStatusCode();
        }

        public Task<LoadOperation<AccessToken>> LoadAsync(string user, DateTime expiry, int page = 1, int size = 20)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var seconds = Math.Round((expiry.ToUniversalTime() - unixEpoch).TotalSeconds);
            var query = $@"
{{
    ""query"": {{
        ""bool"": {{
            ""must"": [
               {{
                  ""term"": {{
                     ""user"": {{
                        ""value"": ""{user}""
                     }}
                  }}
               }},
               {{
                   ""range"": {{
                      ""exp"": {{
                         ""from"": {seconds}
                      }}
                   }}
               }}
            ]
        }}
    }},
    ""from"": {(page - 1) * size},
    ""size"": {size}
}}
";

            return ExecuteSearchAsync(query);
        }

        public async Task<LoadOperation<AccessToken>> LoadAsync(DateTime expiry, int page = 1, int size = 20)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var seconds = Math.Round((expiry.ToUniversalTime() - unixEpoch).TotalSeconds);
            var query = $@"
                {{
                    ""query"": {{
                        ""range"": {{
                           ""exp"": {{ 
                                ""from"":{seconds}
                            }}
                        }}
                    }}, 
                  ""from"": {(page - 1) * size},
                  ""size"": {size}
                }}
            ";

            var lo = await ExecuteSearchAsync(query).ConfigureAwait(false);
            lo.PageSize = size;
            lo.CurrentPage = page;
            return lo;
        }

        private async Task<LoadOperation<AccessToken>> ExecuteSearchAsync(string query)
        {
            var request = new StringContent(query);
            var url = $"{ConfigurationManager.ElasticSearchSystemIndex}/access_token/_search";

            var pr = await Policy.Handle<HttpRequestException>()
                            .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(Math.Pow(2, x) * 500))
                            .ExecuteAndCaptureAsync(async () => await m_client.PostAsync(url, request));

            if (null != pr.FinalException)
                throw pr.FinalException;

            var response = pr.Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content as StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es " + request);
            var json = await content.ReadAsStringAsync();

            var jo = JObject.Parse(json);
            var items = from hit in jo.SelectToken("$.hits.hits")
                        let source = hit.SelectToken("_source")
                        let token = JsonConvert.DeserializeObject<AccessToken>(source.ToString())
                        select token;

            var lo = new LoadOperation<AccessToken>();
            lo.ItemCollection.AddRange(items);
            lo.TotalRows = jo.SelectToken("$.hits.total").Value<int>();
            return lo;
        }

        public async Task<AccessToken> LoadOneAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            var url = $"{ConfigurationManager.ElasticSearchSystemIndex}/access_token/{id}";

            var pr = await Policy.Handle<HttpRequestException>()
                            .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(Math.Pow(2, x) * 500))
                            .ExecuteAndCaptureAsync(async () => await m_client.GetAsync(url));
            if (null != pr.FinalException)
                return null;

            var response = pr.Result;
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // look in sql server
                return await LoadFromSqlServerAsync(id);

            }


            var content = response.Content as StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es ");
            var json = await content.ReadAsStringAsync();

            var jo = JObject.Parse(json);
            var token = jo.SelectToken("$._source").ToString().DeserializeFromJson<AccessToken>();

            return token;
        }

        private async Task<AccessToken> LoadFromSqlServerAsync(string subject)
        {
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand("SELECT [Payload] FROM [Sph].[AccessToken] WHERE [Subject] = @Subject", conn))
            {
                cmd.Parameters.AddWithValue("@Subject", subject);
                await conn.OpenAsync();
                var json = await cmd.ExecuteScalarAsync();
                if (json == DBNull.Value) return default(AccessToken);
                if (string.IsNullOrWhiteSpace($"{json}")) return default(AccessToken);

                return ((string)json).DeserializeFromJson<AccessToken>();

            }
        }
        private async Task RemoveFromSqlServerAsync(string subject)
        {
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand("DELETE FROM [Sph].[AccessToken] WHERE [Subject] = @Subject", conn))
            {
                cmd.Parameters.AddWithValue("@Subject", subject);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
        private async Task AddSqlServerAsync(AccessToken token)
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
                cmd.Parameters.AddWithValue("@Email", token.Email);
                cmd.Parameters.AddWithValue("@UserName", token.Username);
                cmd.Parameters.AddWithValue("@Payload", token.ToJson());
                cmd.Parameters.AddWithValue("@ExpiryDate", token.ExpiryDate);
                await conn.OpenAsync();
                var json = await cmd.ExecuteNonQueryAsync();
                System.Diagnostics.Debug.Assert(json == 1, "1 row must be added");

            }
        }

        public async Task RemoveAsync(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentNullException(nameof(subject));

            var url = $"{ConfigurationManager.ElasticSearchSystemIndex}/access_token/{subject}";
            var pr = await Policy.Handle<HttpRequestException>()
                            .WaitAndRetryAsync(10, x => TimeSpan.FromMilliseconds(Math.Pow(2, x) * 500))
                            .ExecuteAndCaptureAsync(async () => await m_client.DeleteAsync(url));
            if (null != pr.FinalException)
                throw pr.FinalException;

            var response = pr.Result;
            await RemoveFromSqlServerAsync(subject);
            response.EnsureSuccessStatusCode();

        }
    }
}