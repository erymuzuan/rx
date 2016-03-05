using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace domain.test.reports
{
    class ReadonlyRepository<T> : IReadonlyRepository<T> where T : Entity
    {

        private readonly Dictionary<string, T> m_dictionary = new Dictionary<string, T>();

        public void AddToDictionary(string query, T data)
        {
            m_dictionary.Add(query, data);
        }
        public void Clear()
        {
            m_dictionary.Clear();
        }


        public Task<LoadData<T>> LoadOneAsync(string id)
        {
            var lo = new LoadData<T>(m_dictionary[id], "1");
            return Task.FromResult(lo);
        }

        public Task<LoadData<T>> LoadOneAsync(string field, string value)
        {
            throw new NotImplementedException();
        }

        public Task<string> SearchAsync(string query)
        {
            throw new NotImplementedException();
        }
    }
    class MockRepository<T> : IRepository<T> where T : Entity
    {

        private readonly Dictionary<string, T> m_dictionary = new Dictionary<string, T>();

        public void AddToDictionary(string query, T data)
        {
            m_dictionary.Add(query, data);
        }
        public void Clear()
        {
            m_dictionary.Clear();
        }
        public Task<LoadOperation<T>> LoadAsync(IQueryable<T> query, int page, int size, bool includeTotalRows)
        {
            throw new NotImplementedException();
        }

        public Task<T> LoadOneAsync(IQueryable<T> query)
        {
            Console.WriteLine("-----");
            Console.WriteLine(query.ToString());
            return Task.FromResult(m_dictionary[query.ToString()]);
        }

        public Task<T> LoadOneAsync(string id)
        {
            if (m_dictionary.ContainsKey(id))
                return Task.FromResult(m_dictionary[id]);

            return null;
        }

        public T LoadOne(IQueryable<T> query)
        {
            Console.WriteLine("-----");
            Console.WriteLine(query.ToString());
            return m_dictionary[query.ToString()];
        }

        public Task<int> GetCountAsync(IQueryable<T> query)
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

        public Task<TResult> GetMaxAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMinAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector) where TResult : struct
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

        public Task<bool> ExistAsync(IQueryable<T> query)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetScalarAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMinAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetSumAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector) where TResult : struct
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

        public Task<bool> ExistAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetScalarAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> GetListAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tuple<TResult, TResult2>>> GetList2Async<TResult, TResult2>(IQueryable<T> query, Expression<Func<T, TResult>> selector, Expression<Func<T, TResult2>> selector2)
        {
            throw new NotImplementedException();
        }
    }
}
