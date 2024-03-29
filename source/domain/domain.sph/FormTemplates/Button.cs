﻿using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Button", Order = 7d, FontAwesomeIcon = "square",TypeName = "Button", Description = "Creates a command button")]
    public partial class Button : FormElement
    {
        public override bool IsPathIsRequired => false;

        public override BuildDiagnostic[] ValidateBuild(EntityDefinition ed)
        {
            var message = $"[Button] -> '{this.Label}' ";
            if(!string.IsNullOrWhiteSpace(this.Operation) && !string.IsNullOrWhiteSpace(this.CommandName))
                return new[] { new BuildDiagnostic(this.WebId, message + "You cannot have both Operation and Command set at the same time") };

            if (!string.IsNullOrWhiteSpace(this.CommandName) && CommandName != "save" && string.IsNullOrWhiteSpace(this.Command))
            {
                return new[] { new BuildDiagnostic(this.WebId, message + "Please set the command text for " + this.CommandName) };
            }
            return base.ValidateBuild(ed);
        }

    }
}