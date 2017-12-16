using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.Messaging.RabbitMqMessagings;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.MessagingTests
{
    public class BorkerTest : IDisposable
    {
        public ITestOutputHelper Console { get; }
        private RabbitMqMessageBroker Broker { get; }
        public BorkerTest(ITestOutputHelper console)
        {
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));
            Console = console;
            Broker = new RabbitMqMessageBroker();
        }

        [Fact]
        public async Task Connect()
        {
            await Broker.ConnectAsync((text, arg) => { });
        }


        [Fact]
        public async Task DeleteAllQueus()
        {
            var cred = new NetworkCredential(RabbitMqConfigurationManager.UserName, RabbitMqConfigurationManager.Password);
            using (var client = new HttpClient(new HttpClientHandler { Credentials = cred }) { BaseAddress = new Uri("http://localhost:15672") })
            {
                const string API_QUEUES_DEVV1 = "/api/queues/DevV1/";
                var queuesText = await client.GetStringAsync(API_QUEUES_DEVV1);
                var queuesJsonArray = JArray.Parse(queuesText);

                foreach (var jt in queuesJsonArray)
                {
                    var queue = jt.SelectToken("$.name").ToString();
                    if (!queue.ToLowerInvariant().Contains("test-")) continue;

                    Console.WriteLine("Deleting... " + queue);
                    await client.DeleteAsync(API_QUEUES_DEVV1 + queue);
                }
                const string API_EXCHANGES_DEVV1 = "/api/exchanges/DevV1/";
                var exchangesText = await client.GetStringAsync(API_EXCHANGES_DEVV1);
                var exchangesJsonArray = JArray.Parse(exchangesText);

                foreach (var jt in exchangesJsonArray)
                {
                    var exchange = jt.SelectToken("$.name").ToString();
                    if (!exchange.ToLowerInvariant().Contains("test-")) continue;

                    Console.WriteLine("Deleting... " + exchange);
                    await client.DeleteAsync(API_EXCHANGES_DEVV1 + exchange);
                }
            }
        }



        [Fact]
        public async Task CreateSubscription()
        {
            await Broker.ConnectAsync((text, arg) => { });
            var option = new QueueSubscriptionOption("test-one", "Test.*.*");
            await Broker.CreateSubscriptionAsync(option);

            Assert.Equal(0, await GetMessagesCount(option.Name, 0));
            await this.Broker.RemoveSubscriptionAsync(option.Name);
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
            // DeleteAllQueus().Wait(5000);
        }

        [Fact]
        public async Task SendToDelay()
        {
            await DeleteAllQueus();
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

        [Fact]
        public async Task SendToDeadLetter()
        {
            await DeleteAllQueus();
            await Broker.ConnectAsync((text, arg) => { });
            var operation = "GuaranteedFail";
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
                    { "Username", "erymuzuan"}
                },
                RoutingKey = "Test.added." + operation
            };
            var flag = new AutoResetEvent(false);
            var id = "";
            this.Broker.OnMessageDelivered(msg =>
            {
                flag.Set();
                id = msg.Id;
                return Task.FromResult(MessageReceiveStatus.Rejected);
            }, new SubscriberOption(option.Name));

            var before = await GetMessagesCount("ms_dead_letter_queue");
            await Broker.SendAsync(message);
            flag.WaitOne(5_000);
            Assert.Equal(message.Id + "", id);

            Assert.Equal(before + 1, await GetMessagesCount("ms_dead_letter_queue", before + 1, 10_000));
        }

        private async Task<int> GetMessagesCount(string queue, int? expected = default, int timeout = 5000)
        {
            var cred = new NetworkCredential(RabbitMqConfigurationManager.UserName, RabbitMqConfigurationManager.Password);
            using (var client = new HttpClient(new HttpClientHandler { Credentials = cred }) { BaseAddress = new Uri("http://localhost:15672") })
            {
                if (expected.HasValue)
                {
                    var stopWatch = new Stopwatch();
                    stopWatch.Start();
                    while (stopWatch.Elapsed < TimeSpan.FromMilliseconds(timeout))
                    {
                        var text1 = await client.GetStringAsync("/api/queues/DevV1/" + queue);
                        var json1 = JObject.Parse(text1);
                        var messagesCountToken1 = json1.SelectToken("$.messages");
                        if (null != messagesCountToken1)
                        {
                            var count = messagesCountToken1.Value<int>();
                            if (count == expected) return count;
                        }
                        await Task.Delay(500);

                    }
                }
                var text = await client.GetStringAsync("/api/queues/DevV1/" + queue);
                var json = JObject.Parse(text);
                var messagesCountToken = json.SelectToken("$.messages");
                return messagesCountToken.Value<int>();
            }

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
            await DeleteAllQueus();
            var option = new QueueSubscriptionOption("Test-" + Guid.NewGuid(), "Test.#.Get");
            await Broker.ConnectAsync((m, e) => { });
            await Broker.CreateSubscriptionAsync(option);
            await Task.Delay(500);

            var stat = await this.Broker.GetStatisticsAsync(option.Name);
            Assert.Equal(0, stat.Count);
            Assert.Equal(0, stat.DeliveryRate);
            Assert.Equal(0, stat.PublishedRate);


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
            await Task.Delay(2500);
            stat = await this.Broker.GetStatisticsAsync(option.Name);
            Assert.Equal(1, stat.Count);
            Assert.Equal(0, stat.DeliveryRate);
            Assert.Equal(0, stat.PublishedRate);

            var msg = await Broker.GetMessageAsync(option.Name);
            Assert.NotNull(msg);
            Assert.Equal(message.Id, msg.Id);

            msg.Accept();
            await Task.Delay(2500);
            stat = await this.Broker.GetStatisticsAsync(option.Name);
            Assert.Equal(0, stat.Count);
        }
    }
}
