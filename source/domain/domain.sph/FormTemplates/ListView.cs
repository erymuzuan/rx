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
            if (!this.ChildItemType.StartsWith(string.Format("bespoke.sph.w_{0}_{1}", wd.WorkflowDefinitionId, wd.Version)))
                errors.Add(new BuildError(null, string.Format("[ListView] ->{0} :Child item type normally in the form bespoke.sph.w_{1}_{2}.<ChildType>", this.Path, wd.WorkflowDefinitionId, wd.Version)));
            return errors.ToArray();
        }

        public override BuildError[] ValidateBuild(EntityDefinition ed)
        {
            var errors = new List<BuildError>();
            if(string.IsNullOrWhiteSpace(this.ChildItemType))
                errors.Add(new BuildError(null,string.Format("[ListView] -> Child item type cannot be empty for {0}", this.Path)));

            var jsNamespace = string.Format("bespoke.{0}_{1}.domain",
                ConfigurationManager.ApplicationName.ToLowerInvariant(), ed.EntityDefinitionId);
            if(!this.ChildItemType.StartsWith(jsNamespace))
                errors.Add(new BuildError(null,string.Format("[ListView] ->{0} : Child item type normally in the form {1}.<ChildType>", 
                    this.Path, 
                    jsNamespace)));
            return errors.ToArray();
        }
    }
}