using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
    public class RabbitMqBrokerTest : IDisposable
    {
        public ITestOutputHelper Console { get; }
        private RabbitMqMessageBroker Broker { get; }
        public RabbitMqBrokerTest(ITestOutputHelper console)
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
        public async Task DeleteAllQueuesAndExchanges()
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
                    if (exchange.ToLowerInvariant().Contains("amq")) continue;

                    Console.WriteLine("Deleting... " + exchange);
                    await client.DeleteAsync(API_EXCHANGES_DEVV1 + exchange);
                }
            }
        }



        [Fact]
        public async Task CreateSubscription()
        {
            await Broker.ConnectAsync((text, arg) => { });
            var option = new QueueDeclareOption("test-one", "Test.*.*");
            await Broker.CreateSubscriptionAsync(option);

            Assert.Equal(0, await GetMessagesCount(option.QueueName, 0));
            await this.Broker.RemoveSubscriptionAsync(option.QueueName);
        }


        [Theory]
        [InlineData("test1")]
        [InlineData("test2")]
        [InlineData("test3")]
        public async Task SendMessageSubscription(string operation)
        {
            await Broker.ConnectAsync((text, arg) => { });

            var option = new QueueDeclareOption("Test-" + Guid.NewGuid(), "Test.#." + operation);
            await Broker.CreateSubscriptionAsync(option);


            var message = new BrokeredMessage
            {
                Body = await CompressAsync("Some details here " + option.QueueName),
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
            Assert.Equal(1, await GetMessagesCount(option.QueueName, 1));
        }


        [Theory]
        [InlineData("test1")]
        [InlineData("test2")]
        [InlineData("test3")]
        public async Task ReceiveMessageSubscription(string operation)
        {
            await Broker.ConnectAsync((text, arg) => { });

            var option = new QueueDeclareOption("Test-" + Guid.NewGuid(), "Test.#." + operation);
            await Broker.CreateSubscriptionAsync(option);


            var message = new BrokeredMessage
            {
                Body = await CompressAsync("Some details here " + option.QueueName),
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
            }, new SubscriberOption(option.QueueName));

            await Broker.SendAsync(message);
            flag.WaitOne(2500);
            Assert.Equal(message.Id + "", id);

        }

        [Fact]
        public async Task ReceiveMessages()
        {
            await DeleteAllQueuesAndExchanges();
            Console.WriteLine($"Connecting on thread {Thread.CurrentThread.ManagedThreadId} ...");
            await Broker.ConnectAsync((text, arg) => { });

            var qid = Strings.GenerateId(8);
            var option = new QueueDeclareOption("Test-" + qid, "Test.#.Operation")
            {
                DeadLetterQueue = $"Test-{qid}-DLQ"
            };

            const int MESSAGES_COUNT = 10_000;
            var messages = from i in Enumerable.Range(1, MESSAGES_COUNT)
                           let body = CompressAsync($"Some details here {option.QueueName} {i}").Result
                           select new BrokeredMessage
                           {
                               Body = body,
                               Crud = CrudOperation.Added,
                               Username = "erymuzuan",
                               Id = i.ToString(),
                               TryCount = 0,
                               RetryDelay = TimeSpan.FromMilliseconds(500),
                               Headers = { { "Username", "erymuzuan" } },
                               RoutingKey = "Test.added.Operation",
                               Entity = "Test",
                               Operation = "Operation",
                               ReplyTo = "me"
                           };
            var flag = new AutoResetEvent(false);
            var bags = new HashSet<int>(Enumerable.Range(1, MESSAGES_COUNT));
            var dlq = bags.Count(x => x % 100 == 0);

            Parallel.For(1, 25, i =>
            {
                Broker.CreateSubscriptionAsync(option).Wait();
                this.Broker.OnMessageDelivered(async msg =>
                {
                    var id = int.Parse(msg.Id);
                    lock (bags)
                    {
                        Assert.Contains(id, bags);
                        var removed = bags.Remove(id);
                        Assert.True(removed, "Cannot remove " + id);
                    }
                    if (bags.Count == 0)
                        flag.Set();
                    await Task.Delay(25);
                    if (id % 100 == 0)
                        return MessageReceiveStatus.Rejected;
                    return MessageReceiveStatus.Accepted;
                }, new SubscriberOption(option.QueueName, "Test-" + i) { PrefetchCount = 5 });

            });

            var pelir = Parallel.ForEach(messages, new ParallelOptions { MaxDegreeOfParallelism = 8 }, m =>
               {
                   this.Broker.SendAsync(m).ContinueWith(_ =>
                   {
                       if (null != _.Exception)
                           ObjectBuilder.GetObject<ILogger>().Log(new LogEntry(_.Exception));
                   });
               });
            Assert.True(pelir.IsCompleted);
            flag.WaitOne(5_500);
            Assert.Empty(bags);

            Assert.Equal(0, await GetMessagesCount(option.QueueName, 0, 8000));
            Assert.Equal(dlq, await GetMessagesCount(option.DeadLetterQueue, dlq,7_500));

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
            // DeleteAllQueuesAndExchanges().Wait(5000);
        }

        [Fact]
        public async Task SendToDelay()
        {
            await DeleteAllQueuesAndExchanges();
            await Broker.ConnectAsync((text, arg) => { });
            var operation = "TransientError";
            var option = new QueueDeclareOption("Test-" + Strings.GenerateId(8), "Test.#." + operation);
            await Broker.CreateSubscriptionAsync(option);

            var queueName = option.QueueName;
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
            await Task.Delay(5_000);
            Assert.Equal(1, await GetMessagesCount(delayedQueueName));
            Assert.Equal(0, await GetMessagesCount(queueName));

            // wait for it to expires
            flag2.WaitOne(expiration);
            await Task.Delay(6_000);
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
            await DeleteAllQueuesAndExchanges();
            await Broker.ConnectAsync((text, arg) => { });
            var operation = "GuaranteedFail";
            var option = new QueueDeclareOption("Test-" + Guid.NewGuid(), "Test.#." + operation)
            { DeadLetterQueue = dlq };
            await Broker.CreateSubscriptionAsync(option);

            var message = new BrokeredMessage
            {
                Body = await CompressAsync("Some details here " + option.QueueName),
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
            }, new SubscriberOption(option.QueueName));

            var before = await GetMessagesCount(dlq ?? "ms_dead_letter_queue");
            await Broker.SendAsync(message);
            flag.WaitOne(5_000);
            Assert.Equal(message.Id, id);

            Assert.Equal(before + 1, await GetMessagesCount(dlq ?? "ms_dead_letter_queue", before + 1, 10_000));
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
                while (null == messagesCountToken)
                {
                    await Task.Delay(250);
                    text = await client.GetStringAsync("/api/queues/DevV1/" + queue);
                    json = JObject.Parse(text);
                    messagesCountToken = json.SelectToken("$.messages");
                }
                return messagesCountToken.Value<int>();
            }

        }


        [Fact]
        public async Task QueueStatistics()
        {
            var option = new QueueDeclareOption("Test-" + Guid.NewGuid(), "Test.#.Stat");
            await Broker.ConnectAsync((m, e) => { });
            await Broker.CreateSubscriptionAsync(option);
            await Task.Delay(5500);
            var stat = await this.Broker.GetStatisticsAsync(option.QueueName);
            Assert.Equal(0, stat.Count);
            Assert.Equal(0, stat.DeliveryRate);
            Assert.Equal(0, stat.PublishedRate);
        }

        [Fact]
        public async Task GetMessage()
        {
            await DeleteAllQueuesAndExchanges();
            var option = new QueueDeclareOption("Test-" + Strings.GenerateId(8), "Test.#.Get");
            await Broker.ConnectAsync((m, e) => { });
            await Broker.CreateSubscriptionAsync(option);
            await Task.Delay(500);

            var stat = await this.Broker.GetStatisticsAsync(option.QueueName);
            Assert.Equal(0, stat.Count);
            Assert.Equal(0, stat.DeliveryRate);
            Assert.Equal(0, stat.PublishedRate);


            var message = new BrokeredMessage
            {
                Body = await CompressAsync("Some details here " + option.QueueName),
                Crud = CrudOperation.Added,
                Username = "erymuzuan",
                Id = Guid.NewGuid().ToString(),
                TryCount = 0,
                RetryDelay = TimeSpan.FromMilliseconds(500),
                Headers = { { "Username", "erymuzuan" } },
                RoutingKey = "Test.added.Get"
            };
            await this.Broker.SendAsync(message);
            await Task.Delay(5500);
            stat = await this.Broker.GetStatisticsAsync(option.QueueName);
            Assert.Equal(1, stat.Count);
            Assert.Equal(0, stat.DeliveryRate);
            Assert.Equal(0, stat.PublishedRate);

            var msg = await Broker.GetMessageAsync(option.QueueName);
            Assert.NotNull(msg);
            Assert.Equal(message.Id, msg.Id);

            msg.Accept();
            await Task.Delay(5500);
            stat = await this.Broker.GetStatisticsAsync(option.QueueName);
            Assert.Equal(0, stat.Count);
        }
    }
}
