using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Entity Lookup", Order = 15d, FontAwesomeIcon = "search", TypeName = "EntityLookupElement", Description = "Creates a command button to search for another entity to link to")]
    public partial class EntityLookupElement : FormElement
    {
        public override BuildDiagnostic[] ValidateBuild(EntityDefinition ed)
        {
            var errors = new List<BuildDiagnostic>();
            if (string.IsNullOrWhiteSpace(this.Entity))
                errors.Add(new BuildDiagnostic(this.WebId,$"[EntityLookupElement] -> Entity type cannot be empty for {this.Label}"));

            if (string.IsNullOrWhiteSpace(this.ValueMemberPath))
                errors.Add(new BuildDiagnostic(this.WebId,$"[EntityLookupElement] -> ValueMemberPath cannot be empty for {this.Label}"));

            if (string.IsNullOrWhiteSpace(this.DisplayMemberPath)&& string.IsNullOrWhiteSpace(this.DisplayTemplate))
                errors.Add(new BuildDiagnostic(this.WebId,$"[EntityLookupElement] -> DispalyTemplate or DisplayMemberPath cannot be empty for {this.Label}"));

            if (!string.IsNullOrWhiteSpace(this.DisplayMemberPath) && !string.IsNullOrWhiteSpace(this.DisplayTemplate))
                errors.Add(new BuildDiagnostic(this.WebId,$"[EntityLookupElement] -> DispalyTemplate and DisplayMemberPath cannot be both set for {this.Label}"));

          
            return errors.ToArray();
        }
    }
}