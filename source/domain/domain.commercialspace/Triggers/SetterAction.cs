using System;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class SetterAction : CustomAction
    {
        public override void Execute(Entity item)
        {
            throw new System.NotImplementedException();
        }

        public async override Task ExecuteAsync(Entity item)
        {
            var script = ObjectBuilder.GetObject<IScriptEngine>();
            // TODO : translate path to the property path
            var code = new StringBuilder();
            foreach (var action in this.SetterActionChildCollection)
            {
                var val = action.Field.GetValue(item);
                if (val is string)
                    val = string.Format("\"{0}\"", val);

                code.AppendLine("item." + action.Path + " = " + val + ";");
            }
            code.Append("return item;");
            Console.WriteLine(code);

            var modifiedItem = script.Evaluate(code.ToString(), item) as Entity;
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(modifiedItem);
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