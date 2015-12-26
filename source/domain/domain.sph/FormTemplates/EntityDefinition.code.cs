using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class EntityDefinition
    {
        private readonly string[] m_importDirectives =
        {
            typeof(Entity).Namespace,
            typeof(Int32).Namespace ,
            typeof(Task<>).Namespace,
            typeof(Enumerable).Namespace ,
            typeof(XmlAttributeAttribute).Namespace,
            "System.Web.Mvc",
            "Bespoke.Sph.Web.Helpers"
        };




        public IEnumerable<Class> GenerateCode()
        {

            var @class = new Class { Name = this.Name, FileName = $"{Name}.cs", Namespace = CodeNamespace, BaseClass = nameof(Entity) };
            @class.ImportCollection.AddRange(m_importDirectives);
            var list = new ObjectCollection<Class> { @class };

            if (this.TreatDataAsSource)
            {
                var es = this.StoreInElasticsearch ?? true ? "true" : "false";
                var db = this.StoreInDatabase ?? true ? "true" : "false";
                @class.AttributeCollection.Add($"  [StoreAsSource(IsElasticsearch={es}, IsSqlDatabase={db})]");
            }

            var ctor = new StringBuilder();
            // ctor
            ctor.AppendLine($"       public {Name}()");
            ctor.AppendLine("       {");
            ctor.AppendLinf("           var rc = new RuleContext(this);");
            var count = 0;
            foreach (var member in this.MemberCollection)
            {
                count++;
                var defaultValueCode = member.GetDefaultValueCode(count);
                if (!string.IsNullOrWhiteSpace(defaultValueCode))
                    ctor.AppendLine(defaultValueCode);
            }
            ctor.AppendLine("       }");
            @class.CtorCollection.Add(ctor.ToString());

            var toString = $@"     
        public override string ToString()
        {{
            return ""{Name}:"" + {RecordName};
        }}";
            @class.MethodCollection.Add(new Method { Code = toString });

            var properties = from m in this.MemberCollection
                             let prop = m.GeneratedCode("   ")
                             select new Property { Code = prop };
            @class.PropertyCollection.ClearAndAddRange(properties);

            // classes for members
            foreach (var member in this.MemberCollection)
            {
                var mc = member.GeneratedCustomClass(this.CodeNamespace, m_importDirectives);
                list.AddRange(mc);

            }

            var controller = this.GenerateController();
            list.Add(controller);


            return list;
        }


        public string[] SaveSources(IEnumerable<Class> classes)
        {
            var sources = classes.ToArray();
            var folder = Path.Combine(ConfigurationManager.GeneratedSourceDirectory, this.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            foreach (var cs in sources)
            {
                var file = Path.Combine(folder, cs.FileName);
                File.WriteAllText(file, cs.GetCode());
            }
            return sources
                    .Select(f => $"{ConfigurationManager.GeneratedSourceDirectory}\\{this.Name}\\{f.FileName}")
                    .ToArray();
        }
        public string CodeNamespace => $"Bespoke.{ConfigurationManager.ApplicationName}_{this.Id}.Domain";


        public Task<string> GenerateCustomXsdJavascriptClassAsync()
        {
            var jsNamespace = ConfigurationManager.ApplicationName + "_" + this.Id;
            var assemblyName = ConfigurationManager.ApplicationName + "." + this.Name;
            var script = new StringBuilder();
            script.AppendLine("var bespoke = bespoke ||{};");
            script.AppendLinf("bespoke.{0} = bespoke.{0} ||{{}};", jsNamespace);
            script.AppendLinf("bespoke.{0}.domain = bespoke.{0}.domain ||{{}};", jsNamespace);

            script.AppendLinf("bespoke.{0}.domain.{1} = function(optionOrWebid){{", jsNamespace, this.Name);
            script.AppendLine(" var system = require('services/system'),");
            script.AppendLine(" model = {");
            script.AppendLinf("     $type : ko.observable(\"{0}.{1}, {2}\"),", this.CodeNamespace, this.Name, assemblyName);
            script.AppendLine("     Id : ko.observable(\"0\"),");

            var members = from item in this.MemberCollection
                          let m = item.GenerateJavascriptMember(jsNamespace)
                          where !string.IsNullOrWhiteSpace(m)
                          select m;
            members.ToList().ForEach(m => script.AppendLine(m));

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

            var classes = from m in this.MemberCollection
                          let c = m.GenerateJavascriptClass(jsNamespace, CodeNamespace, assemblyName)
                          where !string.IsNullOrWhiteSpace(c)
                          select c;
            classes.ToList().ForEach(x => script.AppendLine(x));


            return Task.FromResult(script.ToString());
        }

        private Class GenerateController()
        {
            var controller = new Class
            {
                Name = $"{Name}Controller",
                IsPartial = true,
                FileName = $"{Name}Controller.cs",
                BaseClass = "System.Web.Mvc.Controller",
                Namespace = CodeNamespace
            };
            controller.ImportCollection.ClearAndAddRange(m_importDirectives);
            controller.ImportCollection.Add("Newtonsoft.Json.Linq");
            controller.AttributeCollection.Add($"[RoutePrefix(\"{Name}\")]");

            var search = GenerateSearchAction();
            controller.MethodCollection.Add(search);
            

            var posts = this.EntityOperationCollection.Where(x => x.IsHttpPost).Select(x => x.GeneratePostAction(this));
            controller.MethodCollection.AddRange(posts);

            var patches = this.EntityOperationCollection.Where(x => x.IsHttpPatch).Select(x => x.GeneratePatchAction(this));
            controller.MethodCollection.AddRange(patches);

            var puts = this.EntityOperationCollection.Where(x => x.IsHttpPut).Select(x => x.GeneratePatchAction(this));
            controller.MethodCollection.AddRange(puts);

            controller.MethodCollection.Add(GenerateValidationAction());

            var remove = GenerateRemoveAction();
            controller.MethodCollection.Add(remove);

            //SCHEMAS
            var schema = GenerateSchemaAction();
            controller.MethodCollection.Add(schema);

            return controller;


        }

        private Method GenerateSchemaAction()
        {
            var schema = new StringBuilder();
            schema.AppendLinf("       //exec:Schemas");
            schema.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Schemas()");
            schema.AppendLine("       {");
            schema.AppendLine("           var context = new SphDataContext();");
            schema.AppendLinf("           var ed = await context.LoadOneAsync<EntityDefinition>(t => t.Name == \"{0}\");",
                this.Name);

            schema.AppendLine("           var script = await ed.GenerateCustomXsdJavascriptClassAsync();");
            schema.AppendLine("           this.Response.ContentType = \"application/javascript\";");

            schema.AppendLine("           return Content(script);");
            schema.AppendLine("       }");
            return new Method { Code = schema.ToString() };
        }

        private Method GenerateSearchAction()
        {
            var search = new StringBuilder();

            search.AppendLinf("       [Route(\"search\")]");
            search.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Search()");
            search.AppendLine("       {");
            search.AppendFormat(@"
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
            search.AppendLine();
            search.AppendLine("       }");
            search.AppendLine();
            return new Method { Code = search.ToString() };
        }
        
        private Method GenerateRemoveAction()
        {
            // REMOVE
            var remove = new StringBuilder();
            remove.AppendLinf("       //exec:Remove");
            remove.AppendLinf("       [HttpDelete]");
            remove.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Remove(string id)");
            remove.AppendLine("       {");
            remove.AppendLinf(@"
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
            return Json(new {{success = true, status=""OK"", id = item.Id}});", this.Name);
            remove.AppendLine("       }");
            return new Method { Code = remove.ToString() };
        }


        private Method GenerateValidationAction()
        {
            var code = new StringBuilder();
            // validates
            code.AppendLinf("       //exec:validate");
            code.AppendLine("       [HttpPost]");

            code.AppendLine($"       public async Task<System.Web.Mvc.ActionResult> Validate(string id,[RequestBody]{Name} item)");
            code.AppendLine("       {");
            code.AppendLine("           var context = new Bespoke.Sph.Domain.SphDataContext();");
            code.AppendLine("           if(null == item) throw new ArgumentNullException(\"item\");");
            code.AppendLine($"           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Id == \"{Id}\");");

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

        public string GetElasticsearchMapping()
        {
            var map = new StringBuilder();
            map.AppendLine("{");
            map.AppendLine($"    \"{Name.ToLowerInvariant()}\":{{");
            map.AppendLine("        \"properties\":{");
            // add entity default properties
            map.AppendLine("            \"CreatedBy\": {\"type\": \"string\", \"index\":\"not_analyzed\"},");
            map.AppendLine("            \"ChangedBy\": {\"type\": \"string\", \"index\":\"not_analyzed\"},");
            map.AppendLine("            \"WebId\": {\"type\": \"string\", \"index\":\"not_analyzed\"},");
            map.AppendLine("            \"CreatedDate\": {\"type\": \"date\"},");
            map.AppendLine("            \"ChangedDate\": {\"type\": \"date\"},");

            var memberMappings = string.Join(",\r\n", this.MemberCollection.Select(d => d.GetEsMapping()));
            map.AppendLine(memberMappings);

            map.AppendLine("        }");
            map.AppendLine("    }");
            map.AppendLine("}");
            return map.ToString();
        }
    }
}
