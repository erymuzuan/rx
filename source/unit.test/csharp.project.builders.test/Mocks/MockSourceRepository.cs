using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;

namespace Bespoke.Sph.Tests.Mocks
{
    internal class MockSourceRepository : ISourceRepository
    {
        private object m_cached;
        public void AddOrReplace<T>(T value)
        {
            m_cached = value;
        }
        public Task<IEnumerable<T>> LoadAsync<T>() where T : Entity
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> LoadAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            throw new NotImplementedException();
        }

        public Task<T> LoadOneAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            return Task.FromResult((T)m_cached);
        }
    }
}
