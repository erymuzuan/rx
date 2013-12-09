﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition
    {

        private string GenerateCode()
        {
            var code = new StringBuilder();
            code.AppendLine("using " + typeof(Entity).Namespace + ";");
            code.AppendLine("using " + typeof(Int32).Namespace + ";");
            code.AppendLine("using " + typeof(Task<>).Namespace + ";");
            code.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            code.AppendLine("using " + typeof(XmlAttributeAttribute).Namespace + ";");
            code.AppendLine();

            code.AppendLine("namespace " + this.CodeNamespace);
            code.AppendLine("{");

            code.AppendLine("   [EntityType(typeof(Workflow))]");
            code.AppendLine("   public class " + this.WorkflowTypeName + " : " + typeof(Workflow).FullName);
            code.AppendLine("   {");

            // contructor
            code.AppendLine("       public " + this.WorkflowTypeName + "()");
            code.AppendLine("       {");

            // default properties
            code.AppendLinf("           this.Name = \"{0}\";", this.Name);
            code.AppendLinf("           this.Version = {0};", this.Version);
            code.AppendLinf("           this.WorkflowDefinitionId = {0};", this.WorkflowDefinitionId);

            foreach (var variable in this.VariableDefinitionCollection.OfType<ComplexVariable>())
            {
                code.AppendLinf("           this.{0} = new {1}();", variable.Name, variable.TypeName);
            }
            code.AppendLine("       }");// end contructor

            // start
            code.AppendLine("       public override Task<ActivityExecutionResult> StartAsync()");
            code.AppendLine("       {");
            code.AppendLinf("           this.SerializedDefinitionStoreId = \"wd.{0}.{1}\";", this.WorkflowDefinitionId, this.Version);
            code.AppendLinf("           return this.{0}();", this.GetInitiatorActivity().MethodName);
            code.AppendLine("       }");

            // execute
            code.AppendLine("       public override async Task<ActivityExecutionResult> ExecuteAsync(string activityId)");
            code.AppendLine("       {");
            code.AppendLinf("           this.SerializedDefinitionStoreId = \"wd.{0}.{1}\";", this.WorkflowDefinitionId, this.Version);
            code.AppendLine("           ActivityExecutionResult result = null;");
            code.AppendLine("           switch(activityId)");
            code.AppendLine("           {");

            foreach (var activity in this.ActivityCollection)
            {
                code.AppendLinf("                   case \"{0}\" : ", activity.WebId);
                code.AppendLinf("                       result = await this.{0}();", activity.MethodName);
                code.AppendLine("                       break;");
            }
            code.AppendLine("           }");// end switch
            code.AppendLine("           await this.SaveAsync(activityId, result);");
            code.AppendLinf("           return result;");
            code.AppendLine("       }");


            // properties for each Variables
            foreach (var variable in this.VariableDefinitionCollection)
            {
                code.AppendLinf("//variable:{0}", variable.Name);
                code.AppendLine("       " + variable.GeneratedCode(this));
            }

            // activities method
            foreach (var activity in this.ActivityCollection)
            {
                activity.BeforeGenerate(this);
            }

            foreach (var activity in this.ActivityCollection)
            {
                code.AppendLine();
                code.AppendLine("//exec:" + activity.WebId);

                if (activity.IsAsync)
                    code.AppendLine(activity.GeneratedInitiateAsyncCode(this));

                code.AppendLine(activity.GeneratedExecutionMethodCode(this));
            }


            code.AppendLine("   }");// end class


            var customSchemaCode = this.GenerateXsdCsharpClasses();
            code.AppendLine(customSchemaCode);

            foreach (var activity in this.ActivityCollection)
            {
                code.AppendLine("   " + activity.GeneratedCustomTypeCode(this));
            }
            // search
            this.GenerateSearchController(code);

            code.AppendLine("}");// end namespace
            return code.ToString();
        }

        private void GenerateSearchController(StringBuilder code)
        {
            code.AppendLinf("public partial class Workflow_{0}_{1}Controller : System.Web.Mvc.Controller", this.WorkflowDefinitionId, this.Version);
            code.AppendLine("{");
            code.AppendLinf("//exec:Search");
            code.AppendLinf("       public async Task<System.Web.Mvc.ActionResult> Search()");
            code.AppendLine("       {");
            code.AppendLinf(@"
            var json = Bespoke.Sph.Web.Helpers.ControllerHelpers.GetRequestBody(this);
            var request = new System.Net.Http.StringContent(json);
            var esHost = ConfigurationManager.ElasticSearchHost;

            var client = new System.Net.Http.HttpClient();
            var response = await client.PostAsync(esHost + ""workflow_{0}_{1}/_search"", request);
            var content = response.Content as System.Net.Http.StreamContent;
            if (null == content) throw new Exception(""Cannot execute query on es "" + request);
            this.Response.ContentType = ""application/json; charset=utf-8"";
            return Content(await content.ReadAsStringAsync());", this.WorkflowDefinitionId, this.Version);
            code.AppendLine("       }");
            code.AppendLine("}");



        }



    }
}