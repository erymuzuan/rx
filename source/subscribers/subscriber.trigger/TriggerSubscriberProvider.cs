using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.CustomTriggers
{
    public class TriggerSubscriberProvider
    {
        public dynamic GetSubscribers()
        {

            var context = new SphDataContext();
            var query = context.Triggers.Where(t => t.IsActive == true);
            var triggers = new List<Trigger>();
            context.LoadAsync(query, 1, 500, true)
                .ContinueWith(_ => triggers.AddRange(_.Result.ItemCollection))
                .Wait();


            var typeFormat = typeof(Entity).GetShortAssemblyQualifiedName().ToEmptyString().Replace("Entity", "{0}");


            var list = new List<dynamic>();
            
            foreach (var t in triggers)
            {
                var typeName = string.Format(typeFormat, t.Entity);
                var type = Type.GetType(typeName);
                var subsType = typeof(TriggerActionSubscriber<>).MakeGenericType(new[] { type });
                dynamic ta = Activator.CreateInstance(subsType);

                ta.SetQueueName(string.Format("trigger_{0}", t.TriggerId));
                ta.SetRoutingKeys(new []{string.Format("{0}.#",t.Entity)});
                ta.Trigger = t;

                list.Add(ta);

            }


            return list;
        }
    }
}