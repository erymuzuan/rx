using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Messaging;
using Xunit;

namespace broker.test
{
    public class Subsriber
    {
        protected virtual Task ProcessMessage(Entity item ,IDictionary<string, object> headers)
        {
            throw new Exception("...");
        }
    }
    public class EsIndexer : Subsriber
    {
        public string QueueName => "es_indexer";
        public string[] RoutingKeys => new[] { "#.added.#", "#.changed.#", "#.deleted.#" };
        protected override Task ProcessMessage(Entity item, IDictionary<string, object> headers)
        {
            Console.WriteLine($"indexer {item.Id}");
            return Task.FromResult(0);
        }
    }
    public class WorkflowExecutor : Subsriber
    {
        public string QueueName => "workflow";
        public string[] RoutingKeys => new[] { "Workflow.#.Executed" };
        protected override Task ProcessMessage(Entity item, IDictionary<string, object> headers)
        {
            Console.WriteLine($"execute workflow {item.Id}");
            return Task.FromResult(0);
        }
    }
    public class MemoryBrokerTest
    {

        [Fact]
        public async Task AddItem()
        {
            var chart = new EntityChart();

            var a = new EsIndexer();
            var b = new WorkflowExecutor();
            var subscribers = new Dictionary<object, string[]>
            {
                [a] = a.RoutingKeys,
                [b] = b.RoutingKeys
            };
            var broker = new Broker(subscribers);
            IDictionary<string, object> headers = new Dictionary<string, object>()
            {
                { "user","erymuzuan"}
            };
            await broker.PublishChanges("Save", new Entity[] { chart }, null, headers);
            Assert.Equal(a.GetType().GetShortAssemblyQualifiedName(), broker.ToString());
        }
        [Fact]
        public async Task LoadFromSubsribers()
        {
            var chart = new EntityChart {Id = "test chart"};

            var broker = new Broker();
            IDictionary<string, object> headers = new Dictionary<string, object>()
            {
                { "user","erymuzuan"}
            };
            await broker.PublishChanges("Save", new Entity[] { chart }, null, headers);
            Assert.Equal("custom_entities_es_indexer;system_asset_es_indexer;source_queue", broker.ToString());
        }
    }
}