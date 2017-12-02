using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;

namespace Bespoke.Sph.Tests.SqlServer.Mocks
{
    public class MockSourceRepository : ISourceRepository
    {
        private IList<Entity> Projects { get; } = new List<Entity>();
        public Dictionary<IProjectDefinition, IEnumerable<AttachedProperty>> Properties { get; } = new Dictionary<IProjectDefinition, IEnumerable<AttachedProperty>>();

        public Task<IEnumerable<T>> LoadAsync<T>() where T : Entity
        {
            var list = Projects.OfType<T>();
            return Task.FromResult(list.AsEnumerable());
        }

        public Task<IEnumerable<T>> LoadAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {

            var list = Projects.OfType<T>().Where(predicate.Compile());
            return Task.FromResult(list.AsEnumerable());
        }

        public Task<T> LoadOneAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            var project = Projects.OfType<T>().SingleOrDefault(predicate.Compile());
            return Task.FromResult(project);
        }
        public Task SavedAsync<T>(T project, IEnumerable<AttachedProperty> properties) where T : Entity, IProjectDefinition
        {
            Projects.Add(project);
            Properties.Add(project, properties);

            return Task.FromResult(0);
        }

        public Task<IEnumerable<AttachedProperty>> GetAttachedPropertiesAsync<T>(IProjectBuilder builder, T project) where T : Entity, IProjectDefinition
        {
            var p = this.Projects.OfType<IProjectDefinition>().SingleOrDefault(x => x.Name == project.Name);
            if (null != p && Properties.ContainsKey(p))
            {
                var properties = Properties[p].Where(x => x.ProviderName == builder.Name);
                return Task.FromResult(properties);
            }
            return Task.FromResult(Array.Empty<AttachedProperty>().AsEnumerable());

        }


    }
}