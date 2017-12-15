using System;
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


        private async Task DeleteAllQueus()
        {
            var cred = new NetworkCredential(RabbitMqConfigurationManager.UserName, RabbitMqConfigurationManager.Password);
            using (var client = new HttpClient(new HttpClientHandler { Credentials = cred }) { BaseAddress = new Uri("http://localhost:15672") })
            {
                const string REQUEST_URI = "/api/queues/DevV1/";
                var text = await client.GetStringAsync(REQUEST_URI);
                var json = JArray.Parse(text);

                foreach (var jt in json)
                {
                    var queue = jt.SelectToken("$.name").ToString();
                    if (!queue.ToLowerInvariant().Contains("test-")) continue;

                    Console.WriteLine("Deleting... " + queue);
                    await client.DeleteAsync(REQUEST_URI + queue);
                }
            }
        }



        [Fact]
        public async Task CreateSubscription()
        {
            await Broker.ConnectAsync((text, arg) => { });
            var option = new QueueSubscriptionOption("test-one", "Test.*.*");
            await Broker.CreateSubscriptionAsync(option);

            var cred = new NetworkCredential(RabbitMqConfigurationManager.UserName, RabbitMqConfigurationManager.Password);
            using (var client = new HttpClient(new HttpClientHandler { Credentials = cred }) { BaseAddress = new Uri("http://localhost:15672") })
            {
                var requestUri = "/api/queues/DevV1/" + option.Name;
                var text = await client.GetStringAsync(requestUri);
                var json = JObject.Parse(text);
                var messagesCountToken = json.SelectToken("$.messages");
                Assert.NotNull(messagesCountToken);
                Assert.Equal(0, messagesCountToken.Value<int>());

                await client.DeleteAsync(requestUri);
            }
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
                    { "Username", "erymuzuan"}
                },
                RoutingKey = "Test.added." + operation
            };

            await Broker.SendAsync(message);
            await Task.Delay(2000);
            var cred = new NetworkCredential(RabbitMqConfigurationManager.UserName, RabbitMqConfigurationManager.Password);
            using (var client = new HttpClient(new HttpClientHandler { Credentials = cred }) { BaseAddress = new Uri("http://localhost:15672") })
            {
                var text = await client.GetStringAsync("/api/queues/DevV1/" + option.Name);
                await this.Broker.RemoveSubscriptionAsync(option.Name);
                var json = JObject.Parse(text);
                var messagesCountToken = json.SelectToken("$.messages");
                Assert.NotNull(messagesCountToken);
                Assert.Equal(1, messagesCountToken.Value<int>());
            }

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
            DeleteAllQueus().Wait(5000);
        }

        [Fact]
        public async Task SendToDelay()
        {
            await Task.Delay(500);
            Assert.True(false);
        }

        [Fact]
        public async Task SendToDeadLetter()
        {
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
            flag.WaitOne(2500);
            Assert.Equal(message.Id + "", id);
            await Task.Delay(2500);

            await this.Broker.RemoveSubscriptionAsync(option.Name);

            Assert.Equal(before + 1, await GetMessagesCount("ms_dead_letter_queue"));
        }

        private async Task<int> GetMessagesCount(string queue)
        {
            var cred = new NetworkCredential(RabbitMqConfigurationManager.UserName, RabbitMqConfigurationManager.Password);
            using (var client = new HttpClient(new HttpClientHandler { Credentials = cred }) { BaseAddress = new Uri("http://localhost:15672") })
            {

                var text = await client.GetStringAsync("/api/queues/DevV1/" + queue);
                var json = JObject.Parse(text);
                var messagesCountToken = json.SelectToken("$.messages");
                return messagesCountToken.Value<int>();
            }

        }

    
        [Fact]
        public async Task Stat()
        {
            await Task.Delay(500);
            Assert.True(false);
        }
    }
}
