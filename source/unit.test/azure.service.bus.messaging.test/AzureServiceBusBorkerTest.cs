using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.Messaging.AzureMessaging;
using Microsoft.ServiceBus;
using Xunit;
using Xunit.Abstractions;
using BrokeredMessage = Bespoke.Sph.Domain.Messaging.BrokeredMessage;

namespace Bespoke.Sph.MessagingTests
{
    public class AzureServiceBusBorkerTest : IDisposable
    {
        public ITestOutputHelper Console { get; }
        private ServiceBusMessageBroker Broker { get; }
        public AzureServiceBusBorkerTest(ITestOutputHelper console)
        {
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));
            Console = console;
            Broker = new ServiceBusMessageBroker();
        }

        [Fact]
        public async Task Connect()
        {
            await Broker.ConnectAsync((text, arg) => { });
        }


        [Fact]
        public async Task DeleteAllTopicsAndSubscriptions()
        {
            var primaryConnectionString = AzureServiceBusConfigurationManager.PrimaryConnectionString;
            Console.WriteLine("Primary : " + primaryConnectionString);
            var namespaceMgr = NamespaceManager.CreateFromConnectionString(primaryConnectionString);

            var topics = await namespaceMgr.GetTopicsAsync();
            foreach (var t in topics)
            {
                await namespaceMgr.DeleteTopicAsync(t.Path);
                var subscriptions = await namespaceMgr.GetSubscriptionsAsync(t.Path);
                foreach (var sub in subscriptions)
                {
                    await namespaceMgr.DeleteSubscriptionAsync(t.Path, sub.Name);
                }
            }
        }



        [Fact]
        public async Task CreateSubscription()
        {
            await Broker.ConnectAsync((text, arg) => { });
            await this.Broker.RemoveSubscriptionAsync("test-one");
            var option = new QueueSubscriptionOption("test-one", "Test.*.*");
            await Broker.CreateSubscriptionAsync(option);

            Assert.Equal(0, await GetMessagesCount(option.Name, 0));
        }


        [Theory]
        [InlineData("test1")]
        [InlineData("test2")]
        [InlineData("test3")]
        public async Task SendMessageSubscription(string operation)
        {
            await Broker.ConnectAsync((text, arg) => { });

            var option = new QueueSubscriptionOption("Test-" + Guid.NewGuid(), "Test.#." + operation);
            await Broker.CreateSubscriptionAsync(option);


            var message = new BrokeredMessage
            {
                Body = await CompressAsync("Some details here " + option.Name),
                Crud = CrudOperation.Added,
                Username = "erymuzuan",
                Id = Guid.NewGuid().ToString(),
                TryCount = 0,
                RetryDelay = TimeSpan.FromMilliseconds(500),
                Headers =
                {
                    {"Username", "erymuzuan"}
                },
                RoutingKey = "Test.added." + operation
            };

            await Broker.SendAsync(message);
            Assert.Equal(1, await GetMessagesCount(option.Name, 1));
        }


        [Theory]
        [InlineData("test1")]
        [InlineData("test2")]
        [InlineData("test3")]
        public async Task ReceiveMessageSubscription(string operation)
        {
            await Broker.ConnectAsync((text, arg) => { });

            var option = new QueueSubscriptionOption("Test-" + Guid.NewGuid(), "Test.#." + operation);
            await Broker.CreateSubscriptionAsync(option);


            var message = new BrokeredMessage
            {
                Body = await CompressAsync("Some details here " + option.Name),
                Crud = CrudOperation.Added,
                Username = "erymuzuan",
                Id = Guid.NewGuid().ToString(),
                TryCount = 0,
                RetryDelay = TimeSpan.FromMilliseconds(500),
                Headers = { { "Username", "erymuzuan" } },
                RoutingKey = "Test.added." + operation
            };
            var flag = new AutoResetEvent(false);
            var id = "";
            this.Broker.OnMessageDelivered(msg =>
            {
                flag.Set();
                id = msg.Id;
                return Task.FromResult(MessageReceiveStatus.Accepted);
            }, new SubscriberOption(option.Name));

            await Broker.SendAsync(message);
            flag.WaitOne(2500);
            Assert.Equal(message.Id + "", id);

        }

        private static async Task<byte[]> CompressAsync(string value)
        {
            var content = new byte[value.Length];
            var index = 0;
            foreach (var item in value)
            {
                content[index++] = (byte)item;
            }
            using (var ms = new MemoryStream())
            using (var sw = new GZipStream(ms, CompressionMode.Compress))
            {
                await sw.WriteAsync(content, 0, content.Length);
                //NOTE : DO NOT FLUSH cause bytes will go missing...
                sw.Close();

                content = ms.ToArray();
            }
            return content;
        }

        public void Dispose()
        {
            Broker?.Dispose();
            // DeleteAllTopicsAndSubscriptions().Wait(5000);
        }

        [Fact]
        public async Task SendToDelay()
        {
            await DeleteAllTopicsAndSubscriptions();
            await Broker.ConnectAsync((text, arg) => { });
            var operation = "TransientError";
            var option = new QueueSubscriptionOption("Test-" + Guid.NewGuid(), "Test.#." + operation);
            await Broker.CreateSubscriptionAsync(option);

            var queueName = option.Name;
            var message = new BrokeredMessage
            {
                Body = await CompressAsync("Some details here " + queueName),
                Crud = CrudOperation.Added,
                Username = "erymuzuan",
                Id = Guid.NewGuid().ToString(),
                TryCount = 0,
                RetryDelay = TimeSpan.FromMilliseconds(1500),
                Headers =
                {
                    { "Username", "erymuzuan"}
                },
                RoutingKey = "Test.added." + operation
            };
            var flag1 = new AutoResetEvent(false);
            var flag2 = new AutoResetEvent(false);
            var id = "";
            var stopWatch = new Stopwatch();
            var expiration = TimeSpan.FromMilliseconds(5_000);

            this.Broker.OnMessageDelivered(msg =>
            {
                var attempt = (msg.TryCount ?? 0) + 1;
                Console.WriteLine("============ Receiving attempt " + attempt + " after " + stopWatch.Elapsed + " =============");
                id = msg.Id;
                if (attempt == 1)
                {
                    flag1.Set();
                    msg.RetryDelay = expiration;
                    return Task.FromResult(MessageReceiveStatus.Delayed);
                }
                flag2.Set();
                return Task.FromResult(MessageReceiveStatus.Accepted);
            }, new SubscriberOption(queueName));

            var delayedQueueName = "rx.delay.queue." + queueName;
            var before = await GetMessagesCount(delayedQueueName);
            Assert.Equal(0, before);

            // send and wait for OnMessage
            await Broker.SendAsync(message);
            stopWatch.Start();
            //await Broker.SendAsync(message);
            flag1.WaitOne(2_500);
            Assert.Equal(message.Id, id);


            // wait for it to be published into delayed queue
            await Task.Delay(2_000);
            Assert.Equal(1, await GetMessagesCount(delayedQueueName));
            Assert.Equal(0, await GetMessagesCount(queueName));

            // wait for it to expires
            flag2.WaitOne(expiration);
            await Task.Delay(3_000);
            Console.WriteLine("Reading queues on " + stopWatch.Elapsed);
            Assert.Equal(0, await GetMessagesCount(delayedQueueName));
            Assert.Equal(0, await GetMessagesCount(queueName)); // the message is received , once flag2 is set
            stopWatch.Stop();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("test-custom-dlq")]
        [InlineData("test-custom-dlq-2")]
        public async Task SendToCustomDeadLetter(string dlq)
        {
            await DeleteAllTopicsAndSubscriptions();
            await Broker.ConnectAsync((text, arg) => { });
            var operation = "GuaranteedFail";
            var option = new QueueSubscriptionOption("Test-" + Guid.NewGuid(), "Test.#." + operation)
            { DeadLetterQueue = dlq };
            await Broker.CreateSubscriptionAsync(option);

            var message = new BrokeredMessage
            {
                Body = await CompressAsync("Some details here " + option.Name),
                Crud = CrudOperation.Added,
                Username = "erymuzuan",
                Id = Guid.NewGuid().ToString(),
                TryCount = 0,
                RetryDelay = TimeSpan.FromMilliseconds(500),
                Headers =
                {
                    { "Username", "erymuzuan"}
                },
                RoutingKey = "Test.added." + operation
            };
            var flag = new AutoResetEvent(false);
            var id = "";
            this.Broker.OnMessageDelivered(msg =>
            {
                Console.WriteLine($" ======================  Receiving {msg.Id}/{msg.TryCount} =========================");
                flag.Set();
                id = msg.Id;
                return Task.FromResult(MessageReceiveStatus.Rejected);
            }, new SubscriberOption(option.Name));

            var before = await GetMessagesCount(dlq ?? "ms_dead_letter_queue");
            await Broker.SendAsync(message);
            flag.WaitOne(5_000);
            Assert.Equal(message.Id, id);

            Assert.Equal(before + 1, await GetMessagesCount(dlq ?? "ms_dead_letter_queue", before + 1, 10_000));
        }


        private async Task<int> GetMessagesCount(string queue, int? expected = default, int timeout = 5000)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(AzureServiceBusConfigurationManager.PrimaryConnectionString);
            var subscriptionDesc = namespaceManager.GetSubscription(AzureServiceBusConfigurationManager.DefaultTopicPath, queue);

            long messageCount = subscriptionDesc.MessageCount;
            if (expected.HasValue)
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                while (stopWatch.Elapsed < TimeSpan.FromMilliseconds(timeout))
                {
                    subscriptionDesc = namespaceManager.GetSubscription(AzureServiceBusConfigurationManager.DefaultTopicPath, queue);
                    var count = (int)subscriptionDesc.MessageCount;
                    if (count == expected) return count;
                    Console.WriteLine(".");
                    await Task.Delay(100);

                }
            }
            return (int)messageCount;
        }


        [Fact]
        public async Task QueueStatistics()
        {
            var option = new QueueSubscriptionOption("Test-" + Guid.NewGuid(), "Test.#.Stat");
            await Broker.ConnectAsync((m, e) => { });
            await Broker.CreateSubscriptionAsync(option);
            await Task.Delay(500);
            var stat = await this.Broker.GetStatisticsAsync(option.Name);
            Assert.Equal(0, stat.Count);
            Assert.Equal(0, stat.DeliveryRate);
            Assert.Equal(0, stat.PublishedRate);
        }

        [Fact]
        public async Task GetMessage()
        {
            await DeleteAllTopicsAndSubscriptions();
            var option = new QueueSubscriptionOption("Test-" + Guid.NewGuid(), "Test.#.Get");
            await Broker.ConnectAsync((m, e) => { });
            await Broker.CreateSubscriptionAsync(option);

            var zero = await this.GetMessagesCount(option.Name, 0);
            Assert.Equal(0, zero);

            var message = new BrokeredMessage
            {
                Body = await CompressAsync("Some details here " + option.Name),
                Crud = CrudOperation.Added,
                Username = "erymuzuan",
                Id = Guid.NewGuid().ToString(),
                TryCount = 0,
                RetryDelay = TimeSpan.FromMilliseconds(500),
                Headers =
                {
                    { "Username", "erymuzuan"}
                },
                RoutingKey = "Test.added.Get"
            };
            await this.Broker.SendAsync(message);
            var count = await this.GetMessagesCount(option.Name, 1);
            Assert.Equal(1, count);

            var msg = await Broker.GetMessageAsync(option.Name);
            Assert.NotNull(msg);
            Assert.Equal(message.Id, msg.Id);

            msg.Accept();
            count = await this.GetMessagesCount(option.Name, 0);
            Assert.Equal(0, count);
        }
    }
}