using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Send", TypeName = "Send", Description = "Send a message to another system")]
    public partial class SendActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = new BuildValidationResult { Result = true };
            try
            {
                var assembly = Assembly.Load(this.AdapterAssembly);
                Console.WriteLine(assembly);
            }
            catch (FileNotFoundException)
            {
                result.Errors.Add(new BuildError(this.WebId, string.Format("[SendActivity] : Cannot find custom entity assembly \"{0}\" for {1}", this.AdapterAssembly, this.Name)));

            }
            result.Result = result.Errors.Count == 0;
            return result;
        }


        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);


            var vrb = wd.VariableDefinitionCollection.SingleOrDefault(d => d.Name == this.ReturnValuePath);
            var code = new StringBuilder();


            foreach (var cs in this.InitializingCorrelationSetCollection)
            {
                var cors = wd.CorrelationSetCollection.Single(x => x.Name == cs);
                var cort = wd.CorrelationTypeCollection.Single(x => x.Name == cors.Type);
                var valExpression = cort.CorrelationPropertyCollection.Select(x => "string.Format(\"{0}\",this." + x.Path + ")");
                code.AppendLine("       // Initializing the correlation set");
                code.AppendLinf("       Console.WriteLine(\"Init correlation set {0}\");", cors);
                code.AppendLinf("       await this.InitializeCorrelationSetAsync(\"{0}\", string.Join(\";\",new []{{{1}}}));", cors.Name, string.Join(",", valExpression));
            }

            code.AppendLinf("        var adapter = new {0}();", this.Adapter);
            if (this.ExceptionFilterCollection.Any())
            {
                foreach (var ef in this.ExceptionFilterCollection)
                {
                    code.AppendLine("        var tries = 0;");
                    code.AppendLinf("        while(tries < {0})", ef.MaxRequeue);
                    code.AppendLine("        {");
                    code.AppendLine("           if(tries > 0)");
                    code.AppendLinf("               await Task.Delay({0});", ef.IntervalInMilisecods);

                    code.AppendLine();
                    code.AppendLine("           tries++;");
                    code.AppendLine("           try");
                    code.AppendLine("           {");
                    code.AppendLine("               Console.WriteLine(\"Trying for {0} time\", tries);");
                    code.AppendLinf("               var result = await adapter.{0}(this.{1});", this.Method, this.ArgumentPath);
                    if (null != vrb)
                    {
                        code.AppendLinf("               this.{0} =  ({1})result;", this.ReturnValuePath, Type.GetType(vrb.TypeName).ToCSharp());
                    }
                    code.AppendLinf("               break;");
                    code.AppendLine("           }");
                    code.AppendLinf("           catch({0} exc)", ef.TypeName);
                    code.AppendLine("           {");
                    code.AppendLinf("               if(tries >={0}) throw;", ef.MaxRequeue);
                    code.AppendLine("           }");
                    code.AppendLine("        }");
                }
            }
            else
            {
                code.AppendLinf("        var response = await adapter.{0}(this.{1});", this.Method, this.ArgumentPath);
                if (null != vrb)
                {
                    code.AppendLinf("        this.{0} =  ({1})response;", this.ReturnValuePath, Type.GetType(vrb.TypeName).ToCSharp());
                }

            }


            code.AppendLine();
            // set the next activity
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLinf("       result.NextActivities = new[]{{\"{0}\"}};", this.NextActivityWebId);/* webid*/


            return code.ToString();
        }
    }
}