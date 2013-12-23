using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

//using sql = Bespoke.Sph.SqlRepository;

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

        protected bool CanStop
        {
            get { return m_stopped; }
        }
        private bool m_stopped;
        public void Stop()
        {
            m_stopped = true;
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

        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
