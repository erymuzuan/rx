﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.Sph.Domain;

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

        private bool CanFire(MessageHeaders headers)
        {
            // look for special mark for operation if fired by this  trigger previously -- SetterAction
            if (headers.Operation.StartsWith(string.Format("Trigger:{0}", this.Trigger.TriggerId)))
                return false;

            if (this.Trigger.IsFiredOnAdded && headers.Crud == CrudOperation.Added)
                return true;
            if (this.Trigger.IsFiredOnChanged && headers.Crud == CrudOperation.Changed)
                return true;
            if (this.Trigger.IsFiredOnDeleted && headers.Crud == CrudOperation.Deleted)
                return true;

            if (!string.IsNullOrWhiteSpace(this.Trigger.FiredOnOperations))
            {
                var ops = this.Trigger.FiredOnOperations.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ops.Contains(headers.Operation)) return true;
            }
            this.WriteMessage("This trigger cannot be fired");
            return false;
        }

        protected async override Task ProcessMessage(T item, MessageHeaders header)
        {
            var fire = this.CanFire(header);
            if (!fire) return;


            this.WriteMessage("Running triggers({0}) with {1} actions and {2} rules", this.Trigger.Name,
                this.Trigger.ActionCollection.Count(x => x.IsActive),
                this.Trigger.RuleCollection.Count);

            foreach (var rule in this.Trigger.RuleCollection)
            {
                try
                {
                    var result = rule.Execute(new RuleContext(item) { Log = header.Log });
                    if (!result)
                    {
                        this.WriteMessage("Rule {0} evaluated to FALSE", rule);
                        return;
                    }
                    this.WriteMessage("Rule {0} evaluated to TRUE", rule);
                }
                catch (Exception e)
                {
                    this.WriteError(e);
                }
            }


            foreach (var customAction in this.Trigger.ActionCollection.Where(a => a.IsActive))
            {
                this.WriteMessage(" ==== Executing {0} ======", customAction.Title);
                if (customAction.UseAsync)
                    await customAction.ExecuteAsync(new RuleContext(item)).ConfigureAwait(false);
                else
                    customAction.Execute(new RuleContext(item));

                this.WriteMessage("done...");
            }
        }
    }
}