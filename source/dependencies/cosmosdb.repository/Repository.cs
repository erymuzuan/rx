using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.Azure.Documents.Client;

namespace Bespoke.Sph.CosmosDbRepository
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly string m_databaseId;
        private readonly string m_collectionId;
        private readonly DocumentClient m_client;
        public int OfferThroughput { get; set; } = 400;

        public Repository() : this("https://localhost:8081", "tokens", "access_token", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")
        {

        }
        public Repository(string uri, string databaseId, string collectionId, string key)
        {
            m_databaseId = databaseId;
            m_collectionId = collectionId;

            m_client = new DocumentClient(new Uri(uri), key);

        }

        public Task<bool> ExistAsync(IQueryable<T> query)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetAverageAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetAverageAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult?>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetAverageAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetAverageAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult?>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(IQueryable<T> query)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tuple<TResult, TResult2>>> GetList2Async<TResult, TResult2>(IQueryable<T> query, Expression<Func<T, TResult>> selector, Expression<Func<T, TResult2>> selector2)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> GetListAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMaxAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMinAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMinAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetScalarAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetScalarAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetSumAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetSumAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult?>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetSumAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<LoadOperation<T>> LoadAsync(IQueryable<T> query2, int page, int size, bool includeTotalRows)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(m_databaseId, m_collectionId);
            var query = m_client.CreateDocumentQuery<T>(uri, new FeedOptions { MaxItemCount = 20 })
                .Skip((page - 1) * size)
                .Take(size)
                .Where(query2.Co)
                .AsDocumentQuery();

            var results = new List<AccessToken>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
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

        public T LoadOne(IQueryable<T> query)
        {
            throw new NotImplementedException();
        }

        public Task<T> LoadOneAsync(IQueryable<T> query)
        {
            throw new NotImplementedException();
        }

        public Task<T> LoadOneAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
