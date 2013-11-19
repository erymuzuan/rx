using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class NotificationActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            return new BuildValidationResult { Result = true };
        }

        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {

            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       var result = new ActivityExecutionResult();");
            code.AppendLinf("       this.CurrentActivityWebId = \"{0}\";", this.NextActivityWebId);
            code.AppendLinf("       System.Console.WriteLine(\"Sending email to : {0}\");", this.To);


            code.AppendLinf("       var @from = await this.Transform{0}(\"{1}\");", this.MethodName, this.From);
            code.AppendLinf("       var to = await this.Transform{0}(\"{1}\");", this.MethodName, this.To);
            code.AppendLinf("       var subject = await this.Transform{0}(\"{1}\");", this.MethodName, this.Subject);
            code.AppendLinf("       var body = await this.Transform{0}(\"{1}\");", this.MethodName, this.Body);

            if (!string.IsNullOrWhiteSpace(this.UserName))
            {
                code.AppendLine("       var context = new SphDataContext();");
                code.AppendLine("       var message = new Message");
                code.AppendLine("       {");
                code.AppendLinf("           Subject = subject,");
                code.AppendLinf("           UserName = \"{0}\",", this.UserName);
                code.AppendLinf("           Body = body");
                code.AppendLine("       };");
                code.AppendLine("       using (var session = context.OpenSession())");
                code.AppendLine("       {");
                code.AppendLine("           session.Attach(message);");
                code.AppendLinf("           await session.SubmitChanges(\"{0}\");", this.Name);
                code.AppendLine("       }");

            }
            code.AppendLine("       var client = new System.Net.Mail.SmtpClient();");
            code.AppendLine("       await client.SendMailAsync(@from, to, subject,body);");

            code.AppendLinf("       await this.SaveAsync(\"{0}\");", this.WebId);
            code.AppendLine("       return result;");
            code.AppendLine("   }");
            code.AppendLine();

            code.AppendLinf("   private async Task<string> Transform{0}(string template)", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       if (string.IsNullOrWhiteSpace(template)) return string.Empty;");

            code.AppendLine("       if (template.StartsWith(\"=\"))");
            code.AppendLine("       {");
            code.AppendLine("           var script = ObjectBuilder.GetObject<IScriptEngine>();");
            code.AppendLine("           return script.Evaluate<string, Workflow>(template.Remove(0,1), this);");
            code.AppendLine("       }");
            code.AppendLine("       var razor = ObjectBuilder.GetObject<ITemplateEngine>();");
            code.AppendLine("       return await razor.GenerateAsync(template, this);");

            code.AppendLine("   }");



            return code.ToString();
        }



        public override Task<ActivityExecutionResult> ExecuteAsync()
        {
            return null;
        }
    }
}
