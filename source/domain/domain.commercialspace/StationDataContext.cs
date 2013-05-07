using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Station.Domain.QueryProviders;

namespace Bespoke.Station.Domain
{
    [Export]
    public class StationDataContext
    {
        public IOrderedQueryable<Account> Accounts { set; get; }
        public IOrderedQueryable<Alert> Alerts { set; get; }
        public IOrderedQueryable<Billing> Billings { set; get; }
        public IOrderedQueryable<Delivery> Deliveries { set; get; }
        public IOrderedQueryable<Dipping> Dippings { set; get; }
        public IOrderedQueryable<DippingLookup> DippingLookups { set; get; }
        public IOrderedQueryable<Employee> Employees{ set; get; }
        public IOrderedQueryable<Order> Orders { set; get; }
        public IOrderedQueryable<Payroll> Payrolls{ set; get; }
        public IOrderedQueryable<Product> Products { set; get; }
        public IOrderedQueryable<PumpSale> PumpSales { set; get; }
        public IOrderedQueryable<Pump> Pumps { set; get; }
        public IOrderedQueryable<PumpTest> PumpTests { set; get; }
        public IOrderedQueryable<Reminder> Reminders { set; get; }
        public IQueryable<Sale> Sales { get; set; }
        public IQueryable<Setting> Settings { get; set; }
        public IQueryable<Shift> Shifts { get; set; }
        public IQueryable<Attendance> Attendances { get; set; }
        public IQueryable<Leave> Leaves { get; set; }
        public IQueryable<ShiftSchedule> ShiftSchedules { get; set; }
        public IQueryable<DailySummary> DailySummaries { get; set; }
        public IOrderedQueryable<Supplier> Suppliers { set; get; }
        public IOrderedQueryable<Tank> Tanks { set; get; }
        public IOrderedQueryable<Transaction> Transactions { set; get; }
        public IOrderedQueryable<Inventory> Inventories { set; get; }
        public IOrderedQueryable<Reconciliation> Reconciliations { set; get; }
        public IOrderedQueryable<ChequeRegistry> ChequeRegistries { set; get; }
        public IOrderedQueryable<LogEntry> LogEntries { set; get; }


        public StationDataContext()
        {
            var provider = ObjectBuilder.GetObject<QueryProvider>();
            this.Pumps = new Query<Pump>(provider);
            this.Alerts = new Query<Alert>(provider);
            this.Billings = new Query<Billing>(provider);
            this.PumpTests = new Query<PumpTest>(provider);
            this.Products = new Query<Product>(provider);
            this.PumpSales = new Query<PumpSale>(provider);
            this.Tanks = new Query<Tank>(provider);
            this.Dippings = new Query<Dipping>(provider);
            this.Orders = new Query<Order>(provider);
            this.DippingLookups = new Query<DippingLookup>(provider);
            this.Suppliers = new Query<Supplier>(provider);
            this.Employees = new Query<Employee>(provider);
            this.Shifts = new Query<Shift>(provider);
            this.Attendances = new Query<Attendance>(provider);
            this.Leaves = new Query<Leave>(provider);
            this.ShiftSchedules = new Query<ShiftSchedule>(provider);
            this.Sales = new Query<Sale>(provider);
            this.Deliveries = new Query<Delivery>(provider);
            this.Accounts = new Query<Account>(provider);
            this.Transactions = new Query<Transaction>(provider);
            this.Reminders = new Query<Reminder>(provider);
            this.Settings = new Query<Setting>(provider);
            this.LogEntries = new Query<LogEntry>(provider);
            this.Inventories = new Query<Inventory>(provider);
            this.Reconciliations = new Query<Reconciliation>(provider);
            this.ChequeRegistries = new Query<ChequeRegistry>(provider);
            this.DailySummaries = new Query<DailySummary>(provider);
            this.Payrolls = new Query<Payroll>(provider);
        }


        public PersistenceSession OpenSession()
        {
            var session = new PersistenceSession(this);
            return session;
        }

        internal async Task<SubmitOperation> SubmitChangesAsync(PersistenceSession session)
        {
            var addedItems = session.AttachedCollection.Where(t => this.GetId(t) == 0).ToArray();
            var changedItems = session.AttachedCollection.Where(t => this.GetId(t) > 0).ToArray();

            var persistence = ObjectBuilder.GetObject<IPersistence>();
            var so = await persistence.SubmitChanges(session.AttachedCollection, session.DeletedCollection, session);

            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();
            var addedTask = publisher.PublishAdded(addedItems);
            var changedTask = publisher.PublishChanges(changedItems);
            var deletedTask = publisher.PublishDeleted(session.DeletedCollection);
            await Task.WhenAll(addedTask, changedTask, deletedTask);
            
            
            return so;
        }

        public async Task<T> LoadOneAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            var provider = ObjectBuilder.GetObject<QueryProvider>();
            var query = new Query<T>(provider).Where(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.LoadOneAsync(query);
        }

        public async Task<LoadOperation<T>> LoadAsync<T>(IQueryable<T> query, int page = 1, int size = 40, bool includeTotalRows = false) where T : Entity
        {
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.LoadAsync(query, page, size, includeTotalRows);
        }

        public async Task<int> GetCountAsync<T>(IQueryable<T> query) where T : Entity
        {
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetCountAsync(query);
        }

        public async Task<int> GetCountAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            var query = Translate(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetCountAsync(query);
        }

        public async Task<bool> GetAnyAsync<T>(IQueryable<T> query) where T : Entity
        {
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.ExistAsync(query);
        }

        public async Task<bool> GetAnyAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity
        {
            var query = Translate(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.ExistAsync(query);
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
            return await repos.GetSumAsync(query, selector);
        }

        public async Task<TResult> GetSumAsync<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
            where T : Entity
            where TResult : struct
        {
            var query = Translate(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetSumAsync(query, selector);
        }
        public async Task<TResult?> GetSumAsync<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult?>> selector)
            where T : Entity
            where TResult : struct
        {
            var query = Translate(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetSumAsync(query, selector);
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
            return await repos.GetAverageAsync(query, selector);
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
            return await repos.GetMinAsync(query, selector);
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
            return await repos.GetScalarAsync(query, selector);
        }


        public async Task<IEnumerable<TResult>> GetListAsync<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
            where T : Entity
        {
            var provider = ObjectBuilder.GetObject<QueryProvider>();
            var query = new Query<T>(provider).Where(predicate);

            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetListAsync(query, selector);
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
            return await repos.GetList2Async(query, selector,selector2);
        }


        public async Task<TResult> GetScalarAsync<T, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
            where T : Entity
        {
            var provider = ObjectBuilder.GetObject<QueryProvider>();
            var query = new Query<T>(provider).Where(predicate);
            var repos = ObjectBuilder.GetObject<IRepository<T>>();
            return await repos.GetScalarAsync(query, selector);
        }

    }
}
