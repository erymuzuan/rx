using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Bespoke.Sph.CosmosDbRepository
{
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : Entity, new()
    {
        private readonly string m_databaseId;
        private readonly string m_collectionId;
        private readonly DocumentClient m_client;
        public int OfferThroughput { get; set; } = 400;

      
        public ReadOnlyRepository() : this("https://localhost:8081", ConfigurationManager.ApplicationName, typeof(T).Name, "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")
        {

        }
        public ReadOnlyRepository(string uri, string databaseId, string collectionId, string key)
        {
            m_databaseId = databaseId;
            m_collectionId = collectionId;

            m_client = new DocumentClient(new Uri(uri), key);
        }

        public Task<int> GetCountAsync(Filter[] filters)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMaxAsync<TResult>(QueryDsl queryDsl)
        {
            throw new NotImplementedException();
        }

        public Task<LoadData<T>> LoadOneAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<LoadData<T>> LoadOneAsync(string field, string value)
        {
            throw new NotImplementedException();
        }

        public Task<LoadOperation<T>> SearchAsync(QueryDsl queryDsl)
        {
            throw new NotImplementedException();
        }

        public Task<LoadOperation<T>> SearchAsync(string odataUri)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }


        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await m_client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(m_databaseId));
            }
            catch (DocumentClientException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await m_client.CreateDatabaseAsync(new Database { Id = m_databaseId });
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await m_client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(m_databaseId, m_collectionId));
            }
            catch (DocumentClientException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await m_client.CreateDocumentCollectionAsync(
                    UriFactory.CreateDatabaseUri(m_databaseId),
                    new DocumentCollection { Id = m_collectionId },
                    new RequestOptions { OfferThroughput = OfferThroughput });

            }

           //
           // var token = new T();
           //TODO : creates one document
        }
    }
}
