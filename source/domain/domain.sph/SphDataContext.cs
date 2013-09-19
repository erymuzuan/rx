using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.QueryProviders;


namespace Bespoke.Sph.Domain
{
    [Export]
    public class SphDataContext
    {
        public IQueryable<Building> Buildings { get; set; }
        public IQueryable<BuildingTemplate> BuildingTemplates { get; set; }
        public IQueryable<Space> Spaces { get; set; }
        public IQueryable<SpaceTemplate> SpaceTemplates { get; set; }
        public IQueryable<ComplaintTemplate> ComplaintTemplates { get; set; }
        public IQueryable<ApplicationTemplate> ApplicationTemplates { get; set; }
        public IQueryable<Complaint> Complaints { get; set; }
        public IQueryable<Contract> Contracts { get; set; }
        public IQueryable<Deposit> Deposits { get; set; }
        public IQueryable<Invoice> Invoices { get; set; }
        public IQueryable<Organization> Organizations { get; set; }
        public IQueryable<MaintenanceTemplate> MaintenanceTemplates { get; set; }
        public IQueryable<Payment> Payments { get; set; }
        public IQueryable<RentalApplication> RentalApplications { get; set; }
        public IQueryable<ReportDefinition> ReportDefinitions { get; set; }
        public IQueryable<Rent> Rents { get; set; }
        public IQueryable<Rebate> Rebates { get; set; }
        public IQueryable<Role> Roles { get; set; }
        public IQueryable<Setting> Settings { get; set; }
        public IQueryable<UserProfile> UserProfiles { get; set; }
        public IQueryable<Trigger> Triggers { get; set; }
        public IQueryable<Watcher> Watchers { get; set; }

        public SphDataContext()
        {
            var provider = ObjectBuilder.GetObject<QueryProvider>();

            this.Buildings = new Query<Building>(provider);
            this.BuildingTemplates = new Query<BuildingTemplate>(provider);
            this.Spaces = new Query<Space>(provider);
            this.SpaceTemplates = new Query<SpaceTemplate>(provider);
            this.MaintenanceTemplates = new Query<MaintenanceTemplate>(provider);
            this.ComplaintTemplates = new Query<ComplaintTemplate>(provider);
            this.ApplicationTemplates = new Query<ApplicationTemplate>(provider);
            this.Complaints = new Query<Complaint>(provider);
            this.Contracts = new Query<Contract>(provider);
            this.Deposits = new Query<Deposit>(provider);
            this.Invoices = new Query<Invoice>(provider);
            this.Organizations = new Query<Organization>(provider);
            this.Payments = new Query<Payment>(provider);
            this.RentalApplications = new Query<RentalApplication>(provider);
            this.ReportDefinitions = new Query<ReportDefinition>(provider);
            this.Rebates = new Query<Rebate>(provider);
            this.Rents = new Query<Rent>(provider);
            this.Roles = new Query<Role>(provider);
            this.Settings = new Query<Setting>(provider);
            this.UserProfiles = new Query<UserProfile>(provider);
            this.Triggers = new Query<Trigger>(provider);
            this.Watchers = new Query<Watcher>(provider);
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
                var type = item.GetType();
                var reposType = typeof(IRepository<>).MakeGenericType(new[] { type });
                var repos = ObjectBuilder.GetObject(reposType);
                var provider = ObjectBuilder.GetObject<QueryProvider>();

                if (type == typeof(Tenant))
                {
                    Expression<Func<Tenant, bool>> predicate = t => t.TenantId == this.GetId(o1);
                    var query = new Query<Tenant>(provider).Where(predicate);
                    var p = await repos.LoadOneAsync(query).ConfigureAwait(false);
                    list.Add(p);
                }
                if (type == typeof(RentalApplication))
                {
                    Expression<Func<RentalApplication, bool>> predicate = t => t.RentalApplicationId == this.GetId(o1);
                    var query = new Query<RentalApplication>(provider).Where(predicate);
                    var p = await repos.LoadOneAsync(query).ConfigureAwait(false);
                    list.Add(p);
                }
                if (type == typeof(Trigger))
                {
                    Expression<Func<Trigger, bool>> predicate = t => t.TriggerId == this.GetId(o1);
                    var query = new Query<Trigger>(provider).Where(predicate);
                    var p = await repos.LoadOneAsync(query).ConfigureAwait(false);
                    list.Add(p);
                }

                if (type == typeof(Contract))
                {
                    Expression<Func<Contract, bool>> predicate = t => t.ContractId == this.GetId(o1);
                    var query = new Query<Contract>(provider).Where(predicate);
                    var p = await repos.LoadOneAsync(query).ConfigureAwait(false);
                    list.Add(p);

                }
                if (type == typeof(Building))
                {
                    Expression<Func<Building, bool>> predicate = t => t.BuildingId == this.GetId(o1);
                    var query = new Query<Building>(provider).Where(predicate);
                    var p = await repos.LoadOneAsync(query).ConfigureAwait(false);
                    list.Add(p);
                }
                if (type == typeof(BuildingTemplate))
                {
                    Expression<Func<BuildingTemplate, bool>> predicate = t => t.BuildingTemplateId == this.GetId(o1);
                    var query = new Query<BuildingTemplate>(provider).Where(predicate);
                    var p = await repos.LoadOneAsync(query).ConfigureAwait(false);
                    list.Add(p);
                }
                if (type == typeof(SpaceTemplate))
                {
                    Expression<Func<SpaceTemplate, bool>> predicate = t => t.SpaceTemplateId == this.GetId(o1);
                    var query = new Query<SpaceTemplate>(provider).Where(predicate);
                    var p = await repos.LoadOneAsync(query).ConfigureAwait(false);
                    list.Add(p);
                }
                if (type == typeof(Complaint))
                {
                    Expression<Func<Complaint, bool>> predicate = t => t.ComplaintId == this.GetId(o1);
                    var query = new Query<Complaint>(provider).Where(predicate);
                    var p = await repos.LoadOneAsync(query).ConfigureAwait(false);
                    list.Add(p);
                }
                if (type == typeof(Maintenance))
                {
                    Expression<Func<Maintenance, bool>> predicate = t => t.MaintenanceId == this.GetId(o1);
                    var query = new Query<Maintenance>(provider).Where(predicate);
                    var p = await repos.LoadOneAsync(query).ConfigureAwait(false);
                    list.Add(p);
                }
            }
            return list;
        }
        internal async Task<SubmitOperation> SubmitChangesAsync(string operation, PersistenceSession session)
        {
            var addedItems = session.AttachedCollection.Where(t => this.GetId(t) == 0).ToArray();
            var changedItems = session.AttachedCollection.Where(t => this.GetId(t) > 0).ToArray();

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
                            EntityId = this.GetId(e),
                            Note = "-"
                        }).ToArray();
            session.AttachedCollection.AddRange(logs.Cast<Entity>());

            var persistence = ObjectBuilder.GetObject<IPersistence>();
            var so = await persistence.SubmitChanges(session.AttachedCollection, session.DeletedCollection, session)
                .ConfigureAwait(false);

            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();
            var logsAddedTask = publisher.PublishAdded(operation, logs);
            var addedTask = publisher.PublishAdded(operation, addedItems);
            var changedTask = publisher.PublishChanges(operation, changedItems, logs);
            var deletedTask = publisher.PublishDeleted(operation, session.DeletedCollection);
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


        private Type GetEntityType(Entity item)
        {
            var type = item.GetType();
            var attr = type.GetCustomAttribute<EntityTypeAttribute>();
            if (null != attr) return attr.Type;
            return type;
        }

        private int GetId(Entity item)
        {
            var type = this.GetEntityType(item);
            var id = type.GetProperties().AsQueryable().Single(p => p.PropertyType == typeof(int)
                                                                    && p.Name == type.Name + "Id");
            return (int)id.GetValue(item);
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
