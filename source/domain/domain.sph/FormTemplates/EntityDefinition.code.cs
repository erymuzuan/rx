using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    public partial class EntityDefinition
    {
        public override int GetId()
        {
            return this.EntityDefinitionId;
        }

        private string GenerateCode()
        {
            var code = new StringBuilder();
            code.AppendLine("using " + typeof(Entity).Namespace + ";");
            code.AppendLine("using " + typeof(Int32).Namespace + ";");
            code.AppendLine("using " + typeof(Task<>).Namespace + ";");
            code.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            code.AppendLine("using " + typeof(XmlAttributeAttribute).Namespace + ";");
            code.AppendLine("using System.Web.Mvc;");
            code.AppendLine("using Bespoke.Sph.Web.Helpers;");
            code.AppendLine();

            code.AppendLine("namespace " + this.CodeNamespace);
            code.AppendLine("{");

            code.AppendLine("   public class " + this.Name + " : Entity");
            code.AppendLine("   {");

            code.AppendLinf("   private int m_{0}Id;", this.Name.ToCamelCase());
            code.AppendLinf("   public int {0}Id", this.Name);
            code.AppendLine("   {");
            code.AppendLinf("       get{{ return m_{0}Id;}}", this.Name.ToCamelCase());
            code.AppendLinf("       set{{ m_{0}Id = value;}}", this.Name.ToCamelCase());
            code.AppendLine("   }");

            code.AppendFormat(@"     
        public override string ToString()
        {{
            return ""{0}:"" + {1};
        }}", this.Name, this.RecordName);

            code.AppendFormat(@"     
        public override void SetId(int id)
        {{
            m_{0}Id = id;
        }}", this.Name.ToCamelCase());
            code.AppendFormat(@"     
        public override int GetId()
        {{
            return m_{0}Id;
        }}
", this.Name.ToCamelCase());

            // properties for each members
            foreach (var member in this.MemberCollection)
            {
                code.AppendLinf("//member:{0}", member.Name);
                code.AppendLine("       " + member.GeneratedCode());
            }


            code.AppendLine("   }");// end class
            // classes for members
            foreach (var member in this.MemberCollection)
            {
                code.AppendLinf("//class:{0}", member.Name);
                code.AppendLine(member.GeneratedCustomClass());
            }

            // search
            this.GenerateController(code);


            code.AppendLine("}");// end namespace
            return code.ToString();
        }

        public string CodeNamespace
        {
            get { return string.Format("Bespoke.{0}_{1}.Domain", ConfigurationManager.ApplicationName, this.EntityDefinitionId); }
        }


        public Task<string> GenerateCustomXsdJavascriptClassAsync()
        {
            var jsNamespace = ConfigurationManager.ApplicationName.ToCamelCase() + "_" + this.EntityDefinitionId;
            var assemblyName = ConfigurationManager.ApplicationName + "." + this.Name;
            var script = new StringBuilder();
            script.AppendLine("var bespoke = bespoke ||{};");
            script.AppendLinf("bespoke.{0} = bespoke.{0} ||{{}};", jsNamespace);
            script.AppendLinf("bespoke.{0}.domain = bespoke.{0}.domain ||{{}};", jsNamespace);

            script.AppendLinf("bespoke.{0}.domain.{1} = function(optionOrWebid){{", jsNamespace, this.Name);
            script.AppendLine(" var system = require('durandal/system'),");
            script.AppendLine(" model = {");
            script.AppendLinf("     $type : ko.observable(\"{0}.{1}, {2}\"),", this.CodeNamespace, this.Name, assemblyName);
            script.AppendLinf("     {0}Id : ko.observable(),", this.Name);
            foreach (var item in this.MemberCollection)
            {
                if (item.Type == typeof(Array))
                    script.AppendLinf("     {0}: ko.observableArray([]),", item.Name);
                else if (item.Type == typeof(object))
                    script.AppendLinf("     {0}: ko.observable(new bespoke.{1}.domain.{0}()),", item.Name, jsNamespace);
                else
                    script.AppendLinf("     {0}: ko.observable(),", item.Name);
            }
            script.AppendFormat(@"
    addChildItem : function(list, type){{
                        return function(){{                          
                            list.push(new type(system.guid()));
                        }}
                    }},
            
   removeChildItem : function(list, obj){{
                        return function(){{
                            list.remove(obj);
                        }}
                    }},
");
            script.AppendLine("     WebId: ko.observable()");

            script.AppendLine(" };");

            script.AppendLine(@" 
             if (optionOrWebid && typeof optionOrWebid === ""object"") {
                for (var n in optionOrWebid) {
                    if (typeof model[n] === ""function"") {
                        model[n](optionOrWebid[n]);
                    }
                }
            }
            if (optionOrWebid && typeof optionOrWebid === ""string"") {
                model.WebId(optionOrWebid);
            }");

            script.AppendFormat(@"

                if (bespoke.{0}.domain.{1}Partial) {{
                    return _(model).extend(new bespoke.{0}.domain.{1}Partial(model));
                }}", jsNamespace, this.Name);

            script.AppendLine(" return model;");
            script.AppendLine("};");
            foreach (var item in this.MemberCollection.Where(m => m.Type == typeof(object) || m.Type == typeof(Array)))
            {
                var code = item.GenerateJavascriptClass(jsNamespace, this.CodeNamespace, assemblyName);
                script.AppendLine(code);
            }
            return Task.FromResult(script.ToString());
        }

        private void GenerateController(StringBuilder code)
        {
            code.AppendLinf("public partial class {0}Controller : System.Web.Mvc.Controller", this.Name);
            code.AppendLine("{");
            code.AppendLinf("//exec:Search");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Search()");
            code.AppendLine("       {");
            code.AppendFormat(@"
            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var url = ""{1}/{0}/_search"";

            using(var client = new System.Net.Http.HttpClient())
            {{
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.PostAsync(url, request);
                var content = response.Content as System.Net.Http.StreamContent;
                if (null == content) throw new Exception(""Cannot execute query on es "" + request);
                this.Response.ContentType = ""application/json; charset=utf-8"";
                return Content(await content.ReadAsStringAsync());
            }}
            ", this.Name.ToLower(), ConfigurationManager.ApplicationName.ToLower());
            code.AppendLine("       }");

            // SAVE
            code.AppendLinf("//exec:Save");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Save([RequestBody]{0} item)", this.Name);
            code.AppendLine("       {");
            code.AppendLinf(@"
            if(null == item) throw new ArgumentNullException(""item"");
            var context = new Bespoke.Sph.Domain.SphDataContext();
            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""save"");
            }}
            this.Response.ContentType = ""application/json; charset=utf-8"";
            return Json(new {{success = true, status=""OK"", id = item.{0}Id}});", this.Name);
            code.AppendLine("       }");

            //OPERATIONS
            this.AppendOperationsCode(code);

            // Validations
            this.AppendValidationCode(code);

            // REMOVE
            code.AppendLinf("//exec:Remove");
            code.AppendLinf("       [HttpDelete]");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Remove(int id)");
            code.AppendLine("       {");
            code.AppendLinf(@"
            var repos = ObjectBuilder.GetObject<IRepository<{0}>>();
            var item = await repos.LoadOneAsync(id);
            if(null == item)
                return new HttpNotFoundResult();

            var context = new Bespoke.Sph.Domain.SphDataContext();
            using(var session = context.OpenSession())
            {{
                session.Delete(item);
                await session.SubmitChanges(""delete"");
            }}
            this.Response.ContentType = ""application/json; charset=utf-8"";
            return Json(new {{success = true, status=""OK"", id = item.{0}Id}});", this.Name);
            code.AppendLine("       }");

            //SCHEMAS

            code.AppendLinf("//exec:Schemas");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Schemas()");
            code.AppendLine("       {");
            code.AppendLine("           var context = new SphDataContext();");
            code.AppendLinf("           var ed = await context.LoadOneAsync<EntityDefinition>(t => t.Name == \"{0}\");", this.Name);

            code.AppendLine("           var script = await ed.GenerateCustomXsdJavascriptClassAsync();");
            code.AppendLine("           this.Response.ContentType = \"application/javascript\";");

            code.AppendLine("           return Content(script);");
            code.AppendLine("       }");

            code.AppendLine("}");

        }

        private void AppendOperationsCode(StringBuilder code)
        {
            foreach (var operation in this.EntityOperationCollection)
            {
                var everybody = operation.Permissions.Contains("Everybody");
                var anonymous = operation.Permissions.Contains("Anonymous");
                // SAVE
                code.AppendLinf("//exec:{0}", operation.Name);
                code.AppendLine("       [HttpPost]");
                if (everybody)
                    code.AppendLine("       [Authorize]");

                if (!everybody && !anonymous && string.Join(",", operation.Permissions.Where(s => s != "Everybody" && s != "Anonymous")).Length > 0)
                    code.AppendLinf("       [Authorize(Roles=\"{0}\")]", string.Join(",", operation.Permissions.Where(s => s != "Everybody" && s != "Anonymous")));

                code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> {0}([RequestBody]{1} item)", operation.Name, this.Name);
                code.AppendLine("       {");
                code.AppendLine("           var context = new Bespoke.Sph.Domain.SphDataContext();");
                code.AppendLine("           if(null == item) throw new ArgumentNullException(\"item\");");
                code.AppendLinf("           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == \"{0}\");", this.Name);

                code.AppendLine("           var brokenRules = new ObjectCollection<ValidationResult>();");
                var count = 0;
                foreach (var rule in operation.Rules)
                {
                    count++;
                    code.AppendFormat(@"
            var appliedRules{1} = ed.BusinessRuleCollection.Where(b => b.Name == ""{0}"");
            ValidationResult result{1} = item.ValidateBusinessRule(appliedRules{1});

            if(!result{1}.Success){{
                brokenRules.Add(result{1});
            }}
", rule, count);
                }
                code.AppendLine("           if( brokenRules.Count > 0) return Json(new {success = false, rules = brokenRules.ToArray()});");

                code.AppendLine();
                // now the setter
                code.AppendLinf("           var operation = ed.EntityOperationCollection.Single(o => o.WebId == \"{0}\");", operation.WebId);
                code.AppendLinf("           var rc = new RuleContext(item);");
                count = 0;
                foreach (var act in operation.SetterActionChildCollection)
                {
                    count++;
                    code.AppendLinf("           var setter{0} = operation.SetterActionChildCollection.Single(a => a.WebId == \"{1}\");", count, act.WebId);
                    code.AppendLinf("           item.{1} = ({2})setter{0}.Field.GetValue(rc);", count, act.Path, this.GetMember(act.Path).Type.FullName);
                }
                code.AppendFormat(@"           
            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""{1}"");
            }}
            return Json(new {{success = true, status=""OK"", id = item.{0}Id}});", this.Name, operation.Name);

                code.AppendLine();
                code.AppendLine("       }");
            }
        }


        private void AppendValidationCode(StringBuilder code)
        {
            // validates
            code.AppendLinf("//exec:validate");
            code.AppendLine("       [HttpPost]");

            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Validate(string id,[RequestBody]{0} item)", this.Name);
            code.AppendLine("       {");
            code.AppendLine("           var context = new Bespoke.Sph.Domain.SphDataContext();");
            code.AppendLine("           if(null == item) throw new ArgumentNullException(\"item\");");
            code.AppendLinf("           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == \"{0}\");", this.Name);

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
            return Json(new {{success = true, status=""OK"", id = item.{0}Id}});", this.Name);

            code.AppendLine();
            code.AppendLine("       }");

        }
    }
}
