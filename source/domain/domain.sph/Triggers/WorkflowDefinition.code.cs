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

            // static ctor
            code.AppendLinf("       static {0}()", this.WorkflowTypeName);
            code.AppendLine("       {");  // register type for the XML serializer
            code.AppendLinf("           XmlSerializerService.RegisterKnownTypes(typeof({0}),typeof({1}));", typeof(Workflow).Name, this.WorkflowTypeName);

            code.AppendLine("       }");

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
            code.AppendLinf("           return this.{0}();", this.GetInititorScreen().MethodName);

            code.AppendLine("       }");
            // execute
            code.AppendLine("       public async override Task<ActivityExecutionResult> ExecuteAsync()");
            code.AppendLine("       {");
            code.AppendLine("               var act = this.GetCurrentActivity();");
            code.AppendLine("               if(null == act)");
            code.AppendLine("                   throw new InvalidOperationException(\"No current activity\");");



            code.AppendLine("               if(act.IsAsync && this.State == \"WaitingAsync\")");
            code.AppendLine("               {");
            code.AppendLine("                   return new ActivityExecutionResult{Status = ActivityExecutionStatus.WaitingAsync};");
            code.AppendLine("               }");



            code.AppendLine("               if(act.IsAsync)");
            code.AppendLine("               {");
            code.AppendLine("                   this.State = \"WaitingAsync\";");

            code.AppendLine("                   await act.InitiateAsync(this);");
            code.AppendLine("                   await this.SaveAsync(act.WebId);");
            code.AppendLine("                   return new ActivityExecutionResult{Status = ActivityExecutionStatus.WaitingAsync};");
            code.AppendLine("               }");
            code.AppendLine();
            code.AppendLine("               ActivityExecutionResult result = null;");
            code.AppendLine("               switch(act.WebId)");
            code.AppendLine("               {");

            foreach (var activity in this.ActivityCollection)
            {
                code.AppendLinf("                   case \"{0}\" : ", activity.WebId);
                code.AppendLinf("                       result = await this.{0}();", activity.MethodName);
                code.AppendLine("                       break;");
            }
            code.AppendLine("               }");// end switch

            code.AppendLine("               if(null == result)");
            code.AppendLine("                   throw new Exception(\"what ever\");");
            code.AppendLine("               if(null != result.NextActivity)");
            code.AppendLine("               {");
            code.AppendLine("                   this.CurrentActivityWebId = result.NextActivity.WebId;");
            code.AppendLine("                   await this.SaveAsync(act.WebId);");
            code.AppendLine("               }");
            code.AppendLine("                return result;");


            code.AppendLine("       }");// end Execute

            // properties for each Variables
            foreach (var variable in this.VariableDefinitionCollection)
            {
                code.AppendLine("       " + variable.GeneratedCode(this));
            }

            // activities method
            foreach (var activity in this.ActivityCollection)
            {
                code.AppendLine();
                code.AppendLine(activity.GeneratedExecutionMethodCode(this));
            }


            code.AppendLine("   }");// end class


            var customSchemaCode = this.GenerateXsdCsharpClasses();
            code.AppendLine(customSchemaCode);

            foreach (var activity in this.ActivityCollection)
            {
                code.AppendLine("   " + activity.GeneratedCustomTypeCode(this));
            }

            code.AppendLine("}");// end namespace
            return code.ToString();
        }



    }
}