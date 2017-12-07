using System;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.Sph.Domain;
using Humanizer;

namespace Bespoke.Sph.ReadOnlyRepositoriesWorkers
{
    public class ReadOnlySyncSubscriber : Subscriber<Entity>
    {
        public override string QueueName => this.GetType().FullName;
        public override string[] RoutingKeys => new[] {"#.added.#", "#.changed.#", "#.deleted.#"};
        

        protected override async Task ProcessMessage(Entity item, MessageHeaders headers)
        {

            var syncManager = ObjectBuilder.GetObject<IReadOnlyRepositorySyncManager>();
            try
            {
                if (headers.Crud == CrudOperation.Added)
                {
                    await syncManager.AddAsync(item);
                }
                if (headers.Crud == CrudOperation.Changed)
                {
                    await syncManager.UpdateAsync(item);
                }
                if (headers.Crud == CrudOperation.Deleted)
                {
                    await syncManager.DeleteAsync(item);
                }

            }
            catch (Exception e)
            {
                // republish the message to a delayed queue
                var delay = ConfigurationManager.GetEnvironmentVariableInt32("ReadOnlyRepositorySyncRetryDelay", 500);
                var maxTry = ConfigurationManager.GetEnvironmentVariableInt32("ReadOnlyRepositorySyncMaxTry", 5);
                if ((headers.TryCount ?? 0) < maxTry)
                {
                    await RequeueMessageAsync(item, headers, e, delay);
                }
                else
                {
                    this.WriteMessage($"Error in {this.GetType().Name}");
                    this.WriteError(e);
                    throw;
                }
            }
            
        
        }

        private async Task RequeueMessageAsync(Entity item, MessageHeaders headers, Exception e, long delay)
        {
            var count = (headers.TryCount ?? 0) + 1;
            this.WriteMessage($"{count.Ordinalize()} retry on HttpRequestException : {e.Message}");

            var ph = headers.GetRawHeaders();
            ph.AddOrReplace(MessageHeaders.SPH_DELAY, delay);
            ph.AddOrReplace(MessageHeaders.SPH_TRYCOUNT, count);

            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();
            await publisher.PublishChanges(headers.Operation, new[] {item}, new AuditTrail[] { }, ph);
        }
    }
}