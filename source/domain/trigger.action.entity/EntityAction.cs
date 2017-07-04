using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Messaging
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "Entity", TypeName = "Bespoke.Sph.Messaging.EntityAction, trigger.action.entity", Description = "Route the message to the specified entity", FontAwesomeIcon = "arrow-circle-o-right")]
    public partial class EntityAction : CustomAction
    {

        public override async Task ExecuteAsync(RuleContext rc)
        {
            // map
            dynamic map = Activator.CreateInstance(this.OutboundMapType);
            var item = await map.TransformAsync(rc.Item);

            var ctx = new SphDataContext();
            using (var session = ctx.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges(this.Title);
            }

        }

        public override string GeneratorCode()
        {
            var code = new StringBuilder();
            var context = new SphDataContext();
            var map = context.LoadOne<TransformDefinition>(x => x.Id == this.OutboundMap);

            code.AppendLine($"var map = new {map.CodeNamespace}.{map.ClassName}();");
            code.AppendLine("var source = await map.TransformAsync(item);");
            code.AppendLine($@"  
                var ctx = new SphDataContext();
                using (var session = ctx.OpenSession())
                {{
                    session.Attach(source);
                    await session.SubmitChanges(""{this.Operation}"");
                }}");
            code.AppendLine($@"return source;");
            return code.ToString();
        }


        public override string GetEditorView()
        {
            return Properties.Resources.EntityActionHtml;
        }

        public override string GetEditorViewModel()
        {
            return Properties.Resources.EntityActionJs;
        }
    }
}
