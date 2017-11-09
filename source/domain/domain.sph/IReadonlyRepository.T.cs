using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IReadonlyRepository<T> where T : Entity, new()
    {
        Task<LoadData<T>> LoadOneAsync(string id);
        Task<LoadData<T>> LoadOneAsync(string field, string value);

        Task<LoadOperation<T>> SearchAsync(QueryDsl queryDsl);
        Task<LoadOperation<T>> SearchAsync(string odataUri);

        Task<int> GetCountAsync(Filter[] filters);
        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);

        Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        Task<TResult> GetMaxAsync<TResult>(QueryDsl queryDsl);
    }


    public class Aggregate
    {
        public Aggregate(string name, string path)
        {
            Name = name;
            Path = path;
        }
        public string Name { get; set; }
        public string Path { get; set; }

    }

    public class MaxAggregate : Aggregate
    {
        public MaxAggregate(string name, string path) : base(name, path)
        {

        }
    }
}