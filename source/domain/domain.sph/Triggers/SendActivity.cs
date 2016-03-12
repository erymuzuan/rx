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
                result.Errors.Add(new BuildError(this.WebId, $"[SendActivity] : Cannot find custom entity assembly \"{this.AdapterAssembly}\" for {this.Name}"));

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

            code.AppendLine($"        var adapter = new {Adapter}();");
            if (this.ExceptionFilterCollection.Any())
            {
                foreach (var ef in this.ExceptionFilterCollection)
                {
                    code.AppendLine("        var tries = 0;");
                    code.AppendLine($"        while(tries < {ef.MaxRequeue})");
                    code.AppendLine("        {");
                    code.AppendLine("           if(tries > 0)");
                    code.AppendLine($"               await Task.Delay({ef.IntervalInMilisecods});");

                    code.AppendLine();
                    code.AppendLine("           tries++;");
                    code.AppendLine("           try");
                    code.AppendLine("           {");
                    code.AppendLine("               Console.WriteLine(\"Trying for {0} time\", tries);");
                    code.AppendLine($"               var result = await adapter.{Method}(this.{ArgumentPath});");
                    if (null != vrb)
                    {
                        var type = Strings.GetType(vrb.TypeName).ToCSharp();
                        code.AppendLine($"               this.{ReturnValuePath} =  ({type})result;");
                    }
                    code.AppendLinf("               break;");
                    code.AppendLine("           }");
                    code.AppendLine($"           catch({ef.TypeName} exc)");
                    code.AppendLine("           {");
                    code.AppendLine($"               if(tries >={ef.MaxRequeue}) throw;");
                    code.AppendLine("           }");
                    code.AppendLine("        }");
                }
            }
            else
            {
                code.AppendLine($"        var response = await adapter.{Method}(this.{ArgumentPath});");
                if (null != vrb)
                {
                    var type = Strings.GetType(vrb.TypeName).ToCSharp();
                    code.AppendLine($"               this.{ReturnValuePath} =  ({type})response;");
                }

            }


            code.AppendLine();
            // set the next activity
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLine($@"       result.NextActivities = new[]{{""{NextActivityWebId}""}};");/* webid*/


            return code.ToString();
        }
    }
}