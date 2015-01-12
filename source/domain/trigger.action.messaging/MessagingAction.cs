using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Messaging
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "Messaging", TypeName = "Bespoke.Sph.Messaging.MessagingAction, trigger.action.messaging", Description = "Re route the message to the specified adapter send port", FontAwesomeIcon = "chevron-circle-right")]
    public partial class MessagingAction : CustomAction
    {

        public override async Task ExecuteAsync(RuleContext context)
        {
            if (string.IsNullOrWhiteSpace(this.Table) && !string.IsNullOrWhiteSpace(this.Operation))
            {
                dynamic adapter = Activator.CreateInstance(this.AdapterType);
                Console.WriteLine(adapter);

            }
            if (!string.IsNullOrWhiteSpace(this.Table) && !string.IsNullOrWhiteSpace(this.Crud))
            {
                var dll = this.AdapterType.Assembly;
                var ttname = this.AdapterType.Namespace + "." + this.Table + "Adapter";
                var tt = dll.GetType(ttname, true);
                if (null == tt) throw new InvalidOperationException(this.AdapterType.Namespace + "." + this.Table);
                dynamic table = Activator.CreateInstance(tt);

                // map
                dynamic map = Activator.CreateInstance(this.OutboundMapType);
                var item = await map.TransformAsync(context.Item);
                await table.InsertAsync(item);

            }


        }

        public override string GeneratorCode()
        {
            var code = new StringBuilder();

            var context = new SphDataContext();
            var map = context.LoadOne<TransformDefinition>(x => x.Id == this.OutboundMap);
            var adapter = context.LoadOne<Adapter>(x => x.Name == this.Adapter);
                        
            code.AppendLinf("var map = new {0}.Integrations.Transforms.{1}();", ConfigurationManager.ApplicationName, map.Name);
            code.AppendLine("var source = await map.TransformAsync(item);");
            if (string.IsNullOrWhiteSpace(this.Table) && !string.IsNullOrWhiteSpace(this.Operation))
            {
                code.AppendLinf("var adapter = new {0}();", this.AdapterType.FullName);
                code.AppendLinf("var response = await adapter.{0}(source);", this.Operation);

            }
            if (!string.IsNullOrWhiteSpace(this.Table) && !string.IsNullOrWhiteSpace(this.Crud))
            {
                code.AppendLinf("var adapter = new {0}.Adapters.{1}.{2}Adapter();", ConfigurationManager.ApplicationName, adapter.Schema, this.Table);
                code.AppendLinf("var response = await adapter.{0}Async(source);", this.Crud);
            }
            code.AppendLine("return response;");
            return code.ToString();
        }


        public override string GetEditorView()
        {
            return Properties.Resources.MessagingActionHtml;
        }

        public override string GetEditorViewModel()
        {
            return Properties.Resources.MessagingActionJs;
        }
    }
}
