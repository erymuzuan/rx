using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace Bespoke.Sph.Persistence
{
    internal static class ChangeUtility
    {


        public static async Task<Entity[]> GetPersistedItems(IEnumerable<Entity> items)
        {
            var list = new ObjectCollection<Entity>();
            foreach (var item in items)
            {
                var o1 = item;
                var type = item.GetEntityType();

                var option = item.GetPersistenceOption();
                if (option.IsSource)
                {
                    var file = $"{ConfigurationManager.SphSourceDirectory}\\{type.Name}\\{item.Id}.json";
                    if (!File.Exists(file))
                        continue;
                    var old = File.ReadAllText(file).DeserializeFromJson<Entity>();
                    list.Add(old);

                    continue;
                }
                if (!option.IsSqlDatabase) continue;
                if (!option.EnableAuditing) continue;

                var reposType = typeof(IRepository<>).MakeGenericType(type);
                var repos = ObjectBuilder.GetObject(reposType);

                var p = await repos.LoadOneAsync(o1.Id).ConfigureAwait(false);
                if (null != p)
                    list.Add(p);


            }
            return list.ToArray();
        }
        public static IEnumerable<Entity> GetChangedItems(IEnumerable<Entity> entities, Entity[] previous)
        {
            return (from r in entities
                let e1 = previous.SingleOrDefault(t => t.Id == r.Id)
                where null != e1
                select r).ToArray();
        }

        public static IEnumerable<Entity> GetAddedItems(IEnumerable<Entity> entities, Entity[] previous)
        {
            return (from r in entities
                let e1 = previous.SingleOrDefault(t => t.Id == r.Id)
                where null == e1
                select r).ToArray();
        }

        public static AuditTrail[] ComputeChanges(IEnumerable<Entity> current, IEnumerable<Entity> previous, string operation, MessageHeaders headers)
        {
            var logs = (from r in current
                let e1 = previous.SingleOrDefault(t => t.Id == r.Id)
                where null != e1
                let diffs = (new ChangeGenerator().GetChanges(e1, r))
                let logId = Guid.NewGuid().ToString()
                select new AuditTrail(diffs)
                {
                    Operation = operation,
                    DateTime = DateTime.Now,
                    User = headers.Username,
                    Type = r.GetType().Name,
                    EntityId = r.Id,
                    Id = logId,
                    WebId = logId,
                    Note = "-"
                }).ToArray();
            return logs;
        }

    }
}
