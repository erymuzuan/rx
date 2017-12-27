using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.Extensions;
using Bespoke.Sph.SubscribersInfrastructure;
using Humanizer;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Persistence
{
    // ReSharper disable once UnusedMember.Global
    public class PersistenceContextSubscriber : Subscriber
    {
        public override string QueueName => "persistence";
        public override string[] RoutingKeys => new[] { "persistence" };
        private TaskCompletionSource<bool> m_stoppingTcs;
        private readonly List<EntityPersistence> m_receivers = new List<EntityPersistence>();

        public override void Run(IMessageBroker broker)
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                RegisterServices();
                m_stoppingTcs = new TaskCompletionSource<bool>();
                this.StartConsume(broker).Wait();
                PrintSubscriberInformation(sw.Elapsed);
                sw.Stop();
            }
            catch (Exception e)
            {
                this.WriteError(e);
            }
        }

        protected override void OnStop()
        {
            this.WriteMessage($"!!Stoping : {this.QueueName}");

            m_stoppingTcs?.SetResult(true);

            while (true)
            {
                if (m_receivers.All(x => x.ProcessingCount == 0))
                {
                    break;
                }
                Thread.Sleep(100);
                this.NotificicationService.WriteDebug("Waiting for processing to finish...");
            }


            while (m_processing > 0)
            {
                Thread.Sleep(100);
            }



            this.WriteMessage("!!Stopped : " + this.QueueName);
        }

        private int m_processing;

        private async Task StartConsume(IMessageBroker broker)
        {
            this.NotificicationService.WriteInfo("Subscribing to persistence..");
            this.OnStart();
            await DeclareQueue(broker);
            broker.OnMessageDelivered(Received, new SubscriberOption(this.QueueName) { PrefetchCount = this.PrefetchCount });
            m_receivers.Clear();

            var entities = await GetEntityDefinitionsAsync();
            var settings = GetWorkersEntitySettings();
            foreach (var per in settings)
            {
                var entitiesToken = per.SelectToken("$.entities");
                if (null == entitiesToken) return;
                foreach (var ed in entities)
                {
                    var count = GetWorkersCount(per, ed);
                    for (var i = 0; i < count; i++)
                    {
                        var rcv = this.StartConsume(ed, broker, i);
                        m_receivers.Add(rcv);
                    }
                }
            }

        }
        private EntityPersistence StartConsume(EntityDefinition ed, IMessageBroker broker, int i)
        {
            var rcv = new EntityPersistence(ed, broker, m => this.WriteMessage(m), e => this.WriteError(e)) { PrefetchCount = this.PrefetchCount };
            rcv.DeclareQueue(broker).Wait();
            var subscriber = $"{rcv.QueueName}{(i + 1):_000}";
            broker.OnMessageDelivered(rcv.ReceivedSingle, new SubscriberOption(rcv.QueueName, subscriber) { PrefetchCount = this.PrefetchCount });
            this.NotificicationService.WriteInfo($"Subscribing to {subscriber}");
            return rcv;
        }

        private static async Task<EntityDefinition[]> GetEntityDefinitionsAsync()
        {
            var rxEntities = (new[]
                {
                    nameof(UserProfile),
                    nameof(WorkflowDefinition),
                    nameof(TransformDefinition),
                    nameof(EntityDefinition),
                    nameof(EntityView), nameof(EntityForm), nameof(Workflow), nameof(Tracker), nameof(Setting),
                    nameof(Watcher)
                })
                .Select(x => new EntityDefinition { Name = x, IsPublished = true });
            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var entities = (await repos.LoadAsync<EntityDefinition>()).Concat(rxEntities).Where(x => x.IsPublished).ToArray();
            return entities;
        }

        private static int GetWorkersCount(JToken per, EntityDefinition ed)
        {
            var entitiesToken = per.SelectToken("$.entities");
            var persistenceCount = per.SelectToken("$.instancesCount").Value<int>();

            var edToken = entitiesToken.SelectToken("$." + ed);
            var count = persistenceCount;
            var countToken = edToken?.SelectToken("$.instancesCount");
            if (null != countToken)
            {
                count = countToken.Value<int>();
            }

            return count;
        }

        private static IEnumerable<JToken> GetWorkersEntitySettings()
        {
            var env = ParseArg("env") ?? ConfigurationManager.GetEnvironmentVariable("Environment") ?? "dev";
            var configName = ParseArg("config") ?? ConfigurationManager.AppSettings["sph:WorkersConfig"] ?? "all";
            var json = JObject.Parse(
                File.ReadAllText($@"{ConfigurationManager.SphSourceDirectory}\WorkersConfig\{env}.{configName}.json"));
            var settings = json.SelectToken("$.subscriberConfigs")
                .Where(x => x.SelectToken("$.queueName").Value<string>() == "persistence");
            return settings;
        }

        private static string ParseArg(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
            return val?.Replace("/" + name + ":", string.Empty);
        }

        private async Task DeclareQueue(IMessageBroker broker)
        {
            const string DEAD_LETTER_QUEUE = "ms_dead_letter_queue";
            await broker.CreateSubscriptionAsync(new QueueDeclareOption(this.QueueName, this.RoutingKeys)
            {
                DeadLetterQueue = DEAD_LETTER_QUEUE
            });
        }



        private async Task<MessageReceiveStatus> Received(BrokeredMessage message)
        {
            Interlocked.Increment(ref m_processing);

            var json = await message.Body.DecompressAsync();
            var headers = message.Headers;
            var operation = message.Operation;
            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();

            var jo = JObject.Parse(json);
            var entities = jo.SelectToken("$.attached").Select(t => t.ToString().DeserializeFromJson<Entity>())
                .ToList();
            var deletedItems = jo.SelectToken("$.deleted").Select(t => t.ToString().DeserializeFromJson<Entity>())
                .ToList();

            var persistence = ObjectBuilder.GetObject<IPersistence>();
            if (message.IsDataImport)
            {
                var retry = message.GetNullableValue<int>("sql.retry") ?? 5;
                var wait = message.GetNullableValue<int>("sql.wait") ?? 2500;
                var bulk = await DataImportUtility.InsertImportDataAsync(entities.ToArray(), retry, wait,
                    m => this.WriteMessage(m));
                return bulk ? MessageReceiveStatus.Accepted : MessageReceiveStatus.Rejected;
            }

            this.WriteMessage($"{message.Operation} for {"item".ToQuantity(entities.Count)}");
            foreach (var item in entities)
            {
                this.WriteMessage($"{message.Operation} for {item.GetType().Name}{{ Id : \"{item.Id}\"}}");
            }
            foreach (var item in deletedItems)
            {
                this.WriteMessage($"Deleting({message.Operation}) {item.GetEntityType().Name}{{ Id : \"{item.Id}\"}}");
            }

            try
            {
                // get changes to items
                var previous = await ChangeUtility.GetPersistedItems(entities);
                var logs = ChangeUtility.ComputeChanges(entities, previous, operation, message);
                var addedItems = ChangeUtility.GetAddedItems(entities, previous);
                var changedItems = ChangeUtility.GetChangedItems(entities, previous);
                entities.AddRange(logs);

                var persistedEntities = from r in entities
                                        let opt = r.GetPersistenceOption()
                                        where opt.IsSqlDatabase
                                        select r;
                var deletedEntities = from r in deletedItems
                                      let opt = r.GetPersistenceOption()
                                      where opt.IsSqlDatabase
                                      select r;

                var so = await persistence
                    .SubmitChanges(persistedEntities.ToArray(), deletedEntities.ToArray(), null, message.Username)
                    .ConfigureAwait(false);
                Trace.WriteIf(null != so.Exeption, so.Exeption);

                var logsAddedTask = publisher.PublishAdded(operation, logs, headers);
                var addedTask = publisher.PublishAdded(operation, addedItems, headers);
                var changedTask = publisher.PublishChanges(operation, changedItems, logs, headers);
                var deletedTask = publisher.PublishDeleted(operation, deletedItems, headers);
                await Task.WhenAll(addedTask, changedTask, deletedTask, logsAddedTask).ConfigureAwait(false);


                return MessageReceiveStatus.Accepted;
            }
            catch (SqlException exc)
            {
                // republish the message to a delayed queue
                var delay = ConfigurationManager.SqlPersistenceDelay;
                var maxTry = ConfigurationManager.SqlPersistenceMaxTry;
                if ((message.TryCount ?? 0) < maxTry)
                {
                    var count = (message.TryCount ?? 0) + 1;
                    this.WriteMessage($"{count.Ordinalize()} retry on SqlException : {exc.Message}");

                    message.TryCount = count;
                    message.RetryDelay = TimeSpan.FromMilliseconds(delay);

                    await publisher.SubmitChangesAsync(operation, entities, deletedItems, headers);
                    return MessageReceiveStatus.Delayed;
                }
                else
                {
                    this.WriteMessage($"Error in {this.GetType().Name}");
                    this.WriteError(exc);
                    return MessageReceiveStatus.Rejected;
                }
            }
            catch (Exception exc)
            {
                this.WriteMessage($"Error in {this.GetType().Name}");
                this.WriteError(exc);
                return MessageReceiveStatus.Rejected;
            }
            finally
            {
                Interlocked.Decrement(ref m_processing);
            }
        }
    }
}