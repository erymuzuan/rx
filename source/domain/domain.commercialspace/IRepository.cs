using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Bespoke.Station.Domain
{
    public interface IRepository<T> where T : Entity 
    {
        Task<LoadOperation<T>> LoadAsync(IQueryable<T> query, int page, int size, bool includeTotalRows);
        Task<T> LoadOneAsync(IQueryable<T> query);
        Task<int> GetCountAsync(IQueryable<T> query);

        Task<TResult> GetSumAsync<TResult>(IQueryable<T> query,Expression<Func<T,TResult>> selector) where TResult : struct;
        Task<TResult> GetSumAsync<TResult>(IQueryable<T> query,Expression<Func<T,TResult?>> selector) where TResult : struct;

        Task<TResult> GetMaxAsync<TResult>(IQueryable<T> query,Expression<Func<T,TResult>> selector) where TResult : struct;

        Task<TResult> GetMinAsync<TResult>(IQueryable<T> query,Expression<Func<T,TResult>> selector) where TResult : struct;

        Task<TResult> GetAverageAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)where TResult : struct;

        Task<TResult> GetAverageAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult?>> selector)where TResult : struct ;

        Task<bool> ExistAsync(IQueryable<T> query);


        Task<TResult> GetScalarAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector);

        Task<IEnumerable<TResult>> GetListAsync<TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector);
        Task<IEnumerable<Tuple<TResult, TResult2>>> GetList2Async<TResult, TResult2>(IQueryable<T> query, Expression<Func<T, TResult>> selector, Expression<Func<T, TResult2>> selector2);
    }
}
