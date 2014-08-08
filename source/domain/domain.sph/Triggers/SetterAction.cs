using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "Setter", Description = "Set values to the item property", FontAwesomeIcon = "exchange")]
    public partial class SetterAction : CustomAction
    {
        public override string GetEditorView()
        {
            return Properties.Resources.SetterActionHtml;
        }

        public override string GetEditorViewModel()
        {
            return Properties.Resources.SetterActionJs;
        }

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

            var modifiedItem = script.Evaluate<Entity,Entity>(code.ToString(), item);
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