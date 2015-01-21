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
    public partial class WorkflowDefinition : IProjectProvider
    {
        public string DefaultNamespace
        {
            get { return this.CodeNamespace; }
        }

        string IProjectProvider.Name
        {
            get { return this.WorkflowTypeName; }
        }


        [JsonIgnore]
        public MetadataReference[] References
        {
            get
            {
                var clrTypes = from v in this.ReferencedAssemblyCollection
                               let location = Path.Combine(ConfigurationManager.WebPath, @"bin\" + Path.GetFileName(v.Location))
                               select MetadataReference.CreateFromAssembly(Assembly.LoadFile(location));
                var references = clrTypes.ToList();
                references.AddMetadataReference<System.Net.WebClient>()
                    .AddMetadataReference<System.Xml.Serialization.XmlAnyAttributeAttribute>()
                    //.AddMetadataReference<object>()
                    .AddMetadataReference<WorkflowDefinition>()
                    .AddMetadataReference<EnumerableQuery>()
                    .AddMetadataReference<System.Net.Mail.SmtpClient>();
                references.Add(MetadataReference.CreateFromAssembly(Assembly.Load("mscorlib")));

                return references.ToArray();
            }
        }

        public IEnumerable<Class> GenerateCode()
        {
            var @classes = new List<Class>();
            var wcd = new Class
            {
                Name = this.WorkflowTypeName,
                Namespace = this.CodeNamespace,
                BaseClass = typeof(Workflow).FullName,
                FileName = this.WorkflowTypeName + ".cs",
                IsPartial = true
            };
            @classes.Add(wcd);
            wcd.ImportCollection.Add(typeof(Entity).Namespace);
            wcd.ImportCollection.Add(typeof(Int32).Namespace);
            wcd.ImportCollection.Add(typeof(Task<>).Namespace);
            wcd.ImportCollection.Add(typeof(Enumerable).Namespace);
            wcd.ImportCollection.Add(typeof(System.Xml.Serialization.XmlAttributeAttribute).Namespace);
            wcd.AttributeCollection.Add("   [EntityType(typeof(Workflow))]");



            var ctor = new StringBuilder();
            // contructor
            ctor.AppendLine("       public " + this.WorkflowTypeName + "()");
            ctor.AppendLine("       {");

            // default properties
            ctor.AppendLinf("           this.Name = \"{0}\";", this.Name);
            ctor.AppendLinf("           this.Version = {0};", this.Version);
            ctor.AppendLinf("           this.WorkflowDefinitionId = \"{0}\";", this.Id);

            foreach (var variable in this.VariableDefinitionCollection.OfType<ComplexVariable>())
            {
                ctor.AppendLinf("           this.{0} = new {1}();", variable.Name, variable.TypeName);
            }
            foreach (var variable in this.VariableDefinitionCollection.OfType<SimpleVariable>().Where(v => !string.IsNullOrWhiteSpace(v.DefaultValue)))
            {
                if (variable.Type == typeof(string))
                    ctor.AppendLinf("           this.{0} = \"{1}\";", variable.Name, variable.DefaultValue);
                if (variable.Type == typeof(int))
                    ctor.AppendLinf("           this.{0} = {1};", variable.Name, variable.DefaultValue);
                if (variable.Type == typeof(decimal))
                    ctor.AppendLinf("           this.{0} = {1};", variable.Name, variable.DefaultValue);
                if (variable.Type == typeof(bool))
                    ctor.AppendLinf("           this.{0} = {1};", variable.Name, variable.DefaultValue);
                if (variable.Type == typeof(DateTime))
                    ctor.AppendLinf("           this.{0} = DateTime.Parse(\"{1}\");", variable.Name, variable.DefaultValue);
            }
            ctor.AppendLine("       }");// end contructor

            wcd.CtorCollection.Add(ctor.ToString());


            var start = new StringBuilder();
            // start
            start.AppendLine("       public override async Task<ActivityExecutionResult> StartAsync()");
            start.AppendLine("       {");
            start.AppendLinf("           this.SerializedDefinitionStoreId = \"wd.{0}.{1}\";", this.Id, this.Version);
            start.AppendLinf("           var result =await this.{0}().ConfigureAwait(false);", this.GetInitiatorActivity().MethodName);
            start.AppendLinf("           return result;");
            start.AppendLine("       }");
            wcd.MethodCollection.Add(new Method { Code = start.ToString() });

            // execute
            var execute = new StringBuilder();
            execute.AppendLine("       public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)");
            execute.AppendLine("       {");
            execute.AppendLinf("           this.SerializedDefinitionStoreId = \"wd.{0}.{1}\";", this.Id, this.Version);
            execute.AppendLine("           ActivityExecutionResult result = null;");
            execute.AppendLine("           switch(activityId)");
            execute.AppendLine("           {");

            foreach (var activity in this.ActivityCollection)
            {
                execute.AppendLinf("                   case \"{0}\" : ", activity.WebId);

                if (!string.IsNullOrEmpty(activity.TryScope))
                {
                    var ts = this.TryScopeCollection.FirstOrDefault(sc => sc.Id == activity.TryScope);
                    if (null == ts) throw new InvalidOperationException("Failed to find scope dfined for this activity " + activity.TryScope);
                    execute.AppendLine(ts.GenerateCode(this, activity));
                }
                else
                {
                    execute.AppendLinf("                       result = await this.{0}().ConfigureAwait(false);", activity.MethodName);
                }

                execute.AppendLine("                       break;");

            }
            execute.AppendLine("           }");// end switch
            execute.AppendLine("           result.Correlation = correlation;");
            execute.AppendLine("           await this.SaveAsync(activityId, result).ConfigureAwait(false);");
            execute.AppendLinf("           return result;");
            execute.AppendLine("       }");
            wcd.MethodCollection.Add(new Method { Code = execute.ToString() });


            // properties for each Variables
            foreach (var variable in this.VariableDefinitionCollection)
            {
                //code.AppendLinf("//variable:{0}", variable.Name);
                wcd.PropertyCollection.Add(new Property { Code = "       " + variable.GeneratedCode(this) });
            }

            // activities method
            foreach (var activity in this.ActivityCollection)
            {
                activity.BeforeGenerate(this);
            }

            foreach (var activity in this.ActivityCollection)
            {
                if (activity.IsAsync)
                    wcd.MethodCollection.Add(new Method { Code = activity.GenerateInitAsyncMethod(this) });


                wcd.MethodCollection.Add(new Method
                {
                    Code = this.SanitizeMethodBody(activity),
                    Comment = "//exec:" + activity.WebId
                });
                if (activity.OtherMethodCollection.Any())
                {
                    var actPartial = new Class
                    {
                        IsPartial = true,
                        Name = this.WorkflowTypeName,
                        Namespace = this.CodeNamespace,
                        FileName = this.WorkflowTypeName + "." + activity.MethodName.Replace("Async", "") + ".cs"
                    };
                    actPartial.ImportCollection.Add(typeof(Entity).Namespace);
                    actPartial.ImportCollection.Add(typeof(Int32).Namespace);
                    actPartial.ImportCollection.Add(typeof(Task<>).Namespace);
                    actPartial.ImportCollection.Add(typeof(Enumerable).Namespace);
                    actPartial.ImportCollection.Add(typeof(System.Xml.Serialization.XmlAttributeAttribute).Namespace);
                    actPartial.MethodCollection.AddRange(activity.OtherMethodCollection);
                    @classes.Add(actPartial);
                }
            }




            var customSchemaCode = this.GenerateXsdCsharpClasses().ToList();
            customSchemaCode.ForEach(c => c.Namespace = this.CodeNamespace);
            customSchemaCode.ForEach(c => c.AddNamespaceImport(typeof(DomainObject)));
            customSchemaCode.ForEach(c => c.AddNamespaceImport(typeof(DateTime)));
            customSchemaCode.ForEach(c => c.AddNamespaceImport(typeof(System.Xml.Serialization.XmlAttributeAttribute)));
            @classes.AddRange(customSchemaCode);

            var @accList = from a in this.ActivityCollection
                           let @acc = a.GeneratedCustomTypeCode(this)
                           select @acc;
            @classes.AddRange(@accList.SelectMany(c => c.ToList()));

            // controller
            var @controller = new Class
            {
                Name = string.Format("{0}Controller", this.WorkflowTypeName),
                BaseClass = "Controller",
                FileName = this.WorkflowTypeName + "Controller.cs",
                Namespace = this.CodeNamespace,
                IsPartial = true
            };
            @controller.ImportCollection.Add("System.Web.Mvc");
            @controller.ImportCollection.Add("System.Net.Http");
            @controller.ImportCollection.Add(typeof(Exception).Namespace);
            @controller.ImportCollection.Add(typeof(DomainObject).Namespace);
            @controller.ImportCollection.Add(typeof(Task<>).Namespace);
            @controller.AttributeCollection.Add(string.Format("     [RoutePrefix(\"wf/{0}/v{1}\")]", this.Id, this.Version));

            @controller.MethodCollection.Add(this.GenerateSearchMethod());
            @controller.MethodCollection.Add(this.GenerateJsSchemasController());
            @classes.Add(@controller);

            return @classes;
        }

        public Task<IProjectModel> GetModelAsync()
        {
            throw new NotImplementedException();
        }

        private Method GenerateJsSchemasController()
        {
            var code = new StringBuilder();


            // custom schema
            code.AppendLine("       [HttpGet]");
            code.AppendLine("       [Route(\"schemas\")]");
            code.AppendLinf("       public async Task<ActionResult> Schemas()");
            code.AppendLine("       {");
            code.AppendLine("           var store = ObjectBuilder.GetObject<IBinaryStore>();");
            code.AppendLinf("           var doc = await store.GetContentAsync(\"wd.{0}.{1}\");", this.Id, this.Version);
            code.AppendLine(@"           WorkflowDefinition wd;
            using (var stream = new System.IO.MemoryStream(doc.Content))
            {
                wd = stream.DeserializeFromJson<WorkflowDefinition>();
            }

                                        ");
            code.AppendLinf("           var script = await wd.GenerateCustomXsdJavascriptClassAsync();", this.WebId);
            code.AppendLine("           this.Response.ContentType = \"application/javascript\";");

            code.AppendLine("           return Content(script);");
            code.AppendLine("       }");

            return new Method { Code = code.ToString(), Name = "Schemas" };

        }

        public Task<string> GenerateCustomXsdJavascriptClassAsync()
        {
            var wd = this;
            var script = new StringBuilder();
            script.AppendLine("var bespoke = bespoke ||{};");
            script.AppendLine("bespoke.sph = bespoke.sph ||{};");
            script.AppendLine("bespoke.sph.wf = bespoke.sph.wf ||{};");
            script.AppendLinf("bespoke.sph.wf.{0} = bespoke.sph.wf.{0} ||{{}};", wd.WorkflowTypeName, wd.Version);

            var xsd = wd.GetCustomSchema();

            var complexTypesElement = xsd.Elements(x + "complexType").ToList();
            var complexTypeClasses = complexTypesElement.Select(wd.GenerateXsdComplexTypeJavascript).ToList();
            complexTypeClasses.ForEach(c => script.AppendLine(c));

            var elements = xsd.Elements(x + "element").ToList();
            var elementClasses = elements.Select(e => wd.GenerateXsdElementJavascript(e, 0, s => complexTypesElement.Single(f => f.Attribute("name").Value == s))).ToList();
            elementClasses.ForEach(c => script.AppendLine(c));



            return Task.FromResult(script.ToString());
        }

        private Method GenerateSearchMethod()
        {
            var code = new StringBuilder();

            code.AppendLinf("//exec:Search");
            code.AppendLine("       [HttpPost]");
            code.AppendLine("       [Route(\"search\")]");
            code.AppendLinf("       public async Task<ActionResult> Search()");
            code.AppendLine("       {");
            code.AppendLinf(@"
            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new StringContent(json);
            var url = string.Format(""{{0}}/{{1}}/workflow_{0}_{1}/_search"", ConfigurationManager.ElasticSearchHost, ConfigurationManager.ElasticSearchIndex );

            using(var client = new  HttpClient())
            {{
                var response = await client.PostAsync(url, request);
                var content = response.Content as StreamContent;

                if (null == content) throw new Exception(""Cannot execute query on es "" + request);
                return Content(await content.ReadAsStringAsync(),""application/json; charset=utf-8"");
            }}
            ", this.Id, this.Version);

            code.AppendLine("       }");


            return new Method { Code = code.ToString(), Name = "Search" };
        }



    }
}