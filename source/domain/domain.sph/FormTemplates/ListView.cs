using System.Collections.Generic;

namespace Bespoke.Sph.Domain
{
    public partial class ListView : FormElement
    {
        public override BuildError[] ValidateBuild(WorkflowDefinition wd, ScreenActivity screen)
        {
            var errors = new List<BuildError>();
            if(string.IsNullOrWhiteSpace(this.ChildItemType))
                errors.Add(new BuildError(screen.WebId,string.Format("[ScreenActivity] -> {1} Child item type cannot be empty for {0}", this.Path, screen.Name)));
            return errors.ToArray();
        }

        public override BuildError[] ValidateBuild(EntityDefinition ed)
        {
            var errors = new List<BuildError>();
            if(string.IsNullOrWhiteSpace(this.ChildItemType))
                errors.Add(new BuildError(null,string.Format("[ListView] -> Child item type cannot be empty for {0}", this.Path)));
            return errors.ToArray();
        }
    }
}