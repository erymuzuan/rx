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


        public async Task<string> Transform(string template, Workflow wf)
        {
            if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            if (template.StartsWith("="))
            {
                var script = ObjectBuilder.GetObject<IScriptEngine>();
                return script.Evaluate<string, Workflow>(template.Remove(0, 1), wf);
            }
            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);

        }

        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {

            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       var result = new ActivityExecutionResult();");
            code.AppendLinf("       var act = this.GetActivity<NotificationActivity>(\"{0}\");", this.WebId);
            code.AppendLinf("       this.CurrentActivityWebId = \"{0}\";", this.NextActivityWebId);


            code.AppendLine("       var @from = await act.Transform(act.From, this);");
            code.AppendLine("       var to = await act.Transform(act.To, this);");
            code.AppendLine("       var subject = await act.Transform(act.Subject, this);");
            code.AppendLine("       var body = await act.Transform(act.Body, this);");

            code.AppendLine("       System.Console.WriteLine(\"Sending email to : {0}\", to);");
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




            return code.ToString();
        }



        public override Task<ActivityExecutionResult> ExecuteAsync()
        {
            return null;
        }
    }
}
