using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IReadonlyRepository
    {
        Task TruncateAsync(EntityDefinition ed);
        Task CleanAsync(EntityDefinition ed);
        Task CleanAsync();
        Task<object> SearchAsync(string types, Filter[] filters);
        Task<int> GetCountAsync(string entity);
    }
    public interface IReadonlyRepository<T> where T : Entity
    {
        Task<LoadData<T>> LoadOneAsync(string id);
        Task<LoadData<T>> LoadOneAsync(string field, string value);
        Task<LoadOperation<T>> SearchAsync(Filter[] filters, int skip, int size);
        Task<string> SearchAsync(string query);
        Task<string> SearchAsync(string query, string queryString);
        Task<int> GetCountAsync(string query, string queryString);
        Task<int> GetCountAsync(Expression<Func<T, bool>> query);
        Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
    }

    public class LoadData<T> where T : Entity
    {
        public LoadData(T source, string version)
        {
            Source = source;
            Version = version;
        }

        public T Source { get;  }
        public string Version { get; }
        public string Json { get; set; }
    }
}