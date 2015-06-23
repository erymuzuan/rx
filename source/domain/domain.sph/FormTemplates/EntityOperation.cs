using System.Collections.Generic;
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
                errors.Add(new BuildError(this.WebId,$"[Operation] \"{this.Name}\" cannot have anonymous and everybody at the same time"));

            if (everybody && roles)
                errors.Add(new BuildError(this.WebId,$"[Operation] \"{this.Name}\" cannot have everybody and other roles at the same time"));

            if (anonymous && roles)
                errors.Add(new BuildError(this.WebId, $"[Operation] \"{this.Name}\" cannot have anonymous and other role set at the same time"));

            return Task.FromResult(errors.AsEnumerable());
        }

        public string GetConfirmationMessage()
        {
            var nav = string.Empty;
            if (!string.IsNullOrWhiteSpace(this.NavigateSuccessUrl))
            {
                nav = "window.location='" + this.NavigateSuccessUrl + "'";
                if (this.NavigateSuccessUrl.StartsWith("="))
                {
                    nav = "window.location" + this.NavigateSuccessUrl;
                }
                if (string.IsNullOrWhiteSpace(this.SuccessMessage))
                    return nav;
            }

            if (string.IsNullOrWhiteSpace(this.SuccessMessage)
                && string.IsNullOrWhiteSpace(this.NavigateSuccessUrl))
                return string.Empty;

            if (!this.ShowSuccessMessage) return nav;

            return string.Format(@" 
                                    app.showMessage(""{0}"", ""{1}"", [""OK""])
	                                    .done(function () {{
                                            {2}
	                                    }});
                                 ", this.SuccessMessage, ConfigurationManager.ApplicationFullName, nav);
        }
    }
}