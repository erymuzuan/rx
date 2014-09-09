using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
        public IQueryable<DocumentTemplate> DocumentTemplates { get; set; }
        public IQueryable<EmailTemplate> EmailTemplates { get; set; }
        public IQueryable<EntityChart> EntityCharts { get; set; }
        public IQueryable<EntityDefinition> EntityDefinitions { get; set; }
        public IQueryable<EntityForm> EntityForms { get; set; }
        public IQueryable<EntityView> EntityViews { get; set; }
        public IQueryable<Organization> Organizations { get; set; }
        public IQueryable<ReportDefinition> ReportDefinitions { get; set; }
        public IQueryable<Role> Roles { get; set; }
        public IQueryable<Setting> Settings { get; set; }
        public IQueryable<UserProfile> UserProfiles { get; set; }
        public IQueryable<Trigger> Triggers { get; set; }
        public IQueryable<Watcher> Watchers { get; set; }
        public IQueryable<WorkflowDefinition> WorkflowDefinitions { get; set; }
        public IQueryable<Workflow> Workflows { get; set; }
        public IQueryable<Page> Pages { get; set; }

        private readonly QueryProvider m_provider;
        public SphDataContext()
        {
            m_provider = ObjectBuilder.GetObject<QueryProvider>();

            this.DocumentTemplates = new Query<DocumentTemplate>(m_provider);
            this.EmailTemplates = new Query<EmailTemplate>(m_provider);
            this.EntityCharts = new Query<EntityChart>(m_provider);
            this.EntityDefinitions = new Query<EntityDefinition>(m_provider);
            this.EntityForms = new Query<EntityForm>(m_provider);
            this.EntityViews = new Query<EntityView>(m_provider);
            this.AuditTrails = new Query<AuditTrail>(m_provider);
            this.Organizations = new Query<Organization>(m_provider);
            this.ReportDefinitions = new Query<ReportDefinition>(m_provider);
            this.Roles = new Query<Role>(m_provider);
            this.Settings = new Query<Setting>(m_provider);
            this.UserProfiles = new Query<UserProfile>(m_provider);
            this.Triggers = new Query<Trigger>(m_provider);
            this.Watchers = new Query<Watcher>(m_provider);
            this.Workflows = new Query<Workflow>(m_provider);
            this.WorkflowDefinitions = new Query<WorkflowDefinition>(m_provider);
            this.Pages = new Query<Page>(m_provider);
        }

        public IQueryable<T> CreateQueryable<T>()
        {
            return new Query<T>(m_provider);
        } 


        public PersistenceSession OpenSession()
        {
            var session = new PersistenceSession(this);
            return session;
        }

        private async Task<IEnumerable<Entity>> GetPreviousItems(IEnumerable<Entity> items)
        {
            var list = new ObjectCollection<Entity>();
            foreach (var item in items)
            {
                var o1 = item;
                var type = item.GetEntityType();
                var reposType = typeof(IRepository<>).MakeGenericType(new[] { type });
                var repos = ObjectBuilder.GetObject(reposType);

                var p = await repos.LoadOneAsync(o1.Id).ConfigureAwait(false);
                list.Add(p);


            }
            return list;
        }

        internal async Task<SubmitOperation> SubmitChangesAsync(string operation, PersistenceSession session, Dictionary<string, object> headers)
        {
            var addedItems = session.AttachedCollection.Where(t => string.IsNullOrWhiteSpace(t.Id)).ToArray();
            var changedItems = session.AttachedCollection.Where(t => !string.IsNullOrWhiteSpace(t.Id)).ToArray();

            var ds = ObjectBuilder.GetObject<IDirectoryService>();
            var previous = await GetPreviousItems(changedItems);
            // get changes to items
            var logs = (from e in changedItems
                        let e1 = previous.SingleOrDefault(t => t.WebId == e.WebId)
                        where null != e1
                        let diffs = (new ChangeGenerator().GetChanges(e1, e))
                        select new AuditTrail(diffs)
                        {
                            Operation = operation,
                            DateTime = DateTime.Now,
                            User = ds.CurrentUserName,
                            Type = e.GetType().Name,
                            EntityId = e.Id,
                            Note = "-"
                        }).ToArray();
            session.AttachedCollection.AddRange(logs.Cast<Entity>());

            var persistence = ObjectBuilder.GetObject<IPersistence>();
            var so = await persistence.SubmitChanges(session.AttachedCollection, session.DeletedCollection, session)
                .ConfigureAwait(false);

            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();
            var logsAddedTask = publisher.PublishAdded(operation, logs, headers);
            var addedTask = publisher.PublishAdded(operation, addedItems, headers);
            var changedTask = publisher.PublishChanges(operation, changedItems, logs, headers);
            var deletedTask = publisher.PublishDeleted(operation, session.DeletedCollection, headers);
            await Task.WhenAll(addedTask, changedTask, deletedTask, logsAddedTask).ConfigureAwait(false);


            return so;
        }

        public async Task<T> LoadOneAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            var provider = ObjectBuilder.GetObject<QueryProvider>();
            var query = new Query<T>(provider).Where(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.LoadOneAsync(query).ConfigureAwait(false);
        }
        public T LoadOne<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
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
