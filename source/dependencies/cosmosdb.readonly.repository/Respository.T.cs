using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.CosmosDbRepository
{
    public class Respository<T> : IReadOnlyRepository<T> where T : Entity, new()
    {
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
    }
}
