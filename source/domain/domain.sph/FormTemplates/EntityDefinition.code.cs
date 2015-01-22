using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;
using Humanizer;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class EntityDefinition : IProjectProvider, IProjectModel
    {

        public string DefaultNamespace
        {
            get { return string.Format("Bespoke.{0}_{1}.Domain", ConfigurationManager.ApplicationName, this.Id); }
        }

        [JsonIgnore]
        public MetadataReference[] References
        {
            get
            {
                var references = new List<MetadataReference>
                {
                    this.CreateMetadataReference<System.Net.WebClient>(),
                    this.CreateMetadataReference<System.Net.Mail.SmtpClient>(),
                    this.CreateMetadataReference<System.Xml.Serialization.XmlAttributes>(),
                    this.CreateMetadataReference<object>(),
                    this.CreateMetadataReference<WorkflowDefinition>(),
                    this.CreateMetadataReference<EnumerableQuery>()
                };


                return references.ToArray();

            }
        }

        public IEnumerable<Class> GenerateCode()
        {
            var @class = new Class { Name = this.Name, Namespace = this.DefaultNamespace, BaseClass = "Entity" };
            @class.AddNamespaceImport<DateTime>();
            @class.AddNamespaceImport<Entity>();

            var ctor = GenerateConstructorCode();

            var toStringMethod = string.Format(@"     
        public override string ToString()
        {{
            return ""{0}:"" + {1};
        }}", this.Name, this.RecordName);

            @class.PropertyCollection.AddRange(this.MemberCollection.Select(x => x.CreateProperty()));
            @class.MethodCollection.Add(new Method { Code = toStringMethod, Name = "ToString" });
            @class.CtorCollection.Add(ctor.ToString());


            var sourceCodes = new List<Class> { @class };
            var customMembers = this.MemberCollection.Where(m => m.IsComplex)
                .SelectMany(member => member.GeneratedCustomClass());
            sourceCodes.AddRange(customMembers);

            var controller = this.GenerateController();
            sourceCodes.Add(controller);

            sourceCodes.ForEach(x => x.Namespace = DefaultNamespace);


            return sourceCodes;
        }

        public Task<IProjectModel> GetModelAsync()
        {
            return Task.FromResult((IProjectModel)this);
        }

        private StringBuilder GenerateConstructorCode()
        {
            var ctor = new StringBuilder();
            // ctor
            ctor.AppendLine("       public " + this.Name + "()");
            ctor.AppendLine("       {");
            ctor.AppendLine("           var item = this;");
            ctor.AppendLinf("           var rc = new RuleContext(item);");
            foreach (var mb in this.MemberCollection)
            {
                var inferred = mb.InferredType;
                if (!string.IsNullOrWhiteSpace(inferred))
                {
                    ctor.AppendLinf("           this.{0} = new {1}();", mb.Name, inferred);
                }
                if (null == mb.DefaultValue) continue;

                var defaultValueExpression = mb.DefaultValue.GetCSharpExpression();
                ctor.AppendLinf("           this.{0} = {1};", mb.Name, defaultValueExpression);
            }
            ctor.AppendLine("       }");
            return ctor;
        }


        public string[] SaveSources(IEnumerable<Class> sources)
        {
            var folder = Path.Combine(ConfigurationManager.UserSourceDirectory, this.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return sources.Select(x => x.Save(folder)).ToArray();

        }


        private Class GenerateController()
        {
            var @class = new Class { Name = this.Name + "Controller", Namespace = DefaultNamespace, BaseClass = "System.Web.Mvc.Controller" };
            @class.AddNamespaceImport<string>();
            @class.AddNamespaceImport<DomainObject>();
            @class.AddNamespaceImport<Task>();
            @class.ImportCollection.Add("System.Web.Mvc");
            @class.ImportCollection.Add("System.Linq");
            @class.ImportCollection.Add("Bespoke.Sph.Web.Helpers");


            var search = GenerateControllerSearchMethod();
            @class.MethodCollection.Add(search);

            var save = GenerateControllerSaveAction();
            @class.MethodCollection.Add(save);

            @class.MethodCollection.AddRange(this.GenerateControlerOperationMethods());
            @class.MethodCollection.Add(this.GenerateControllerValidateMethod());

            // REMOVE
            var remove = GenerateControllerRemoveAction();
            @class.MethodCollection.Add(remove);


            var schemas = GenerateControllerSchemasMethod();
            @class.MethodCollection.Add(schemas);


            return @class;


        }

        private Method GenerateControllerSearchMethod()
        {
            var search = new StringBuilder();
            search.AppendLinf("       //exec:Search");
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
            return new Method { Name = "Search", Code = search.ToString() };
        }

        private Method GenerateControllerSaveAction()
        {
            var save = new StringBuilder();
            save.AppendLinf("       //exec:Save");
            save.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Save([RequestBody]{0} item)", this.Name);
            save.AppendLine("       {");
            save.AppendLinf(@"
            if(null == item) throw new ArgumentNullException(""item"");
            var context = new Bespoke.Sph.Domain.SphDataContext();
            if(item.IsNewItem)item.Id = Guid.NewGuid().ToString();

            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""save"");
            }}
            this.Response.ContentType = ""application/json; charset=utf-8"";
            return Json(new {{success = true, status=""OK"", id = item.Id, href = ""{1}/"" + item.Id}});", this.Name,
                this.Name.ToLowerInvariant());
            save.AppendLine("       }");
            return new Method { Name = "Save", Code = save.ToString() };
        }

        private Method GenerateControllerRemoveAction()
        {
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
            return new Method { Name = "Remove", Code = remove.ToString() };
        }

        private Method GenerateControllerSchemasMethod()
        {
            var schemas = new StringBuilder();
            schemas.AppendLinf("       //exec:Schemas");
            schemas.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Schemas()");
            schemas.AppendLine("       {");
            schemas.AppendLine("           var context = new SphDataContext();");
            schemas.AppendLinf("           var ed = await context.LoadOneAsync<EntityDefinition>(t => t.Name == \"{0}\");",
                this.Name);

            schemas.AppendLine("           var script = await ed.GenerateCustomXsdJavascriptClassAsync();");
            schemas.AppendLine("           this.Response.ContentType = \"application/javascript\";");

            schemas.AppendLine("           return Content(script);");
            schemas.AppendLine("       }");
            return new Method { Name = "Schemas", Code = schemas.ToString() };
        }

        private IEnumerable<Method> GenerateControlerOperationMethods()
        {
            var methods = new List<Method>();
            foreach (var operation in this.EntityOperationCollection)
            {
                var code = new StringBuilder();
                var everybody = operation.Permissions.Contains("Everybody");
                var anonymous = operation.Permissions.Contains("Anonymous");
                // SAVE
                code.AppendLinf("       //exec:{0}", operation.Name);
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
            if(item.IsNewItem)item.Id = Guid.NewGuid().ToString();
        
            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""{1}"");
            }}
            return Json(new {{success = true, message=""{2}"", status=""OK"", id = item.Id}});", this.Name, operation.Name, operation.SuccessMessage);

                code.AppendLine();
                code.AppendLine("       }");


                methods.Add(new Method { Name = operation.Name, Code = code.ToString() });
            }


            return methods;
        }


        private Method GenerateControllerValidateMethod()
        {
            var code = new StringBuilder();
            code.AppendLinf("       //exec:validate");
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
            return Json(new {{success = true, status=""OK"", id = item.Id}});");

            code.AppendLine();
            code.AppendLine("       }");

            return new Method { Name = "Validate", Code = code.ToString() };

        }

        public IEnumerable<Member> Members { get { return this.MemberCollection; } }
    }
}
