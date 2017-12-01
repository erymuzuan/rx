using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Compilers
{
    public interface IProjectDefinition
    {
        string AssemblyName { get; }
        string CodeNamespace { get; }
        string Name { get; }
        string Id { get; set; }

        //Task<IEnumerable<IProjectDefinition>> GetDependentProjectsAsync();
        //Task<IEnumerable<IProjectDefinition>> GetChildProjectsAsync();
        //IList<AttachProperty> AttachedProperties { get; }
    }

    public interface ISourceRepository
    {
        Task<IEnumerable<T>> LoadAsync<T>() where T : Entity;
        Task<IEnumerable<T>> LoadAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity;
        Task<T> LoadOneAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity;
    }

    [Obsolete("Move this class to specific project")]
    public class LocalDiskSourceRepository : ISourceRepository
    {
        public IEnumerable<T> LoadFromSources<T>() where T : Entity
        {
            var path = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\";
            if (!Directory.Exists(path))
                return new T[] { };

            return Directory.GetFiles(path, "*.json")
                .Select(f => f.DeserializeFromJsonFile<T>());
        }

        public IEnumerable<T> LoadFromSources<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            var path = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\";
            if (!Directory.Exists(path))
                return new T[] { };

            return Directory.GetFiles(path, "*.json")
                .Select(f => f.DeserializeFromJsonFile<T>())
                .Where(predicate.Compile());
        }

        public T LoadOneFromSources<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            var path = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\";
            if (!Directory.Exists(path))
                return default(T);

            return Directory.GetFiles(path, "*.json")
                .Select(f => f.DeserializeFromJsonFile<T>())
                .FirstOrDefault(predicate.Compile());
        }

        public Task<IEnumerable<T>> LoadAsync<T>() where T : Entity
        {
            return Task.FromResult(LoadFromSources<T>());
        }

        public Task<IEnumerable<T>> LoadAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            return Task.FromResult(LoadFromSources<T>(predicate));
        }

        public Task<T> LoadOneAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            return Task.FromResult(LoadOneFromSources<T>(predicate));
        }
    }
}