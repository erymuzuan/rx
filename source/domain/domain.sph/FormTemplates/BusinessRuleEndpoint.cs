using System.Text;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class BusinessRuleEndpoint : DomainObject
    {
        public Method GenerateValidationAction(EntityDefinition ed)
        {
            var code = new StringBuilder();
            // validates
            code.AppendLinf("       //exec:validate");
            code.AppendLine("       [HttpPost]");

            code.AppendLine($"       public async Task<System.Web.Mvc.ActionResult> Validate(string id,[RequestBody]{ed.Name} item)");
            code.AppendLine("       {");
            code.AppendLine("           var context = new Bespoke.Sph.Domain.SphDataContext();");
            code.AppendLine("           if(null == item) throw new ArgumentNullException(\"item\");");
            code.AppendLine($"           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Id == \"{ed.Id}\");");

            code.AppendLine("           var brokenRules = new ObjectCollection<ValidationResult>();");
            code.AppendLine("           var rules = id.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);");

            code.AppendFormat(@"
           foreach(var r in rules)
           {{
                var appliedRules = ed.BusinessRuleCollection.Where(b => b.Name == r);
                ValidationResult result = item.ValidateBusinessRule(appliedRules);

                if(!result.Success){{
                    brokenRules.Add(result);
                }}
           }}
");
            code.AppendLine("           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});");

            code.AppendLine();

            code.AppendFormat(@"   
            return Json(new {{success = true, status=""OK"", id = item.Id}});");

            code.AppendLine();
            code.AppendLine("       }");

            return new Method { Name = "Validate", Code = code.ToString() };

        }
    }
}