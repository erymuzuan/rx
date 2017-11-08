using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public class Query
    {
        public Query()
        {

        }
        public Query(Filter[] filters = null, Sort[] sorts = null, int skip = 0, int size = 20)
        {
            Filters.AddRange(filters);
            Sorts.AddRange(sorts);
            Skip = skip;
            Size = size;
        }

        public int Skip { get; set; } = 0;
        public int Size { get; set; } = 20;

        public ObjectCollection<Aggregate> Aggregates { get; } = new ObjectCollection<Aggregate>();
        public ObjectCollection<Filter> Filters { get; } = new ObjectCollection<Filter>();
        public ObjectCollection<Sort> Sorts { get; } = new ObjectCollection<Sort>();
    }

    public interface IReadonlyRepository<T> where T : Entity
    {
        Task<LoadData<T>> LoadOneAsync(string id);
        Task<LoadData<T>> LoadOneAsync(string field, string value);

        Task<LoadOperation<T>> SearchAsync(Query query);
        Task<LoadOperation<T>> SearchAsync(string odataUri);

        Task<int> GetCountAsync(Filter[] filters);
        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);

        Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        Task<TResult> GetMaxAsync<TResult>(Query query);
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