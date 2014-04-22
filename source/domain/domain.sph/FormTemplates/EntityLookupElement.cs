using System.Collections.Generic;

namespace Bespoke.Sph.Domain
{
    public partial class EntityLookupElement : FormElement
    {
        public override BuildError[] ValidateBuild(EntityDefinition ed)
        {
            var errors = new List<BuildError>();
            if (string.IsNullOrWhiteSpace(this.Entity))
                errors.Add(new BuildError(this.WebId, string.Format("[EntityLookupElement] -> Entity type cannot be empty for {0}", this.Label)));

            if (string.IsNullOrWhiteSpace(this.ValueMemberPath))
                errors.Add(new BuildError(this.WebId, string.Format("[EntityLookupElement] -> ValueMemberPath cannot be empty for {0}", this.Label)));

            if (string.IsNullOrWhiteSpace(this.DisplayMemberPath)&& string.IsNullOrWhiteSpace(this.DispalyTemplate))
                errors.Add(new BuildError(this.WebId, string.Format("[EntityLookupElement] -> DispalyTemplate or DisplayMemberPath cannot be empty for {0}", this.Label)));

            if (!string.IsNullOrWhiteSpace(this.DisplayMemberPath)&& !string.IsNullOrWhiteSpace(this.DispalyTemplate))
                errors.Add(new BuildError(this.WebId, string.Format("[EntityLookupElement] -> DispalyTemplate and DisplayMemberPath cannot be both set for {0}", this.Label)));

          
            return base.ValidateBuild(ed);
        }
    }
}