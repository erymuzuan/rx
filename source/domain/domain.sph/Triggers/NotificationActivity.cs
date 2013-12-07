using System;
using System.Linq.Expressions;
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
            code.AppendLine("       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};");
            code.AppendLinf("       var act = this.GetActivity<NotificationActivity>(\"{0}\");", this.WebId);
            code.AppendLinf("       result.NextActivities = new[]{{\"{0}\"}};", this.NextActivityWebId);


            code.AppendLinf("       var @from = await this.TransformFrom{0}(act.From);", this.MethodName);
            code.AppendLinf("       var to = await this.TransformTo{0}(act.To);", this.MethodName);
            code.AppendLinf("       var subject = await this.TransformSubject{0}(act.Subject);", this.MethodName);
            code.AppendLinf("       var body = await this.TransformBody{0}(act.Body);", this.MethodName);

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

            code.AppendLine("       return result;");
            code.AppendLine("   }");
            code.AppendLine();

            code.AppendLine(this.GenerateTranformCode(x => x.From));
            code.AppendLine(this.GenerateTranformCode(x => x.To));
            code.AppendLine(this.GenerateTranformCode(x => x.Subject));
            code.AppendLine(this.GenerateTranformCode(x => x.Body));



            return code.ToString();
        }

        private string GenerateTranformCode(Expression<Func<NotificationActivity, string>>  field)
        {
            var code = new StringBuilder();
            var func = field.Compile();
            var template = func(this);
            dynamic fdyn = field.Body;
            string fieldName = fdyn.Member.Name;
            // tranform
            code.AppendLinf("   public async Task<string> Transform{1}{0}(string template)", this.MethodName, fieldName);
            code.AppendLine("   {");

            code.AppendLine("        if (string.IsNullOrWhiteSpace(template)) return string.Empty;");
            if (template.StartsWith("="))
            {
                code.AppendLinf(@"
                await Task.Delay(100);
                var item = this;
                return {0};", template.Remove(0, 1));
            }
            else
            {
                code.AppendLine(@"
            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this);");
            }
            code.AppendLine("   }");

            return code.ToString();

        }



        public override Task<ActivityExecutionResult> ExecuteAsync()
        {
            return null;
        }
    }
}
