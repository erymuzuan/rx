using System;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class SetterAction : CustomAction
    {
        public override void Execute(RuleContext context)
        {
            throw new Exception("Not implement, use the async");
        }

        public async override Task ExecuteAsync(RuleContext context)
        {
            var item = context.Item;
            if(this.TriggerId == 0)
                throw new InvalidOperationException("Please set the trigger id");

            var script = ObjectBuilder.GetObject<IScriptEngine>();

            var code = new StringBuilder();
            foreach (var action in this.SetterActionChildCollection)
            {
                var val = action.Field.GetValue(context);
                if (val is string)
                    val = string.Format("\"{0}\"", val);
                if (val is DateTime)
                    val = string.Format("DateTime.Parse(\"{0:s}\")", val);

                code.AppendLine("item." + action.Path + " = " + val + ";");
            }
            code.Append("return item;");

            var modifiedItem = script.Evaluate(code.ToString(), item) as Entity;
            var dcontext = new SphDataContext();
            using (var session = dcontext.OpenSession())
            {
                session.Attach(modifiedItem);
                // NOTE : the subscriber should watch for the Trigger:{TriggerId} property and ignore this particular trigger
                // to avoid StackOverflow
                await session.SubmitChanges("Trigger:" + this.TriggerId + " " + this.Title);
            }
        }

        public override bool UseAsync
        {
            get { return true; }
        }
    }
}