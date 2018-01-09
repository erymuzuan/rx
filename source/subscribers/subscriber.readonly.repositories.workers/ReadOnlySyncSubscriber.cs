using System;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Messaging;
using Humanizer;

namespace Bespoke.Sph.ReadOnlyRepositoriesWorkers
{
    public class ReadOnlySyncSubscriber : Subscriber<Entity>
    {
        public override string QueueName => "readonly.sync";
        public override string[] RoutingKeys => new[] {"#.added.#", "#.changed.#", "#.deleted.#"};
        

        protected override async Task ProcessMessage(Entity item, BrokeredMessage message)
        {

            var syncManager = ObjectBuilder.GetObject<IReadOnlyRepositorySyncManager>();
            try
            {
                if (message.Crud == CrudOperation.Added)
                {
                    await syncManager.AddAsync(item);
                }
                if (message.Crud == CrudOperation.Changed)
                {
                    await syncManager.UpdateAsync(item);
                }
                if (message.Crud == CrudOperation.Deleted)
                {
                    await syncManager.DeleteAsync(item);
                }

            }
            catch (Exception e)
            {
                // republish the message to a delayed queue
                var delay = ConfigurationManager.GetEnvironmentVariableInt32("ReadOnlyRepositorySyncRetryDelay", 500);
                var maxTry = ConfigurationManager.GetEnvironmentVariableInt32("ReadOnlyRepositorySyncMaxTry", 5);
                if ((message.TryCount ?? 0) < maxTry)
                {
                    await RequeueMessageAsync(item, message, e, delay);
                }
                else
                {
                    this.WriteMessage($"Error in {this.GetType().Name}");
                    this.WriteError(e);
                    throw;
                }
            }
            
        
        }

        private async Task RequeueMessageAsync(Entity item, BrokeredMessage message, Exception e, long delay)
        {
            var count = (message.TryCount ?? 0) + 1;
            this.WriteMessage($"{count.Ordinalize()} retry on HttpRequestException : {e.Message}");

            var ph = message.Headers;
            ph.AddOrReplace("RetryDelay", delay);
            ph.AddOrReplace("RetriedAttempt", count);

            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();
            await publisher.PublishChanges(message.Operation, new[] {item}, new AuditTrail[] { }, ph);
        }
    }
}