using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit.Abstractions;

namespace domain.test
{
    internal class MockPersistence : IPersistence
    {
        public ITestOutputHelper Console { get; }

        public MockPersistence(ITestOutputHelper console)
        {
            Console = console;
        }
        public Task<SubmitOperation> BulkInsertAsync(IEnumerable<Entity> items)
        {
            throw new NotImplementedException();
        }

        public Task<SubmitOperation> SubmitChanges(IEnumerable<Entity> addedOrUpdatedItems, IEnumerable<Entity> deletedItems, PersistenceSession session, string user)
        {
            foreach (var item in addedOrUpdatedItems)
            {
                this.ChangedItems.Add(item);
            }
            Console.WriteLine("saving");
            return Task.FromResult(new SubmitOperation());
        }

        public Task<SubmitOperation> SubmitChanges(Entity item, string user)
        {
            throw new NotImplementedException();
        }

        public List<Entity> ChangedItems { get; } = new List<Entity>();
    }
}