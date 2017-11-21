using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Bespoke.Sph.CosmosRepository
{
    public class TokenRepository : ITokenRepository, IDisposable
    {
        private readonly string m_databaseId;
        private readonly string m_collectionId;
        private readonly DocumentClient m_client;

        public TokenRepository() : this("https://localhost:8081", "tokens", "access_token", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")
        {

        }

        public TokenRepository(string uri, string databaseId, string collectionId, string key)
        {
            m_databaseId = databaseId;
            m_collectionId = collectionId;

            m_client = new DocumentClient(new Uri(uri), key);

        }
        public async Task SaveAsync(AccessToken token)
        {
            await m_client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(m_databaseId, m_collectionId), token);
        }

        public async Task<LoadOperation<AccessToken>> LoadAsync(string user, DateTime expiry, int page = 1, int size = 20)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(m_databaseId, m_collectionId);
            var query = m_client.CreateDocumentQuery<AccessToken>(uri, new FeedOptions { MaxItemCount = 20 })
                .Skip((page - 1) * size)
                .Take(size)
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
                    .Skip((page - 1) * size)
                    .Take(size)
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
    }
}
