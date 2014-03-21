﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class EntityOperation : DomainObject
    {
        public Task<IEnumerable<BuildError>> ValidateBuildAsync(EntityDefinition ed)
        {
            var errors = new ObjectCollection<BuildError>();
            var everybody = this.Permissions.Contains("Everybody");
            var anonymous = this.Permissions.Contains("Anonymous");
            var roles = this.Permissions.Any(s => s != "Everybody" && s != "Anonymous");
            if (everybody && anonymous)
                errors.Add(new BuildError(this.WebId, string.Format("[Operation] \"{0}\" cannot have anonymous and everybody at the same time", this.Name)));
            
            if (everybody && roles)
                errors.Add(new BuildError(this.WebId, string.Format("[Operation] \"{0}\" cannot have everybody and other roles at the same time", this.Name)));
            
            if (anonymous && roles)
                errors.Add(new BuildError(this.WebId, string.Format("[Operation] \"{0}\" cannot have anonymous and other role set at the same time", this.Name)));
            
            return Task.FromResult(errors.AsEnumerable());
        }

        public string GetConfirmationMessage()
        {
            if (string.IsNullOrWhiteSpace(this.SuccessMessage)) return string.Empty;
            var nav = string.Empty;
            if (!string.IsNullOrWhiteSpace(this.NavigateSuccessUrl))
                nav = "window.location='" + this.NavigateSuccessUrl + "'";

            return string.Format(@" 
                                    app.showMessage(""{0}"", ""{1}"", [""OK""])
	                                    .done(function (dialogResult) {{
                                            console.log();
                                            {2}
	                                    }});
                                 ", this.SuccessMessage, ConfigurationManager.ApplicationFullName, nav);
        }
    }
}