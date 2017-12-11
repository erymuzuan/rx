using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "ListView", Order = 13d, FontAwesomeIcon = "list-ul",TypeName = "ListView", Description = "ListView for collection")]
    public partial class ListView : FormElement
    {

        public override BuildDiagnostic[] ValidateBuild(EntityDefinition ed)
        {
            var errors = new List<BuildDiagnostic>();
            if (string.IsNullOrWhiteSpace(this.ChildItemType))
                errors.Add(new BuildDiagnostic(null, ($"[ListView] -> Child item type cannot be empty for {Path}")));

            return errors.ToArray();
        }

    }
}