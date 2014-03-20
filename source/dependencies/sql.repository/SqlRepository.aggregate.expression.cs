using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;

namespace Bespoke.Sph.SqlRepository
{
    public partial class SqlRepository<T>
    {
        public async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate)
        {
            var query = Translate(predicate);
            return await this.GetCountAsync(query);
        }

        private static IQueryable<T> Translate(Expression<Func<T, bool>> predicate)
        {
            var provider = ObjectBuilder.GetObject<QueryProvider>();
            var query = new Query<T>(provider).Where(predicate);
            return query;
        }




        public async Task<TResult> GetMaxAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector) where TResult : struct
        {

            var query = Translate(predicate);
            return await this.GetMaxAsync(query, selector);
        }

        public async Task<TResult> GetMinAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            var query = Translate(predicate);
            return await this.GetMinAsync(query, selector);
        }

        public Task<TResult> GetSumAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult?>> selector)
            where TResult : struct
        {

            var query = Translate(predicate);
            return this.GetSumAsync(query, selector);
        }

        public async Task<TResult> GetSumAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector) where TResult : struct
        {

            var query = Translate(predicate);
            return await this.GetScalarAsync(query, selector);
        }


        public async Task<TResult> GetAverageAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector) where TResult : struct
        {
            var query = Translate(predicate);
            return await this.GetAverageAsync(query, selector);
        }

        public async Task<TResult> GetAverageAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult?>> selector) where TResult : struct
        {
            var query = Translate(predicate);
            return await this.GetAverageAsync(query, selector);
        }


        public async Task<bool> ExistAsync(Expression<Func<T, bool>> predicate)
        {
            var query = Translate(predicate);
            return await this.ExistAsync(query);
        }

        public async Task<TResult> GetScalarAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            var query = Translate(predicate);
            return await this.GetScalarAsync(query, selector);
        }

    }

}
