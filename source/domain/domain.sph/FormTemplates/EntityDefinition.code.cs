using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class EntityDefinition : IProjectProvider, IProjectModel
    {

        public override string DefaultNamespace
        {
            get { return string.Format("Bespoke.{0}_{1}.Domain", ConfigurationManager.ApplicationName, this.Id); }
        }

        [JsonIgnore]
        public override MetadataReference[] References
        {
            get
            {
                const string SYSTEM_WEB = "System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
                var mvc = Path.Combine(ConfigurationManager.WebPath, @"bin\System.Web.Http.dll");
                var references = new List<MetadataReference>
                {
                    this.CreateMetadataReference<System.Net.WebClient>(),
                    this.CreateMetadataReference<System.Net.Mail.SmtpClient>(),
                    this.CreateMetadataReference<System.Xml.Serialization.XmlAttributes>(),
                    this.CreateMetadataReference<object>(),
                    this.CreateMetadataReference<WorkflowDefinition>(),
                    this.CreateMetadataReference<Task>(),
                    this.CreateMetadataReference<Newtonsoft.Json.Converters.DataTableConverter>(),
                    this.CreateMetadataReference<System.Net.Http.HttpClient>(),
                    this.CreateMetadataReference<EnumerableQuery>(),
                    this.CreateMetadataFromFile(mvc),
                    MetadataReference.CreateFromAssembly(Assembly.Load(SYSTEM_WEB))
                };
                return references.ToArray();

            }
        }

        public override Task<IEnumerable<Class>> GenerateCodeAsync()
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


            return Task.FromResult(sourceCodes.AsEnumerable());
        }

        public override Task<IProjectModel> GetModelAsync()
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
                if (!string.IsNullOrWhiteSpace(mb.InferredType) && !mb.InferredType.Contains("ObjectCollection`1"))
                {
                    ctor.AppendLinf("           this.{0} = new {1}();", mb.Name, mb.InferredType);
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
            var @class = new Class { Name = this.Name + "Controller", Namespace = DefaultNamespace, BaseClass = "System.Web.Http.ApiController" };
            @class.AttributeCollection.Add(string.Format("[RoutePrefix(\"{0}\")]", this.Id));
            @class.AddNamespaceImport<string>();
            @class.AddNamespaceImport<DomainObject>();
            @class.AddNamespaceImport<Task>();
            @class.ImportCollection.Add("System.Web.Http");
            @class.ImportCollection.Add("System.Net");
            @class.ImportCollection.Add("System.Net.Http");
            @class.ImportCollection.Add("System.Linq");
            @class.ImportCollection.Add("System.IO");


            var search = GenerateControllerSearchMethod();
            @class.MethodCollection.Add(search);

            var save = GenerateControllerSaveAction();
            @class.MethodCollection.Add(save);

            @class.MethodCollection.AddRange(this.GenerateControlerOperationMethods());
            @class.MethodCollection.Add(this.GenerateControllerValidateMethod());

            // REMOVE
            var remove = GenerateControllerRemoveAction();
            @class.MethodCollection.Add(remove);



            return @class;


        }


        private Method GenerateControllerSearchMethod()
        {
            var search = new StringBuilder();
            search.AppendLinf("       //exec:Search");
            search.AppendLinf("       [Route(\"search\")]");
            search.AppendLinf("       public async Task<HttpResponseMessage> Search([FromBody]string json)");
            search.AppendLine("       {");
            search.AppendFormat(@"
            var request = new System.Net.Http.StringContent(json);
            var url = ""{1}/{0}/_search"";

            using(var client = new System.Net.Http.HttpClient())
            {{
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var es = await client.PostAsync(url, request);
                var content = es.Content as System.Net.Http.StreamContent;
                if (null == content) throw new Exception(""Cannot execute query on es "" + request);

                var result = await content.ReadAsStringAsync();
    
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(result, System.Text.Encoding.Default, ""application/json"");
                return response;
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
            save.AppendLinf("       [HttpPost]");
            save.AppendLinf("       [Route(\"\")]");
            save.AppendLinf("       public async Task<HttpResponseMessage> Save([FromBody]{0} item)", this.Name);
            save.AppendLine("       {");
            save.AppendFormat(@"
            if(null == item) throw new ArgumentNullException(""item"");
            var context = new Bespoke.Sph.Domain.SphDataContext();
            if(item.IsNewItem)
                item.Id = Guid.NewGuid().ToString();

            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""save"");
            }}
            
            var result = new {{success = true, status=""OK"", id = item.Id, href = ""{0}/"" + item.Id}};
            var  response = Request.CreateResponse(HttpStatusCode.Accepted, result );
            return response;", this.Id);
            save.AppendLine();
            save.AppendLine("       }");
            return new Method { Name = "Save", Code = save.ToString() };
        }

        private Method GenerateControllerRemoveAction()
        {
            var remove = new StringBuilder();
            remove.AppendLinf("       //exec:Remove");
            remove.AppendLinf("       [HttpDelete]");
            remove.AppendLine("       [Route(\"{id}\")]");
            remove.AppendLinf("       public async Task<HttpResponseMessage> Remove(string id)");
            remove.AppendLine("       {");
            remove.AppendLinf(@"
            var repos = ObjectBuilder.GetObject<IRepository<{0}>>();
            var item = await repos.LoadOneAsync(id);
            if(null == item)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var context = new Bespoke.Sph.Domain.SphDataContext();
            using(var session = context.OpenSession())
            {{
                session.Delete(item);
                await session.SubmitChanges(""delete"");
            }}
            var  response = Request.CreateResponse(HttpStatusCode.Accepted,new {{success = true, status=""OK""}} );
            return response;", this.Name);
            remove.AppendLine("       }");
            return new Method { Name = "Remove", Code = remove.ToString() };
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

                code.AppendLinf("       [Route(\"{0}\")]", operation.Name);
                code.AppendLinf("       public async Task<HttpResponseMessage> {0}([FromBody]{1} item)", operation.Name, this.Name);
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
                code.AppendLine("           if( brokenRules.Count > 0) Request.CreateResponse(HttpStatusCode.OK, new {success = false, rules = brokenRules.ToArray()});");

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
                await session.SubmitChanges(""{0}"");
            }}

            var  response = Request.CreateResponse(HttpStatusCode.OK,new {{success = true, status=""OK"", message=""{1}"" ,id = item.Id}} );
            return response;", operation.Name, operation.SuccessMessage);

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
            code.AppendLine("       [Route(\"validate/{rules}\")]");

            code.AppendLinf("       public async Task<HttpResponseMessage> Validate([FromBody]{0} item, string rules)", this.Name);
            code.AppendLine("       {");
            code.AppendLine("           var context = new Bespoke.Sph.Domain.SphDataContext();");
            code.AppendLine("           if(null == item) throw new ArgumentNullException(\"item\");");
            code.AppendLinf("           var ed = await context.LoadOneAsync<EntityDefinition>(d => d.Name == \"{0}\");", this.Name);

            code.AppendLine("           var brokenRules = new ObjectCollection<ValidationResult>();");
            code.AppendLine("           var ruleNames = rules.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);");

            code.AppendFormat(@"
           foreach(var r in ruleNames)
           {{
                var appliedRules = ed.BusinessRuleCollection.Where(b => b.Name == r);
                ValidationResult br = item.ValidateBusinessRule(appliedRules);

                if(!br.Success){{
                    brokenRules.Add(br);
                }}
           }}
");
            code.AppendLine();
            code.AppendFormat(@"   
            var result = new {{ success = brokenRules.Count > 0, id = item.Id, status=""OK"", rules = brokenRules.ToArray()}};
            var  response = Request.CreateResponse( HttpStatusCode.Accepted, result);
            return response;");

            code.AppendLine();
            code.AppendLine("       }");

            return new Method { Name = "Validate", Code = code.ToString() };

        }

        public IEnumerable<Member> Members { get { return this.MemberCollection; } }
    }
}
