using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

            var json = project.ToJsonString(Formatting.Indented, VersionInfo.Version);
            File.WriteAllText($"{path}{project.Id}.json", json);

            // now the properties, override those in files if exists
            var newItems = properties.ToArray();
            var list = new HashSet<AttachedProperty>(newItems, new AttachedProperty.EqualityComparer());
            var file = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\{project.Name}.AttachedProperties";
            if (File.Exists(file))
            {
                var saved = DeserializeArrayFromJsonFile<AttachedProperty>(file).ToArray();
                foreach (var prop in saved)
                {
                    list.Add(prop);
                }
            }

            var jsonProperties = list.ToList().ToJsonString(Formatting.Indented, VersionInfo.Version);
            File.WriteAllText($"{path}{project.Name}.AttachedProperties", jsonProperties);


            return Task.FromResult(0);
        }



        public async Task<IEnumerable<AttachedProperty>> GetAttachedPropertiesAsync<T>(IProjectBuilder builder, T project) where T : Entity, IProjectDefinition
        {
            var defaultProperties = await builder.GetDefaultAttachedPropertiesAsync(project);

            var file = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\{project.Name}.AttachedProperties";
            if (!File.Exists(file))
                return defaultProperties;

            var persistedProperties = DeserializeArrayFromJsonFile<AttachedProperty>(file);

            var properties = from d in defaultProperties
                             let value = persistedProperties.SingleOrDefault(x => x.Name == d.Name && x.ProviderName == builder.Name)
                             where null != value
                             select d.WithValue(value);

            return properties;

        }

        public async Task<IEnumerable<AttachedProperty>> GetAttachedPropertiesAsync<T>(IProjectBuilder builder, T project, Member member) where T : Entity, IProjectDefinition
        {
            var defaultProperties = await builder.GetDefaultAttachedPropertiesAsync(member);

            var file = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\{project.Name}.AttachedProperties";
            if (!File.Exists(file))
                return defaultProperties;

            var persistedProperties = DeserializeArrayFromJsonFile<AttachedProperty>(file);

            var properties = from d in defaultProperties
                             let value = persistedProperties.SingleOrDefault(x => x.Name == d.Name && x.ProviderName == builder.Name && member.WebId == x.AttachedTo)
                             let d2 = d
                             select d2.WithValue(value, member);

            return properties;

        }

        private static IEnumerable<T> DeserializeArrayFromJsonFile<T>(string file)
        {
            try
            {
                var json = JObject.Parse(File.ReadAllText(file));

                var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                var properties = from token in json.SelectToken("$.$values")
                                 where null != token
                                 select JsonConvert.DeserializeObject<T>(token.ToString(), setting);

                return properties;
            }
            catch (IOException ioe) when (ioe.Message.StartsWith("The process cannot access the file") && ioe.Message.EndsWith("because it is being used by another process"))
            {
                Thread.Sleep(200);
                return DeserializeArrayFromJsonFile<T>(file);
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