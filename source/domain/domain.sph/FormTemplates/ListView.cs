using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "ListView", Order = 13d, FontAwesomeIcon = "list-ul",TypeName = "ListView", Description = "ListView for collection")]
    public partial class ListView : FormElement
    {

        public override BuildError[] ValidateBuild(EntityDefinition ed)
        {
            var errors = new List<BuildError>();
            if (string.IsNullOrWhiteSpace(this.ChildItemType))
                errors.Add(new BuildError(null, string.Format("[ListView] -> Child item type cannot be empty for {0}", this.Path)));

            return errors.ToArray();
        }

    }
}