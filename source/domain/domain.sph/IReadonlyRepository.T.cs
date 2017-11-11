using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IReadOnlyRepository<T> where T : Entity, new()
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

        public virtual TResult GetValue<TResult>()
        {
            if (null == m_result)
                return default;

            if (typeof(TResult) == typeof(DateTime))
            {
                if (DateTime.TryParse(m_stringResult, out var dt))
                    return (TResult)(object)dt;
            }
            return (TResult)m_result;
        }

        private object m_result;
        private string m_stringResult;

        public virtual void SetValue<TResult>(TResult value)
        {
            m_result = value;
        }

        public virtual void SetStringValue(string stringValue)
        {
            m_stringResult = stringValue;
        }
    }

    public class MaxAggregate : Aggregate
    {
        public MaxAggregate(string name, string path) : base(name, path)
        {

        }

    }

    public class AverageAggregate : Aggregate
    {
        public AverageAggregate(string name, string path) : base(name, path)
        {
            
        }
    }
    public class MinAggregate : Aggregate
    {
        public MinAggregate(string name, string path) : base(name, path)
        {
            
        }
    }
    public class SumAggregate : Aggregate
    {
        public SumAggregate(string name, string path) : base(name, path)
        {
            
        }
    }
    public class CountDistinctAggregate : Aggregate
    {
        public CountDistinctAggregate(string name, string path) : base(name, path)
        {
            
        }
    }
    public class GroupByAggregate : Aggregate
    {
        public GroupByAggregate(string name, string path) : base(name, path)
        {
            
        }
    }
}