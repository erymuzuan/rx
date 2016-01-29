using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public abstract class Subscriber : MarshalByRefObject
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public abstract string QueueName { get; }
        public abstract string[] RoutingKeys { get; }
        public INotificationService NotificicationService { get; set; }

        private ushort m_prefetchCount = 1;
        /// <summary>
        /// The number of messages prefetch by the broker in a batch.
        /// </summary>
        /// <returns></returns>
        public virtual ushort PrefetchCount
        {
            get
            {
                if (m_prefetchCount != 1) return m_prefetchCount;
                var config = ConfigurationManager.AppSettings["sph:PrefetchCount:" + this.QueueName];
                int count;
                if (int.TryParse(config, out count))
                    return Convert.ToUInt16(count);
                return m_prefetchCount;
            }
            set { m_prefetchCount = value; }
        }

        public abstract void Run(IConnection connection);
        protected void WriteError(Exception exception)
        {
            try
            {
                this.NotificicationService.WriteError(exception, "Exception is thrown in " + this.QueueName);
                var logger = ObjectBuilder.GetObject<ILogger>();
                logger.Log(new LogEntry(exception));
            }
            catch (Exception e)
            {
                this.NotificicationService.WriteError(e, "Fail to log exception ");
            }

        }

        protected void WriteMessage(object value)
        {
            this.NotificicationService.Write(this.GetType().Name + " : " + "{0}", value);
        }

        protected void WriteMessage(string format, params object[] args)
        {
            this.NotificicationService.Write(this.GetType().Name + " : " + format, args);
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
            catch (AlreadyClosedException)
            {
                this.NotificicationService.Write(this.GetType().Namespace + " throws AlreadyClosedException on Stop");
            }
        }


        protected void PrintSubscriberInformation(TimeSpan elapse)
        {
            var message = new StringBuilder();
            message.AppendFormat("{0,-15}: {1}\r\n", "Queue Name", this.QueueName);
            message.AppendFormat("{0,-15}: {1}\r\n", "Routing Keys", string.Join(",", this.RoutingKeys));
            message.AppendFormat("{0,-15}: {1}\r\n", "Config file", Path.GetFileName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            message.AppendFormat("{0,-15}: {1}\r\n", "App domain", AppDomain.CurrentDomain.FriendlyName);
            message.AppendFormat("{0,-15}: {1}\r\n", "Startup time", elapse.TotalSeconds);
            this.WriteMessage(message.ToString());

        }


        protected virtual void RegisterServices()
        {
            this.RegisterCustomEntityDependencies().Wait();
        }


        public Task RegisterCustomEntityDependencies()
        {
            var sqlAssembly = Assembly.Load("sql.repository");
            var sqlRepositoryType = sqlAssembly.GetType("Bespoke.Sph.SqlRepository.SqlRepository`1");

            var context = new SphDataContext();
            var entityDefinitions = context.LoadFromSources<EntityDefinition>(x => x.IsPublished);


            var bags = new ConcurrentDictionary<Type, object>();
            Parallel.ForEach(entityDefinitions, (ed, count) =>
            {
                var ed1 = ed;
                try
                {
                    var edAssembly = Assembly.Load(ConfigurationManager.ApplicationName + "." + ed1.Name);
                    var edTypeName = $"{ed1.CodeNamespace}.{ed1.Name}";
                    var edType = edAssembly.GetType(edTypeName);
                    if (null == edType)
                        Console.WriteLine("Cannot create type " + edTypeName);

                    var reposType = sqlRepositoryType.MakeGenericType(edType);
                    var repository = Activator.CreateInstance(reposType);

                    var ff = typeof (IRepository<>).MakeGenericType(edType);
                    bags.AddOrReplace(ff, repository);
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

            return Task.FromResult(0);

        }



        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
