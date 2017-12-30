using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.Extensions;
using EventLog = Bespoke.Sph.Domain.EventLog;

// ReSharper disable ExplicitCallerInfoArgument

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public abstract class Subscriber : MarshalByRefObject
    {
        public abstract string QueueName { get; }
        public abstract string[] RoutingKeys { get; }
        public ILogger NotificationService { protected get; set; }

        private int m_prefetchCount = 1;

        /// <summary>
        /// The number of messages prefetch by the broker in a batch.
        /// </summary>
        /// <returns></returns>
        public virtual int PrefetchCount
        {
            get => m_prefetchCount;
            set => m_prefetchCount = value;
        }

        public abstract void Run(IMessageBroker broker);

        protected void WriteError(Exception exception,
            [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                this.NotificationService.WriteError(exception, $"Exception is thrown in {this.QueueName}",
                    filePath, memberName, lineNumber);
                var logger = ObjectBuilder.GetObject<ILogger>();
                logger.Log(new LogEntry(exception)
                {
                    Source = this.QueueName,
                    Log = EventLog.Subscribers,
                    CallerFilePath = filePath,
                    CallerMemberName = memberName,
                    CallerLineNumber = lineNumber
                });
            }
            catch (Exception e)
            {
                this.NotificationService.WriteError(e, "Fail to log exception ", filePath, memberName, lineNumber);
            }
        }


        protected void WriteWarning(string message,
            [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            this.NotificationService.WriteWarning($"{this.GetType().Name} : {message}",
                filePath, memberName, lineNumber);
        }

        protected void WriteMessage(string message,
            [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            this.NotificationService.WriteInfo($"{this.GetType().Name} : {message}",
                filePath, memberName, lineNumber);
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnStop()
        {
        }

        public void Stop()
        {
            try
            {
                this.OnStop();
            }
            catch (Exception)
            {
                this.NotificationService.WriteInfo(
                    this.GetType().Namespace + " throws AlreadyClosedException on Stop");
            }
        }


        protected void PrintSubscriberInformation(TimeSpan elapse)
        {
            var message = new StringBuilder();
            message.Append($"{"Queue Name",-15}: {this.QueueName}\r\n");
            message.Append($"{"Routing Keys",-15}: {string.Join(",", this.RoutingKeys)}\r\n");
            message.Append(
                $"{"Config file",-15}: {Path.GetFileName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)}\r\n");
            message.Append($"{"App domain",-15}: {AppDomain.CurrentDomain.FriendlyName}\r\n");
            message.Append($"{"Startup time",-15}: {elapse.TotalSeconds}\r\n");
            this.WriteMessage(message.ToString());
        }


        protected virtual void RegisterServices()
        {
            this.RegisterCustomEntityDependencies().Wait();
        }


        public async Task RegisterCustomEntityDependencies()
        {

            var context = ObjectBuilder.GetObject<ISourceRepository>();
            var entityDefinitions = await context.LoadAsync<EntityDefinition>(x => x.IsPublished);


            var bags = new ConcurrentDictionary<Type, object>();
            Parallel.ForEach(entityDefinitions, (ed, count) =>
            {

                try
                {
                    var resolved = ObjectBuilder.GetObject<ICustomEntityDependenciesResolver>()
                        .ResolveRepository(ed);
                    bags.TryAdd(resolved.DependencyType, resolved.Implementation);
                }
                catch (FileLoadException e)
                {
                    Debug.WriteLine(e);
                }
                catch (FileNotFoundException e)
                {
                    Debug.WriteLine(e);
                }
                catch (Exception e) when (e.Message.Contains("Cannot load"))
                {
                    Debug.WriteLine(e);
                }
            });
            foreach (var type in bags.Keys)
            {
                ObjectBuilder.AddCacheList(type, bags[type]);
            }

        }


        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}