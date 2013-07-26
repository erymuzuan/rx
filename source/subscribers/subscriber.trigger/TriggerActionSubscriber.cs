using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.CustomTriggers
{
    public class TriggerActionSubscriber<T> : Subscriber<T> where T : Entity
    {
        private string m_queueName;
        private string[] m_routingKeys;
        public Trigger Trigger { get; set; }

        public void SetRoutingKeys(string[] keys)
        {
            m_routingKeys = keys;
        }
        public void SetQueueName(string name)
        {
            m_queueName = name;
        }
        public override string QueueName
        {
            get { return m_queueName; }
        }

        public override string[] RoutingKeys
        {
            get { return m_routingKeys; }
        }

        protected async override Task ProcessMessage(T item, MessageHeaders header)
        {
            await Task.Delay(500);
            Console.WriteLine("Running triggers({0}) with {1} actions and {2} rules", this.Trigger.Name,
                this.Trigger.ActionCollection.Count,
                this.Trigger.RuleCollection.Count);

            foreach (var rule in this.Trigger.RuleCollection)
            {
                try
                {

                    var result = rule.Execute(item);
                    if (!result)
                    {
                        Console.WriteLine("Rule {0} evaluated to false", rule);
                        return;
                    }
                    Console.WriteLine("Rule {0} evaluated to true", rule);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }


            foreach (var customAction in this.Trigger.ActionCollection.Where(a => a.IsActive))
            {
                this.WriteMessage(" ==== Executing {0} ======", customAction.Title);
                if (customAction.UseAsync)
                    await customAction.ExecuteAsync(item);
                else
                    customAction.Execute(item);

            }
            Console.WriteLine("OK");
        }
    }
}