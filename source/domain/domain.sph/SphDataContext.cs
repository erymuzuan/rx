using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.QueryProviders;

namespace Bespoke.Sph.Domain
{
    [Export]
    public class SphDataContext
    {
        public IQueryable<AuditTrail> AuditTrails { get; set; }
        public IQueryable<Organization> Organizations { get; set; }
        public IQueryable<Role> Roles { get; set; }
        public IQueryable<Setting> Settings { get; set; }
        public IQueryable<UserProfile> UserProfiles { get; set; }
        public IQueryable<Watcher> Watchers { get; set; }
        public IQueryable<Workflow> Workflows { get; set; }

        private readonly QueryProvider m_provider;
        public SphDataContext()
        {
            m_provider = ObjectBuilder.GetObject<QueryProvider>();

            this.AuditTrails = new Query<AuditTrail>(m_provider);
            this.Organizations = new Query<Organization>(m_provider);
            this.Roles = new Query<Role>(m_provider);
            this.Settings = new Query<Setting>(m_provider);
            this.UserProfiles = new Query<UserProfile>(m_provider);
            this.Watchers = new Query<Watcher>(m_provider);
            this.Workflows = new Query<Workflow>(m_provider);
        }

        public IQueryable<T> CreateQueryable<T>()
        {
            return new Query<T>(m_provider);
        }


        public IEnumerable<T> LoadFromSources<T>() where T : Entity
        {
            string path = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\";
            if (!Directory.Exists(path))
                return new T[] { };

            return Directory.GetFiles(path, "*.json")
                .Select(f => f.DeserializeFromJsonFile<T>());
        }

        public IEnumerable<T> LoadFromSources<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            string path = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\";
            if (!Directory.Exists(path))
                return new T[] { };

            return Directory.GetFiles(path, "*.json")
                .Select(f => f.DeserializeFromJsonFile<T>())
                .Where(predicate.Compile());
        }
        public T LoadOneFromSources<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            string path = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}\\";
            if (!Directory.Exists(path))
                return default(T);

            return Directory.GetFiles(path, "*.json")
                .Select(f => f.DeserializeFromJsonFile<T>())
                .FirstOrDefault(predicate.Compile());
        }


        public PersistenceSession OpenSession()
        {
            var session = new PersistenceSession(this);
            return session;
        }

        internal async Task<SubmitOperation> SubmitChangesAsync(string operation, PersistenceSession session, IDictionary<string, object> headers)
        {
            if (null == headers)
                headers = new Dictionary<string, object>();

            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();
            var so = new SubmitOperation { Token = Guid.NewGuid().ToString() };
            headers.AddOrReplace("sph.token", so.Token);
            headers.AddOrReplace("sph.timestamp", DateTime.Now.ToString("s"));
            headers.AddOrReplace("username", ObjectBuilder.GetObject<IDirectoryService>().CurrentUserName);

            await publisher.SubmitChangesAsync(operation, session.AttachedCollection, session.DeletedCollection, headers)
                .ConfigureAwait(false);

            return so;
        }

        public async Task<T> LoadOneAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            var storeAsSource = StoreAsSourceAttribute.GetAttribute<T>();
            if (null != storeAsSource)
                return this.LoadOneFromSources(predicate);


            var provider = ObjectBuilder.GetObject<QueryProvider>();
            var query = new Query<T>(provider).Where(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.LoadOneAsync(query).ConfigureAwait(false);
        }
        public T LoadOne<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            var storeAsSource = StoreAsSourceAttribute.GetAttribute<T>();
            if (null != storeAsSource)
                return this.LoadOneFromSources(predicate);

            var provider = ObjectBuilder.GetObject<QueryProvider>();
            var query = new Query<T>(provider).Where(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return repos.LoadOne(query);
        }

        public async Task<LoadOperation<T>> LoadAsync<T>(IQueryable<T> query, int page = 1, int size = 40, bool includeTotalRows = false) where T : Entity
        {
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.LoadAsync(query, page, size, includeTotalRows).ConfigureAwait(false);
        }

        public async Task<int> GetCountAsync<T>(IQueryable<T> query) where T : Entity
        {
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetCountAsync(query).ConfigureAwait(false);
        }

        public async Task<int> GetCountAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            var source = StoreAsSourceAttribute.GetAttribute<T>();
            if (null != source && !source.IsSqlDatabase)
            {
                var list = LoadFromSources(predicate);
                return list.Count();
            }
            var query = Translate(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetCountAsync(query).ConfigureAwait(false);
        }

        public async Task<bool> GetAnyAsync<T>(IQueryable<T> query) where T : Entity
        {
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.ExistAsync(query).ConfigureAwait(false);
        }

        public async Task<bool> GetAnyAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            var query = Translate(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.ExistAsync(query).ConfigureAwait(false);
        }

        private static IQueryable<T> Translate<T>(Expression<Func<T, bool>> predicate)
        {
            var provider = ObjectBuilder.GetObject<QueryProvider>();
            var query = new Query<T>(provider).Where(predicate);
            return query;
        }

        public async Task<TResult> GetSumAsync<T, TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
            where T : Entity
            where TResult : struct
        {
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetSumAsync(query, selector);
        }
        public async Task<TResult?> GetSumAsync<T, TResult>(IQueryable<T> query, Expression<Func<T, TResult?>> selector)
            where T : Entity
            where TResult : struct
        {
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetSumAsync(query, selector).ConfigureAwait(false);
        }

        public async Task<TResult> GetSumAsync<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
            where T : Entity
            where TResult : struct
        {
            var query = Translate(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetSumAsync(query, selector).ConfigureAwait(false);
        }
        public async Task<TResult?> GetSumAsync<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult?>> selector)
            where T : Entity
            where TResult : struct
        {
            var query = Translate(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetSumAsync(query, selector).ConfigureAwait(false);
        }



        public async Task<TResult> GetAverageAsync<T, TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
            where T : Entity
            where TResult : struct
        {
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetAverageAsync(query, selector);
        }


        public async Task<TResult> GetAverageAsync<T, TResult>(IQueryable<T> query, Expression<Func<T, TResult?>> selector)
            where T : Entity
            where TResult : struct
        {
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetAverageAsync(query, selector).ConfigureAwait(false);
        }

        public async Task<TResult> GetMaxAsync<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
            where T : Entity
            where TResult : struct
        {
            var source = StoreAsSourceAttribute.GetAttribute<T>();
            if (null != source && !source.IsSqlDatabase)
            {
                var list = this.LoadFromSources(predicate).ToArray();
                return !list.Any() ? default(TResult) : list.Max(selector.Compile());
            }

            var query = Translate(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetMaxAsync(query, selector);
        }

        public async Task<TResult> GetMaxAsync<T, TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
            where T : Entity
            where TResult : struct
        {
            
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetMaxAsync(query, selector);
        }

        public async Task<TResult> GetMinAsync<T, TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
            where T : Entity
            where TResult : struct
        {
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetMinAsync(query, selector).ConfigureAwait(false);
        }


        public async Task<TResult> GetScalarAsync<T, TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
            where T : Entity
        {
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetScalarAsync(query, selector).ConfigureAwait(false);
        }


        public async Task<IEnumerable<TResult>> GetListAsync<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
            where T : Entity
        {
            var source = StoreAsSourceAttribute.GetAttribute<T>();
            if (null != source && !source.IsSqlDatabase)
            {
                var list = LoadFromSources(predicate)
                    .Select(selector.Compile());
                return list;
            }
            var provider = ObjectBuilder.GetObject<QueryProvider>();
            var query = new Query<T>(provider).Where(predicate);

            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetListAsync(query, selector).ConfigureAwait(false);
        }


        public async Task<IEnumerable<TResult>> GetListAsync<T, TResult>(IQueryable<T> query, Expression<Func<T, TResult>> selector)
            where T : Entity
        {
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetListAsync(query, selector);
        }


        public async Task<IEnumerable<Tuple<TResult, TResult2>>> GetListAsync<T, TResult, TResult2>(IQueryable<T> query, Expression<Func<T, TResult>> selector, Expression<Func<T, TResult2>> selector2)
            where T : Entity
        {
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetList2Async(query, selector, selector2).ConfigureAwait(false);
        }


        public async Task<TResult> GetScalarAsync<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
            where T : Entity
        {
            var provider = ObjectBuilder.GetObject<QueryProvider>();
            var query = new Query<T>(provider).Where(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetScalarAsync(query, selector).ConfigureAwait(false);
        }

    }
}
