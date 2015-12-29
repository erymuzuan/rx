using System;
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

        public Method GeneratePostAction(EntityDefinition ed)
        {
            if (!IsHttpPost) return null;
            var route = this.Route ?? this.Name;
            var post = new Method { Name = $"Post{Name}", ReturnTypeName = "Task<ActionResult>", AccessModifier = Modifier.Public };
            post.AttributeCollection.Add("[HttpPost]");
            post.AttributeCollection.Add($"[Route(\"{route.ToLowerInvariant()}\")]");

            var authorize = GenerateAuthorizeAttribute();
            if (!string.IsNullOrWhiteSpace(authorize))
                post.AttributeCollection.Add(authorize);


            var body = new MethodArg { Name = "item", TypeName = ed.Name };
            body.AttributeCollection.Add("[RequestBody]");
            post.ArgumentCollection.Add(body);


            post.AppendLine("           var context = new SphDataContext();");
            if (this.Rules.Any() || this.SetterActionChildCollection.Any())
                post.AppendLine(GetEntityDefinitionCode(ed));

            post.AppendLine($"      item.Id = Strings.GenerateId(); ");

            var rules = GenerateRulesCode();
            if (!string.IsNullOrWhiteSpace(rules))
                post.AppendLine(rules);


            var setterCode = GetSetterCode(ed);
            if (!string.IsNullOrWhiteSpace(setterCode))
                post.AppendLine(setterCode);

            post.AppendLine($@"
        
            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""{Name}"");
            }}
            return Json(new {{success = true, message=""{SuccessMessage}"", status=""OK"", id = item.Id, href=""{ed.Name}/"" + item.Id}});");

            return post;
        }
        public Method GeneratePatchAction(EntityDefinition ed)
        {
            if (!IsHttpPatch) return null;

            var route = this.Route ?? this.Name;
            var patch = new Method { Name = $"Patch{Name}", ReturnTypeName = "Task<ActionResult>", AccessModifier = Modifier.Public };
            patch.AttributeCollection.Add("[HttpPatch]");
            patch.AttributeCollection.Add($"[Route(\"{route.ToLowerInvariant()}/{{id}}\")]");

            var authorize = GenerateAuthorizeAttribute();
            if (!string.IsNullOrWhiteSpace(authorize))
                patch.AttributeCollection.Add(authorize);


            patch.ArgumentCollection.Add(new MethodArg { Name = "id", Type = typeof(string) });
            var body = new MethodArg { Name = "body", Type = typeof(string) };
            body.AttributeCollection.Add("[RequestBody]");
            patch.ArgumentCollection.Add(body);


            patch.AppendLine("           var context = new SphDataContext();");
            if (this.Rules.Any() || this.SetterActionChildCollection.Any())
                patch.AppendLine(GetEntityDefinitionCode(ed));

            patch.AppendLine(
                $@"
            var repos = ObjectBuilder.GetObject<IRepository<{ed.Name}>>();
            var item = await repos.LoadOneAsync(id);
            if(null == item) return HttpNotFound(""Cannot find any {ed.Name} with Id "" + id);

            var jo = JObject.Parse(body);");

            foreach (var path in this.PatchPathCollection)
            {
                var member = ed.GetMember(path.Path);
                if (null == member) throw new InvalidOperationException($"Cannot find member with path {path}");
                patch.AppendLine($"            item.{path} = jo.SelectToken(\"$.{path}\").Value<{member.Type.ToCSharp()}>();");
            }
            patch.AppendLine();

            var rules = GenerateRulesCode();
            patch.AppendLine(rules);


            var setterCode = GetSetterCode(ed);
            if (!string.IsNullOrWhiteSpace(setterCode))
                patch.AppendLine(setterCode);

            patch.AppendLine($@"
        
            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""{Name}"");
            }}
            return Json(new {{success = true, message=""{SuccessMessage}"", status=""OK"", id = item.Id}});");

            return patch;
        }
        public Method GeneratePutAction(EntityDefinition ed)
        {
            if (!IsHttpPut) return null;

            var route = this.Route ?? this.Name;
            var put = new Method { Name = $"Put{Name}", ReturnTypeName = "Task<ActionResult>", AccessModifier = Modifier.Public };
            put.AttributeCollection.Add("[HttpPatch]");
            put.AttributeCollection.Add($"[Route(\"{route.ToLowerInvariant()}/{{id?}}\")]");

            var authorize = GenerateAuthorizeAttribute();
            if (!string.IsNullOrWhiteSpace(authorize))
                put.AttributeCollection.Add(authorize);


            var body = new MethodArg { Name = "body", Type = typeof(string) };
            body.AttributeCollection.Add("[RequestBody]");
            put.ArgumentCollection.Add(body);
            put.ArgumentCollection.Add(new MethodArg { Name = "id", Type = typeof(string), Default = "null" });


            put.AppendLine("           var context = new SphDataContext();");
            if (this.Rules.Any() || this.SetterActionChildCollection.Any())
                put.AppendLine(GetEntityDefinitionCode(ed));

            put.AppendLine(
                $@"
            var repos = ObjectBuilder.GetObject<IRepository<{ed.Name}>>();
            var item = await repos.LoadOneAsync(id);
            if(null == item)
            {{
                item = body.DeserializeFromJson<{ed.Name}>();
                if (!string.IsNullOrWhiteSpace(item.Id))
                    item.Id = id ?? System.Guid.NewGuid().ToString();
                this.Response.StatusCode = (int) System.Net.HttpStatusCode.Created;
            }}
            else
            {{
                this.Response.StatusCode = (int) System.Net.HttpStatusCode.OK;

                var jo = JObject.Parse(body);
            ");

            foreach (var path in this.PatchPathCollection)
            {
                var member = ed.GetMember(path.Path);
                if (null == member) throw new InvalidOperationException($"Cannot find member with path {path}");
                put.AppendLine($"            item.{path.Path} = jo.SelectToken(\"$.{path}\").Value<{member.Type.ToCSharp()}>();");
            }
            put.AppendLine(@"
            }");

            var rules = GenerateRulesCode();
            if (!string.IsNullOrWhiteSpace(rules))
                put.AppendLine(rules);


            var setterCode = GetSetterCode(ed);
            if (!string.IsNullOrWhiteSpace(setterCode))
                put.AppendLine(setterCode);

            put.AppendLine($@"
        
            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""{Name}"");
            }}
            return Json(new {{success = true, message=""{SuccessMessage}"", status=""OK"", id = item.Id, href=""{ed.Name}/"" + item.Id}});");

            return put;
        }

        private string GetSetterCode(EntityDefinition ed)
        {
            if (this.SetterActionChildCollection.Count == 0) return string.Empty;

            var code = new StringBuilder();
            // now the setter
            code.AppendLine($"           var operation = ed.EntityOperationCollection.Single(o => o.WebId == \"{WebId}\");");
            code.AppendLinf("           var rc = new RuleContext(item);");
            var count = 0;
            foreach (var act in this.SetterActionChildCollection)
            {
                count++;
                code.AppendLinf(
                    "           var setter{0} = operation.SetterActionChildCollection.Single(a => a.WebId == \"{1}\");", count,
                    act.WebId);
                code.AppendLinf("           item.{1} = ({2})setter{0}.Field.GetValue(rc);", count, act.Path,
                    ed.GetMember(act.Path).Type.FullName);
            }

            return code.ToString();

        }

        private string GetEntityDefinitionCode(EntityDefinition ed)
        {
            return $"           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Id == \"{ed.Id}\");";

        }
        private string GenerateRulesCode()
        {
            if (this.Rules.Count == 0) return string.Empty;

            var code = new StringBuilder();

            code.AppendLine("           var brokenRules = new ObjectCollection<ValidationResult>();");
            var count = 0;
            foreach (var rule in this.Rules)
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
            return code.ToString();
        }


        private string GenerateAuthorizeAttribute()
        {
            var code = new StringBuilder();
            var everybody = this.Permissions.Contains("Everybody");
            var anonymous = this.Permissions.Contains("Anonymous");
            if (everybody)
                code.AppendLine("       [Authorize]");

            if (!everybody && !anonymous &&
                string.Join(",", this.Permissions.Where(s => s != "Everybody" && s != "Anonymous")).Length > 0)
                code.AppendLinf("       [Authorize(Roles=\"{0}\")]",
                    string.Join(",", this.Permissions.Where(s => s != "Everybody" && s != "Anonymous")));
            return code.ToString();
        }

    }
}