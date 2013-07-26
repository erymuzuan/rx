using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain.QueryProviders;


namespace Bespoke.SphCommercialSpaces.Domain
{
    [Export]
    public class SphDataContext
    {
        public IQueryable<Building> Buildings { get; set; }
        public IQueryable<CommercialSpace> CommercialSpaces { get; set; }
        public IQueryable<Complaint> Complaints { get; set; }
        public IQueryable<Contract> Contracts { get; set; }
        public IQueryable<Deposit> Deposits { get; set; }
        public IQueryable<Invoice> Invoices { get; set; }
        public IQueryable<Organization> Organizations { get; set; }
        public IQueryable<RentalApplication> RentalApplications { get; set; }
        public IQueryable<Rent> Rents { get; set; }
        public IQueryable<Role> Roles { get; set; }
        public IQueryable<Setting> Settings { get; set; }
        public IQueryable<UserProfile> UserProfiles { get; set; }
        public IQueryable<Trigger> Triggers { get; set; }

        public SphDataContext()
        {
            var provider = ObjectBuilder.GetObject<QueryProvider>();

            this.Buildings = new Query<Building>(provider);
            this.CommercialSpaces = new Query<CommercialSpace>(provider);
            this.Complaints = new Query<Complaint>(provider);
            this.Contracts = new Query<Contract>(provider);
            this.Deposits = new Query<Deposit>(provider);
            this.Invoices = new Query<Invoice>(provider);
            this.Organizations = new Query<Organization>(provider);
            this.RentalApplications = new Query<RentalApplication>(provider);
            this.Rents = new Query<Rent>(provider);
            this.Roles = new Query<Role>(provider);
            this.Settings = new Query<Setting>(provider);
            this.UserProfiles = new Query<UserProfile>(provider);
            this.Triggers = new Query<Trigger>(provider);
        }


        public PersistenceSession OpenSession()
        {
            var session = new PersistenceSession(this);
            return session;
        }

        internal async Task<SubmitOperation> SubmitChangesAsync(string operation, PersistenceSession session)
        {
            Console.WriteLine("sending  to changes {0} {1} items" , operation, session.AttachedCollection.Count);
            var addedItems = session.AttachedCollection.Where(t => this.GetId(t) == 0).ToArray();
            var changedItems = session.AttachedCollection.Where(t => this.GetId(t) > 0).ToArray();

            var persistence = ObjectBuilder.GetObject<IPersistence>();
            var so = await persistence.SubmitChanges(session.AttachedCollection, session.DeletedCollection, session)
                .ConfigureAwait(false);

            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();
            var addedTask = publisher.PublishAdded(operation, addedItems);
            var changedTask = publisher.PublishChanges(operation, changedItems);
            var deletedTask = publisher.PublishDeleted(operation, session.DeletedCollection);
            await Task.WhenAll(addedTask, changedTask, deletedTask).ConfigureAwait(false);


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
