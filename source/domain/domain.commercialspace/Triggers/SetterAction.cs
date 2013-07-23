using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public class SetterAction : CustomAction
    {
        private readonly Dictionary<string, string> m_pathValueCollection = new Dictionary<string, string>();

        public Dictionary<string, string> PathValueCollection
        {
            get { return m_pathValueCollection; }
        }
        public override void Execute(Entity item)
        {
            throw new System.NotImplementedException();
        }

        public async override Task ExecuteAsync(Entity item)
        {
            // TODO : translate path to the property path
            foreach (var path in this.PathValueCollection.Keys)
            {
                Console.WriteLine(path + "=" + this.PathValueCollection[path]);
            }

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(item);
                // NOTE : the subscriber should watch for the Trigger:{TriggerId} property and ignore this particular trigger
                // to avoid StackOverflow
                await session.SubmitChanges("Trigger:" + this.TriggerId);
            }
        }

        public override bool UseAsync
        {
            get { return true; }
        }
    }
}