using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

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
                errors.Add(new BuildError(this.WebId, $"[Operation] \"{this.Name}\" cannot have anonymous and everybody at the same time"));

            if (everybody && roles)
                errors.Add(new BuildError(this.WebId, $"[Operation] \"{this.Name}\" cannot have everybody and other roles at the same time"));

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

            return
                $@" 
                                    app.showMessage(""{this.SuccessMessage}"", ""{ConfigurationManager
                    .ApplicationFullName}"", [""OK""])
	                                    .done(function () {{
                                            {nav}
	                                    }});
                                 ";
        }

        public Method GenerateMethod(EntityDefinition ed)
        {
            var operation = this;
            var code = new StringBuilder();
            var everybody = operation.Permissions.Contains("Everybody");
            var anonymous = operation.Permissions.Contains("Anonymous");
            // SAVE
            code.AppendLine($"       //exec:{Name}");
            code.AppendLine("       [HttpPost]");
            code.AppendLine($"       [Route(\"{Name}\")]");
            if (everybody)
                code.AppendLine("       [Authorize]");

            if (!everybody && !anonymous && string.Join(",", operation.Permissions.Where(s => s != "Everybody" && s != "Anonymous")).Length > 0)
                code.AppendLinf("       [Authorize(Roles=\"{0}\")]", string.Join(",", operation.Permissions.Where(s => s != "Everybody" && s != "Anonymous")));

            code.AppendLine($"       public async Task<System.Web.Mvc.ActionResult> {Name}([RequestBody]{ed.Name} item)");
            code.AppendLine("       {");
            code.AppendLine("           var context = new Bespoke.Sph.Domain.SphDataContext();");
            code.AppendLine("           if(null == item) throw new ArgumentNullException(\"item\");");
            code.AppendLine($"           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Id == \"{ed.Id}\");");

            code.AppendLine("           var brokenRules = new ObjectCollection<ValidationResult>();");
            var count = 0;
            foreach (var rule in operation.Rules)
            {
                count++;
                code.AppendLine($@"
            var appliedRules{count} = ed.BusinessRuleCollection.Where(b => b.Name == ""{rule}"");
            ValidationResult result{count} = item.ValidateBusinessRule(appliedRules{count});

            if(!result{1}.Success){{
                brokenRules.Add(result{count});
            }}
");
            }
            code.AppendLine("           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});");

            code.AppendLine();
            // now the setter
            code.AppendLine($"           var operation = ed.EntityOperationCollection.Single(o => o.WebId == \"{WebId}\");");
            code.AppendLinf("           var rc = new RuleContext(item);");
            count = 0;
            foreach (var act in operation.SetterActionChildCollection)
            {
                count++;
                code.AppendLinf("           var setter{0} = operation.SetterActionChildCollection.Single(a => a.WebId == \"{1}\");", count, act.WebId);
                code.AppendLinf("           item.{1} = ({2})setter{0}.Field.GetValue(rc);", count, act.Path, ed.GetMember(act.Path).Type.FullName);
            }
            code.AppendLine($@"
            if(item.IsNewItem)item.Id = Guid.NewGuid().ToString();
        
            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""{Name}"");
            }}
            return Json(new {{success = true, message=""{SuccessMessage}"", status=""OK"", id = item.Id}});");

            code.AppendLine();
            code.AppendLine("       }");

            return new Method { Code = code.ToString() };

        }
    }
}