using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace trigger.test
{
    public class MockEdRepos : IRepository<EntityDefinition>
    {
        private readonly EntityDefinition m_ed;

        public MockEdRepos(EntityDefinition ed)
        {
            m_ed = ed;
        }

        public Task<LoadOperation<EntityDefinition>> LoadAsync(IQueryable<EntityDefinition> query, int page, int size, bool includeTotalRows)
        {
            throw new NotImplementedException();
        }

        public Task<EntityDefinition> LoadOneAsync(IQueryable<EntityDefinition> query)
        {
            return Task.FromResult(m_ed) ;
        }

        public Task<EntityDefinition> LoadOneAsync(string id)
        {
            throw new NotImplementedException();
        }

        public EntityDefinition LoadOne(IQueryable<EntityDefinition> query)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(IQueryable<EntityDefinition> query)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetSumAsync<TResult>(IQueryable<EntityDefinition> query, Expression<Func<EntityDefinition, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetSumAsync<TResult>(IQueryable<EntityDefinition> query, Expression<Func<EntityDefinition, TResult?>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMaxAsync<TResult>(IQueryable<EntityDefinition> query, Expression<Func<EntityDefinition, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMinAsync<TResult>(IQueryable<EntityDefinition> query, Expression<Func<EntityDefinition, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetAverageAsync<TResult>(IQueryable<EntityDefinition> query, Expression<Func<EntityDefinition, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetAverageAsync<TResult>(IQueryable<EntityDefinition> query, Expression<Func<EntityDefinition, TResult?>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistAsync(IQueryable<EntityDefinition> query)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetScalarAsync<TResult>(IQueryable<EntityDefinition> query, Expression<Func<EntityDefinition, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(Expression<Func<EntityDefinition, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMaxAsync<TResult>(Expression<Func<EntityDefinition, bool>> predicate, Expression<Func<EntityDefinition, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetMinAsync<TResult>(Expression<Func<EntityDefinition, bool>> predicate, Expression<Func<EntityDefinition, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetSumAsync<TResult>(Expression<Func<EntityDefinition, bool>> predicate, Expression<Func<EntityDefinition, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetAverageAsync<TResult>(Expression<Func<EntityDefinition, bool>> predicate, Expression<Func<EntityDefinition, TResult>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetAverageAsync<TResult>(Expression<Func<EntityDefinition, bool>> predicate, Expression<Func<EntityDefinition, TResult?>> selector) where TResult : struct
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistAsync(Expression<Func<EntityDefinition, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetScalarAsync<TResult>(Expression<Func<EntityDefinition, bool>> predicate, Expression<Func<EntityDefinition, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> GetListAsync<TResult>(IQueryable<EntityDefinition> query, Expression<Func<EntityDefinition, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tuple<TResult, TResult2>>> GetList2Async<TResult, TResult2>(IQueryable<EntityDefinition> query, Expression<Func<EntityDefinition, TResult>> selector, Expression<Func<EntityDefinition, TResult2>> selector2)
        {
            throw new NotImplementedException();
        }
    }
}