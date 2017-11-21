using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.CosmosDbRepository.Extensions;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.CosmosDbRepository
{
    public class TokenRepository : ITokenRepository, IDisposable
    {
        private readonly string m_databaseId;
        private readonly string m_collectionId;
        private readonly DocumentClient m_client;
        public int OfferThroughput { get; set; } = 400;

        public TokenRepository() : this("https://localhost:8081", "tokens", "access_token", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")
        {

        }

        public void Initialize()
        {
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();

            // TODO : initialize indices
            var collection = new DocumentCollection
            {
                Id = m_collectionId,
                IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 })
                {
                    IndexingMode = IndexingMode.Consistent
                }
            };


            m_client.CreateDocumentCollectionAsync(UriFactory.CreateDatabaseUri(m_databaseId), collection)
                .Wait();
        }



        public TokenRepository(string uri, string databaseId, string collectionId, string key)
        {
            m_databaseId = databaseId;
            m_collectionId = collectionId;

            m_client = new DocumentClient(new Uri(uri), key);

        }
        public async Task SaveAsync(AccessToken token)
        {
            var json = JsonConvert.SerializeObject(token);
            await m_client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(m_databaseId, m_collectionId), JObject.Parse(json).CreateIdNodeWith());
        }

        public async Task<LoadOperation<AccessToken>> LoadAsync(string user, DateTime expiry, int page = 1, int size = 20)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(m_databaseId, m_collectionId);
            var query = m_client.CreateDocumentQuery<AccessToken>(uri, new FeedOptions { MaxItemCount = 20 })
                //.Skip((page - 1) * size)
                //.Take(size)
                .Where(x => x.Username == user)
                .Where(x => x.ExpiryDate >= expiry)
                .AsDocumentQuery();

            var results = new List<AccessToken>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<AccessToken>());
            }

            var lo = new LoadOperation<AccessToken>
            {
                CurrentPage = page,
                PageSize = size,
                TotalRows = results.Count
            };
            lo.ItemCollection.AddRange(results);

            return lo;
        }

        public async Task<LoadOperation<AccessToken>> LoadAsync(DateTime expiry, int page = 1, int size = 20)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(m_databaseId, m_collectionId);
            var query = m_client.CreateDocumentQuery<AccessToken>(uri, new FeedOptions { MaxItemCount = 20 })
                    // .Skip((page - 1) * size)
                    //.Take(size)
                    //.Where(x => x.ExpiryDate >= expiry)
                    .AsDocumentQuery();

            var results = new List<AccessToken>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<AccessToken>());
            }

            var lo = new LoadOperation<AccessToken>
            {
                CurrentPage = page,
                PageSize = size,
                TotalRows = results.Count
            };
            lo.ItemCollection.AddRange(results);

            return lo;
        }

        public async Task<AccessToken> LoadOneAsync(string id)
        {
            var uri = UriFactory.CreateDocumentUri(m_databaseId, m_collectionId, id);

            var query = await m_client.ReadDocumentAsync<AccessToken>(uri);
            return query;
        }

        public async Task RemoveAsync(string id)
        {
            await m_client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(m_databaseId, m_collectionId, id));
        }

        public Task<LoadOperation<AccessToken>> SearchAsync(string query, int page = 1, int size = 20)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            m_client?.Dispose();
        }


        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await m_client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(m_databaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await m_client.CreateDatabaseAsync(new Database { Id = m_databaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await m_client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(m_databaseId, m_collectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await m_client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(m_databaseId),
                        new DocumentCollection { Id = m_collectionId },
                        new RequestOptions { OfferThroughput = OfferThroughput });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
