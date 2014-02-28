using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace Bespoke.Sph.WorkflowsExecution
{
    public class PagePublshingSubscriber : Subscriber<Page>
    {
        public override string QueueName
        {
            get { return "wd_page_queue"; }
        }

        public override string[] RoutingKeys
        {
            get
            {
                return new[]
                {
                    typeof(Page).Name + ".#.#"
                };
            }
        }

        protected override Task ProcessMessage(Page item, MessageHeaders header)
        {
            var wc = ConfigurationManager.WebPath + item.VirtualPath.Replace("~", "");
            var folder = Path.GetDirectoryName(wc) ?? "";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);


            File.WriteAllText(wc, item.Code);
            return Task.FromResult(0);
        }
    }
}