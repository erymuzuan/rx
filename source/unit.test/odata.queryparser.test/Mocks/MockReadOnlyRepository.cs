using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ODataQueryParserTests
{
    internal class MockReadOnlyRepository<T> : IReadOnlyRepository<T> where T : Entity, new()
    {
        private readonly Dictionary<string, T> m_dictionary = new Dictionary<string, T>();

        public Task<LoadData<T>> LoadOneAsync(string id)
        {
            var lo = new LoadData<T>(m_dictionary[id], "1");
            return Task.FromResult(lo);
        }

        public Task<LoadData<T>> LoadOneAsync(string field, string value)
        {
            throw new NotImplementedException();
        }

        public Task<LoadOperation<T>> SearchAsync(QueryDsl queryDsl)
        {
            throw new NotImplementedException();
        }

        Task<LoadOperation<T>> IReadOnlyRepository<T>.SearchAsync(string odataUri)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(Filter[] filters)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(Expression<Func<T, bool>> query)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMaxAsync<TResult>(QueryDsl queryDsl)
        {
            throw new NotImplementedException();
        }

        public void AddToDictionary(string query, T data)
        {
            m_dictionary.Add(query, data);
        }

        public void Clear()
        {
            m_dictionary.Clear();
        }

        public Task<LoadOperation<T>> SearchAsync(Filter[] filters, int skip, int size)
        {
            throw new NotImplementedException();
        }

        public Task<string> SearchAsync(string query)
        {
            throw new NotImplementedException();
        }

        public Task<string> SearchAsync(string query, string queryString)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(string query, string queryString)
        {
            throw new NotImplementedException();
        }
    }
}