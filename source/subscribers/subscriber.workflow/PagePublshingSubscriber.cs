using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace Bespoke.Sph.WorkflowsExecution
{
    public class ScreenActivityFormPublshingSubscriber : Subscriber<ScreenActivityForm>
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
                    typeof(ScreenActivityForm).Name + ".#.#"
                };
            }
        }

        protected override Task ProcessMessage(ScreenActivityForm item, MessageHeaders header)
        {

            this.WriteError(new NotImplementedException("Whoaaa ScreenActivityForm not implemented"));
            return Task.FromResult(0);
        }
    }
}