using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition
    {

        public IEnumerable<Class> GenerateCode()
        {
            var classes = new List<Class>();
            var wcd = GenerateWorkflowClass();
            classes.Add(wcd);

            var activityPartials = from a in this.ActivityCollection
                let pc = a.GenerateWorkflowPartial(this)
                where null != pc
                select pc;
            classes.AddRange(activityPartials);


            foreach (var var in this.VariableDefinitionCollection)
            {
                var varClasses = var.GenerateCustomTypesAsync(this).Result;
                classes.AddRange(varClasses);

            }

            var accList = from a in this.ActivityCollection
                           let acc = a.GeneratedCustomTypeCode(this)
                           select acc;
            classes.AddRange(accList.SelectMany(c => c.ToList()));

            // controller
            var controller = GenerateController();
            classes.Add(controller);

            return classes;
        }

        private Class GenerateWorkflowClass()
        {
            var wcd = new Class
            {
                Name = this.WorkflowTypeName,
                Namespace = this.CodeNamespace,
                BaseClass = typeof(Workflow).FullName,
                FileName = this.WorkflowTypeName + ".cs",
                IsPartial = true
            };
            wcd.ImportCollection.Add(typeof(Entity).Namespace);
            wcd.ImportCollection.Add(typeof(int).Namespace);
            wcd.ImportCollection.Add(typeof(Task<>).Namespace);
            wcd.ImportCollection.Add(typeof(Enumerable).Namespace);
            wcd.ImportCollection.Add(typeof(XmlAttributeAttribute).Namespace);
            wcd.ImportCollection.Add(typeof(Polly.Policy).Namespace);
            wcd.AttributeCollection.Add("   [EntityType(typeof(Workflow))]");

            var ctor = new StringBuilder();
            // contructor
            ctor.AppendLine($"       public {this.WorkflowTypeName}()");
            ctor.AppendLine("       {");

            // default properties
            ctor.AppendLine($"           this.Name = \"{Name}\";");
            ctor.AppendLine($"           this.Version = {Version};");
            ctor.AppendLine($"           this.WorkflowDefinitionId = \"{Id}\";");

            foreach (var var in this.VariableDefinitionCollection)
            {
                ctor.AppendLine(var.GeneratedCtorCode(this));
            }
            ctor.AppendLine("       }");// end contructor
            wcd.CtorCollection.Add(ctor.ToString());

            var start = this.GenerateStartMethod();
            wcd.AddMethod(start);

            var execute = this.GenerateExecuteMethod();
            wcd.MethodCollection.Add(execute);


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
            }

            return wcd;
        }

        private Class GenerateController()
        {
            var controller = new Class
            {
                Name = $"{this.WorkflowTypeName}Controller",
                BaseClass = "BaseApiController",
                FileName = this.WorkflowTypeName + "Controller.cs",
                Namespace = this.CodeNamespace,
                IsPartial = true
            };
            controller.ImportCollection.Add("System.Web.Http");
            controller.ImportCollection.Add("System.Net.Http");
            controller.ImportCollection.Add("Bespoke.Sph.WebApi");
            controller.ImportCollection.Add(typeof(Exception).Namespace);
            controller.ImportCollection.Add(typeof(DomainObject).Namespace);
            controller.ImportCollection.Add(typeof(Task<>).Namespace);
            controller.ImportCollection.Add(typeof(Enumerable).Namespace);
            controller.ImportCollection.Add(typeof(Newtonsoft.Json.Linq.JObject).Namespace);
            controller.AttributeCollection.Add($"     [RoutePrefix(\"wf/{this.Id}/v{this.Version}\")]");

            controller.MethodCollection.Add(this.GenerateGetPendingTasksMethod());
            controller.MethodCollection.Add(this.GenerateSearchMethod());
            controller.MethodCollection.Add(this.GenerateGetOneEndpointMethod());
            controller.MethodCollection.Add(this.GenerateJsSchemasController());
            return controller;
        }

        private Method GenerateStartMethod()
        {

            var start = new StringBuilder();
            // start
            start.AppendLine("       public override async Task<ActivityExecutionResult> StartAsync()");
            start.AppendLine("       {");
            start.AppendLinf("           this.SerializedDefinitionStoreId = \"wd.{0}.{1}\";", this.Id, this.Version);
            start.AppendLinf("           var result =await this.{0}().ConfigureAwait(false);", this.GetInitiatorActivity().MethodName);
            start.AppendLinf("           return result;");
            start.AppendLine("       }");

            return new Method { Code = start.ToString() };
        }

        private Method GenerateExecuteMethod()
        {
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

            return new Method { Code = execute.ToString() };
        }

        private Method GenerateJsSchemasController()
        {
            var code = new StringBuilder();


            // custom schema
            code.AppendLine("       [HttpGet]");
            code.AppendLine("       [Route(\"schemas\")]");
            code.AppendLinf("       public async Task<IHttpActionResult> Schemas()");
            code.AppendLine("       {");
            code.AppendLine("           var store = ObjectBuilder.GetObject<IBinaryStore>();");
            code.AppendLinf("           var doc = await store.GetContentAsync(\"wd.{0}.{1}\");", this.Id, this.Version);
            code.AppendLine(@"           
            WorkflowDefinition wd;
            using (var stream = new System.IO.MemoryStream(doc.Content))
            {
                wd = stream.DeserializeFromJson<WorkflowDefinition>();
            }");
            code.AppendLine("           var script = await wd.GenerateCustomXsdJavascriptClassAsync();");
            code.AppendLine("           return Javascript(script);");
            code.AppendLine("       }");

            return new Method { Code = code.ToString(), Name = "Schemas" };

        }

        public async Task<string> GenerateCustomXsdJavascriptClassAsync()
        {
            var code = new StringBuilder();
            foreach (var @var in this.VariableDefinitionCollection)
            {
                var script = await @var.GenerateCustomJavascriptAsync(this);
                if (!string.IsNullOrWhiteSpace(script))
                    code.AppendLine(script);
            }
            return code.ToString();
        }

        private Method GenerateGetPendingTasksMethod()
        {
            var code = new StringBuilder();

            code.AppendLinf("//exec:Search");
            code.AppendLine("       [HttpGet]");
            code.AppendLine("       [Route(\"{activity:guid}/pendingtasks\")]");
            code.AppendLinf("       public async Task<IHttpActionResult> GetPendingTasksAsync(string activity)");
            code.AppendLine("       {");
            code.AppendLine($@"
                var variables = new System.Collections.Generic.Dictionary<string, object>();
                foreach (var k in this.Request.GetQueryNameValuePairs())
                {{ 
                    variables.Add(k.Key, k.Value);
                }} 
                var ws = ObjectBuilder.GetObject<IWorkflowService>();
                var list = await ws.GetPendingWorkflowsAsync<{WorkflowTypeName}>(activity, variables);
                return Json(list);
");

            code.AppendLine("       }");

            return new Method { Code = code.ToString(), Name = "GetPendingTasksAsync" };
        }
        

        private Method GenerateSearchMethod()
        {
            var code = new StringBuilder();

            code.AppendLinf("//exec:Search");
            code.AppendLine("       [HttpPost]");
            code.AppendLine("       [Route(\"search\")]");
            code.AppendLinf("       public async Task<IHttpActionResult> Search([RawBody]string json)");
            code.AppendLine("       {");
            code.AppendLine($@"
                var ws = ObjectBuilder.GetObject<IWorkflowService>();
                var items = await ws.SearchAsync<{WorkflowTypeName}>(json);
                return Json(items);
        ");

            code.AppendLine("       }");


            return new Method { Code = code.ToString(), Name = "Search" };
        }

        private Method GenerateGetOneEndpointMethod()
        {
            var code = new StringBuilder();

            code.AppendLinf("//exec:GetOneAsync");
            code.AppendLine("       [HttpGet]");
            code.AppendLine("       [Route(\"{id:guid}\")]");
            code.AppendLinf("       public async Task<IHttpActionResult> GetOneAsync(string id)");
            code.AppendLine("       {");
            code.AppendLine($@"
                var ws = ObjectBuilder.GetObject<IWorkflowService>();
                var item = await ws.GetOneAsync<{WorkflowTypeName}>(id);
                if(null == item)
                    return NotFound();
                return Json(item);

");

            code.AppendLine("       }");
            

            return new Method { Code = code.ToString(), Name = "GetOneAsync" };
        }
        
    }
}