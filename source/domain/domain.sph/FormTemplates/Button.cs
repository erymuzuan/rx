namespace Bespoke.Sph.Domain
{
    public partial class Button : FormElement
    {
        public override BuildError[] ValidateBuild(EntityDefinition ed)
        {
            var message =string.Format("[Button] -> '{0}' ",this.Label);
            if(!string.IsNullOrWhiteSpace(this.Operation) && !string.IsNullOrWhiteSpace(this.CommandName))
                return new[] { new BuildError(this.WebId, message + "You cannot have both Operation and Command set at the same time") };

            if (!string.IsNullOrWhiteSpace(this.CommandName) && CommandName != "save" && string.IsNullOrWhiteSpace(this.Command))
            {
                return new[] { new BuildError(this.WebId, message + "Please set the command text for " + this.CommandName) };
            }
            return base.ValidateBuild(ed);
        }

    }
}