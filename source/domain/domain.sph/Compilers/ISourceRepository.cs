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
        // may be write to stream something like SourceStream, file  or Memory as underlying stream
        // TODO : Task SaveAsync<T>();
        // TODO : Task SaveAsync(string path, string content); 
        Task SavedAsync<T>(T project, IEnumerable<AttachedProperty> properties) where T : Entity, IProjectDefinition;


        Task<IEnumerable<AttachedProperty>> GetAttachedPropertiesAsync<T>(IProjectBuilder builder, T project)
            where T : Entity, IProjectDefinition;

        // TODO : Task<string> ReadAsStringAsync(string path);
        // TODO : ReadAllBytesAsync(string path);
    }
}