using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Devart.Data.PostgreSql;

namespace postgres.repository
{
    public class PostgreRepository<T> : IRepository<T> where T : Entity
    {
        public Task<LoadOperation<T>> LoadAsync(IQueryable<T> query, int page, int size, bool includeTotalRows)
        {
            using (var conn = new PgSqlConnection())
            using (var cmd = new PgSqlCommand(){Connection = conn})
            {
                //return await cmd.ExecuteScalarAsync();
                throw new Exception("Whoaaa");
            }
        }

        public Task<T> LoadOneAsync(IQueryable<T> query)
        {
            throw new NotImplementedException();
        }

        public Task<T> LoadOneAsync(int id)
        {
            throw new NotImplementedException();
        }

        public T LoadOne(IQueryable<T> query)
        {
            throw new NotImplementedException();
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
