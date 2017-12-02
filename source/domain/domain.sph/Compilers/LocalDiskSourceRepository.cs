using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Compilers
{
    [Export(typeof(ISourceRepository))]
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
                return default;

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
            return Task.FromResult(LoadFromSources(predicate));
        }

        public Task<T> LoadOneAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            return Task.FromResult(LoadOneFromSources(predicate));
        }

        public Task SavedAsync<T>(T project, IEnumerable<AttachedProperty> properties) where T : Entity, IProjectDefinition
        {
            var path = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var json = project.ToJsonString(Formatting.Indented, VersionInfo.All);
            File.WriteAllText($"{path}{project.Name}.json", json);

            // now the properties
            var jsonProperties = properties.ToJsonString(Formatting.Indented, VersionInfo.Version);
            File.WriteAllText($"{path}{project.Name}.AttachedProperties.json", jsonProperties);


            return Task.FromResult(0);
        }

       

        public async Task<IEnumerable<AttachedProperty>> GetAttachedPropertiesAsync<T>(IProjectBuilder builder, T project) where T : Entity, IProjectDefinition
        {
            var path = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\{project.Name}.AttachedProperties.json";
            if (!Directory.Exists(path))
                return Array.Empty<AttachedProperty>();

            var persistedProperties = DeserializeFromJsonFile<AttachedProperty[]>(path);
            var defaultProperties = await builder.GetDefaultAttachedPropertiesAsync(project);

            var properties = from d in defaultProperties
                let value = persistedProperties.SingleOrDefault(x => x.Name == d.Name && x.ProviderName == builder.Name)
                where null != value
                select d.WithValue(value);

            return properties;

        }

        private static T DeserializeFromJsonFile<T>(string file)
        {
            try
            {
                var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(file), setting);
            }
            catch (IOException ioe) when (ioe.Message.StartsWith("The process cannot access the file") && ioe.Message.EndsWith("because it is being used by another process"))
            {
                Thread.Sleep(200);
                return DeserializeFromJsonFile<T>(file);
            }
            catch (JsonReaderException e)
            {
                ObjectBuilder.GetObject<ILogger>().Log(new LogEntry(e, new[] { "File", file }));
                throw new Exception($"Cannot deserialize the content of {file} to {typeof(T).FullName}", e);
            }
            catch (JsonSerializationException e)
            {
                ObjectBuilder.GetObject<ILogger>().Log(new LogEntry(e, new[] { "File", file }));
                throw new Exception($"Cannot deserialize the content of {file} to {typeof(T).FullName}", e);
            }
        }
    }
}