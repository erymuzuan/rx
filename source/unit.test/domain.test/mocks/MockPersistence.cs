using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace domain.test
{
    internal class MockPersistence : IPersistence
    {
     

        public Task<SubmitOperation> BulkInsertAsync(IEnumerable<Entity> items)
        {
            throw new NotImplementedException();
        }

        public async Task<SubmitOperation> SubmitChanges(IEnumerable<Entity> addedOrUpdatedItems, IEnumerable<Entity> deletedItems, PersistenceSession session, string user)
        {
            foreach (var item in addedOrUpdatedItems)
            {
                this.ChangedItems.Add(item);
            }
            Console.WriteLine("saving");
            await Task.Delay(2000).ConfigureAwait(false);
            return new SubmitOperation();
        }

        public Task<SubmitOperation> SubmitChanges(Entity item, string user)
        {
            throw new System.NotImplementedException();
        }

        private readonly List<Entity> m_changedItems = new List<Entity>();
        public List<Entity> ChangedItems
        {
            get { return m_changedItems; }
        }
    }
}