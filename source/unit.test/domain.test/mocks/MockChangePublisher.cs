using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit.Abstractions;

namespace domain.test
{
    internal class MockChangePublisher : IEntityChangePublisher
    {
        public ITestOutputHelper Console { get; }
        public List<Entity> ChangedItems { get; } = new List<Entity>();

        public MockChangePublisher(ITestOutputHelper console)
        {
            Console = console;
        }
        public Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection, IDictionary<string, object> headers)
        {
            foreach (var item in attachedCollection)
            {
                this.ChangedItems.Add(item);
            }
            Console.WriteLine("Publishing added....");
            return Task.FromResult(0);
        }

        public Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs, IDictionary<string, object> headers)
        {
            foreach (var item in attachedCollection)
            {
                this.ChangedItems.Add(item);
            }
            Console.WriteLine("Publishing changes....");
            return Task.FromResult(0);
        }

        public Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection, IDictionary<string, object> headers)
        {
            foreach (var item in deletedCollection)
            {
                this.ChangedItems.Add(item);
            }
            Console.WriteLine("Publishing deleted....");
            return Task.FromResult(0);
        }

        public Task SubmitChangesAsync(string operation, IEnumerable<Entity> attachedEntities, IEnumerable<Entity> deletedEntities, IDictionary<string, object> headers)
        {
            foreach (var item in attachedEntities)
            {
                this.ChangedItems.Add(item);
            }
            Console.WriteLine("Submitting changes ....");
            return Task.FromResult(0);
        }


    }
}