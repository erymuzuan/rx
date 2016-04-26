using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class EmailTemplateProviders : SourceAssetProviders<EmailTemplate>
    {
        protected override string Icon => "fa fa-envelope-o";
        protected override string GetIcon(EmailTemplate d) => this.Icon;
        protected override string GetEditUrl(EmailTemplate d) => $"email.template.details/{d.Id}";
        protected override string GetName(EmailTemplate d) => d.Name;
    }
}