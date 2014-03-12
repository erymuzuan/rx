using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
//using sql = Bespoke.Sph.SqlRepository;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

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

        public abstract void Run();
        protected void WriteError(Exception exception)
        {
            var message = new StringBuilder();
            var exc = exception;
            var aeg = exc as AggregateException;
            if (null != aeg)
            {
                foreach (var ie in aeg.InnerExceptions)
                {
                    this.WriteError(ie);
                }

            }
            while (null != exc)
            {
                message.AppendLine(exc.GetType().FullName);
                message.AppendLine(exc.Message);
                message.AppendLine(exc.StackTrace);
                message.AppendLine();
                message.AppendLine();
                exc = exc.InnerException;
            }
            this.NotificicationService.WriteError("{0}", new object[] { message.ToString() });
        }

        protected void WriteMessage(object value)
        {
            this.NotificicationService.Write("{0}", new[] { value });
        }

        protected void WriteMessage(string format, params object[] args)
        {
            this.NotificicationService.Write(format, args);
        }



        protected virtual void OnStart()
        {

        }
        protected virtual void OnStop()
        {

        }

        public void Stop()
        {
            this.OnStop();
        }


        protected void PrintSubscriberInformation()
        {
            var message = new StringBuilder();
            message.AppendFormat("{0,-15}: {1}\r\n", "Queue Name", this.QueueName);
            message.AppendFormat("{0,-15}: {1}\r\n", "Routing Keys", string.Join(",", this.RoutingKeys));
            message.AppendFormat("{0,-15}: {1}\r\n", "Config file", Path.GetFileName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            message.AppendFormat("{0,-15}: {1}\r\n", "App domain", AppDomain.CurrentDomain.FriendlyName);
            this.WriteMessage(message.ToString());

        }


        protected virtual void RegisterServices()
        {
            this.RegisterCustomEntityDependencies().Wait();
        }


        public async Task RegisterCustomEntityDependencies()
        {
            var sqlAssembly = Assembly.Load("sql.repository");
            var sqlRepositoryType = sqlAssembly.GetType("Bespoke.Sph.SqlRepository.SqlRepository`1");

            var context = new SphDataContext();
            var query = context.EntityDefinitions.Where(e => e.IsPublished == true);
            var lo = await context.LoadAsync(query, includeTotalRows: true);
            var entityDefinitions = new ObjectCollection<EntityDefinition>(lo.ItemCollection);
            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(query, includeTotalRows: true, page: lo.CurrentPage + 1);
                entityDefinitions.AddRange(lo.ItemCollection);
            }

            foreach (var ed in entityDefinitions)
            {
                var ed1 = ed;
                try
                {
                    var edAssembly = Assembly.Load(ConfigurationManager.ApplicationName + "." + ed1.Name);
                    var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, ed1.EntityDefinitionId, ed1.Name);
                    var edType = edAssembly.GetType(edTypeName);
                    if (null == edType)
                        Console.WriteLine("Cannot create type " + edTypeName);

                    var reposType = sqlRepositoryType.MakeGenericType(edType);
                    var repository = Activator.CreateInstance(reposType);

                    var ff = typeof(IRepository<>).MakeGenericType(new[] { edType });

                    ObjectBuilder.AddCacheList(ff, repository);
                }
                catch (FileNotFoundException e)
                {
                    //  Console.WriteLine(e);
                }
            }

        }



        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
