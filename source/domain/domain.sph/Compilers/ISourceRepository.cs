using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Compilers
{
    // TODO : merge it with ICvsProvider
    public interface ISourceRepository
    {
        Task<IEnumerable<T>> LoadAsync<T>() where T : Entity;
        Task<IEnumerable<T>> LoadAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity;
        Task<T> LoadOneAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity;
    }
}