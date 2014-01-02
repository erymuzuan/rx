using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace domain.test
{
    internal class MockPersistence : IPersistence
    {
        public async Task<SubmitOperation> SubmitChanges(IEnumerable<Entity> addedOrUpdatedItems, IEnumerable<Entity> deletedItems, PersistenceSession session)
        {
            this.Building = addedOrUpdatedItems.OfType<Designation>().FirstOrDefault().Clone();
            Console.WriteLine("saving");
            await Task.Delay(2000).ConfigureAwait(false);
            return new SubmitOperation();
        }

        public Task<SubmitOperation> SubmitChanges(Entity item)
        {
            throw new System.NotImplementedException();
        }

        public Designation Building { get; set; }
    }
}