using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IReadonlyRepository<T> where T : Entity
    {
        Task<LoadData<T>> LoadOneAsync(string id);
        Task<LoadData<T>> LoadOneAsync(string field, string value);

        Task<LoadOperation<T>> SearchAsync(Filter[] filters = null, Sort[] sorts = null, int skip = 0, int size = 20);
        Task<LoadOperation<T>> SearchAsync(string odataUri);
        
        Task<int> GetCountAsync(Filter[] filters);
        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);

        Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        Task<TResult> GetMaxAsync<TResult>(Filter[] filters = null, Sort[] sorts = null, int skip = 0, int size = 20);
    }
}