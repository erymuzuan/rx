using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace Bespoke.Sph.CustomTriggers
{
    public class TriggerSubscriberProvider
    {
        public Subscriber[] GetSubscribers()
        {
            var context = new SphDataContext();
            var query = context.Triggers.Where(t => t.IsActive == true);
            var triggers = new List<Trigger>();
            context.LoadAsync(query, 1, 500, true)
                .ContinueWith(_ => triggers.AddRange(_.Result.ItemCollection))
                .Wait();

            var list = new List<Subscriber>();
            
            foreach (var t in triggers)
            {
                var trigger1 = t;
                var ed = context.LoadOne<EntityDefinition>(e => e.Name == trigger1.Entity);
                
                var typeName = string.Format("Bespoke.{0}_{1}.Domain.{2}",ConfigurationManager.ApplicationName, ed.EntityDefinitionId, trigger1.Entity);
                var assembly = Assembly.Load(ConfigurationManager.ApplicationName + "." + t.Entity);
                var type = assembly.GetType(typeName);
               
                var subsType = typeof(TriggerActionSubscriber<>).MakeGenericType(new[] { type });
                dynamic ta = Activator.CreateInstance(subsType);

                ta.SetQueueName(string.Format("trigger_{0}", t.TriggerId));
                ta.SetRoutingKeys(new []{string.Format("{0}.#.#",t.Entity)});
                ta.Trigger = t;

                list.Add(ta);

            }


            return list.ToArray();
        }
    }
}