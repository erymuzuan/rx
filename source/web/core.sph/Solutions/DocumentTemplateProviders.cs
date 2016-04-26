using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class DocumentTemplateProviders : SourceAssetProviders<DocumentTemplate>
    {
        protected override string Icon => "fa fa-file-word-o";
        protected override string GetIcon(DocumentTemplate d) => this.Icon;
        protected override string GetEditUrl(DocumentTemplate d) => $"document.template.details/{d.Id}";
        protected override string GetName(DocumentTemplate d) => d.Name;
    }
}