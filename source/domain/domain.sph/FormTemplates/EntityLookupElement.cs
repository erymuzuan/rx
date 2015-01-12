using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Entity Lookup", Order = 15d, FontAwesomeIcon = "search", TypeName = "EntityLookupElement", Description = "Creates a command button to search for another entity to link to")]
    public partial class EntityLookupElement : FormElement
    {
        public override BuildError[] ValidateBuild(IProjectProvider ed)
        {
            var errors = new List<BuildError>();
            if (string.IsNullOrWhiteSpace(this.Entity))
                errors.Add(new BuildError(this.WebId, string.Format("[EntityLookupElement] -> Entity type cannot be empty for {0}", this.Label)));

            if (string.IsNullOrWhiteSpace(this.ValueMemberPath))
                errors.Add(new BuildError(this.WebId, string.Format("[EntityLookupElement] -> ValueMemberPath cannot be empty for {0}", this.Label)));

            if (string.IsNullOrWhiteSpace(this.DisplayMemberPath)&& string.IsNullOrWhiteSpace(this.DisplayTemplate))
                errors.Add(new BuildError(this.WebId, string.Format("[EntityLookupElement] -> DispalyTemplate or DisplayMemberPath cannot be empty for {0}", this.Label)));

            if (!string.IsNullOrWhiteSpace(this.DisplayMemberPath) && !string.IsNullOrWhiteSpace(this.DisplayTemplate))
                errors.Add(new BuildError(this.WebId, string.Format("[EntityLookupElement] -> DispalyTemplate and DisplayMemberPath cannot be both set for {0}", this.Label)));

          
            return errors.ToArray();
        }

    }
}