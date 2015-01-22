using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "ListView", Order = 13d, FontAwesomeIcon = "list-ul",TypeName = "ListView", Description = "ListView for collection")]
    public partial class ListView : FormElement
    {
        public override BuildError[] ValidateBuild(IForm form)
        {
            var errors = new List<BuildError>();
            if (string.IsNullOrWhiteSpace(this.ChildItemType))
                errors.Add(new BuildError(form.WebId, string.Format("[ScreenActivity] -> {1} Child item type cannot be empty for {0}", this.Path, form.Name)));

            if (!this.ChildItemType.StartsWith("bespoke."))
                errors.Add(new BuildError(null, string.Format("[ListView] ->{0} : Child item type normally in the form bespoke.sph.w_{1}_{2}.<ChildType> or one of the custom entity", this.Path, form.Id, "ipp.Version")));
            return errors.ToArray();
        }

        public override BuildError[] ValidateBuild(IProjectProvider ed)
        {
            var errors = new List<BuildError>();
            if (string.IsNullOrWhiteSpace(this.ChildItemType))
                errors.Add(new BuildError(null, string.Format("[ListView] -> Child item type cannot be empty for {0}", this.Path)));

            var jsNamespace = string.Format("bespoke.{0}_{1}.domain",
                ConfigurationManager.ApplicationName.ToLowerInvariant(), ed.Id);
            if (!this.ChildItemType.StartsWith(jsNamespace))
                errors.Add(new BuildError(null, string.Format("[ListView] ->{0} : Child item type normally in the form {1}.<ChildType>",
                    this.Path,
                    jsNamespace)));
            return errors.ToArray();
        }


    }
}