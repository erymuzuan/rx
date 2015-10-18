using System;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Notification", TypeName = "Notification", Description = "Notify via email and messages")]
    public partial class NotificationActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            return new BuildValidationResult { Result = true };
        }


        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            this.OtherMethodCollection.Clear();
            this.OtherMethodCollection.Add(new Method
            {
                Code = this.GenerateTranformCode(x => x.From)
            });

            this.OtherMethodCollection.Add(new Method
            {
                Code = this.GenerateTranformCode(x => x.To)
            });

            this.OtherMethodCollection.Add(new Method
            {
                Code = this.GenerateTranformCode(x => x.Subject)
            });

            this.OtherMethodCollection.Add(new Method
            {
                Code = this.GenerateTranformCode(x => x.Body)
            });


            var code = new StringBuilder();

            code.AppendLine("       var logger = ObjectBuilder.GetObject<ILogger>();");
            code.AppendLine("       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};");
            code.AppendLine($"       var act = this.GetActivity<NotificationActivity>(\"{this.WebId}\");");
            code.AppendLine($"       result.NextActivities = new[]{{\"{this.NextActivityWebId}\"}};");
            code.AppendLine();

            code.AppendLine($"       var @from = await this.TransformFrom{this.MethodName}(act.From);");
            code.AppendLine($"       var to = await this.TransformTo{this.MethodName}(act.To);");
            code.AppendLine($"       var subject = await this.TransformSubject{this.MethodName}(act.Subject);");
            code.AppendLine($"       var body = await this.TransformBody{this.MethodName}(act.Body);");
            code.AppendLine($"       var cc = await this.TransformBody{this.MethodName}(act.Cc);");
            code.AppendLine($"       var bcc = await this.TransformBody{this.MethodName}(act.Bcc);");

            code.AppendLine();

            code.AppendLine("       var client = new System.Net.Mail.SmtpClient();");
            code.AppendLine("       var mm = new System.Net.Mail.MailMessage();");
            code.AppendLine("       mm.Subject = subject;");
            code.AppendLine("       mm.Body = body;");
            code.AppendLine("       mm.From = new System.Net.Mail.MailAddress(@from);");
            code.AppendLine("       mm.To.Add(to);");
            code.AppendLine($"      mm.IsBodyHtml = { (this.IsHtmlEmail ? "true" : "false")};");
            code.AppendLine("       if (!string.IsNullOrWhiteSpace(cc))");
            code.AppendLine("           mm.CC.Add(cc);");
            code.AppendLine("       if (!string.IsNullOrWhiteSpace(bcc))");
            code.AppendLine("           mm.Bcc.Add(bcc);");
            if (null != this.Retry)
            {
                code.AppendLine("      await Policy.Handle<InvalidOperationException>()");
                code.AppendLine("               .Or<System.Net.WebException>()");
                code.AppendLine("               .Or<System.Net.Mail.SmtpException>()");
                code.AppendLine("               .Or<System.IO.DirectoryNotFoundException>()");
                code.AppendLine("               .Or<System.Exception>()");
                code.AppendLine($"          .WaitAndRetryAsync({this.Retry}, cx => TimeSpan.FromMilliseconds({this.RetryInterval ?? 5000}),");
                code.AppendLine("              (exc, time) =>");
                code.AppendLine("              {  ");
                code.AppendLine("                   logger.LogAsync(new LogEntry(exc)).ContinueWith(_ => ");
                code.AppendLine("                       { ");
                code.AppendLine("                           logger.Log(new LogEntry { Message = \"Fail to send email, retry again in \" + time.TotalSeconds + \" seconds\", Severity = Severity.Info });");
                code.AppendLine("                       });");
                code.AppendLine("               }");
                code.AppendLine("           )");
                code.AppendLine("           .ExecuteAsync(() => client.SendMailAsync(mm));");


            }
            else
            {
                code.AppendLine("       await client.SendMailAsync(mm).ConfigureAwait(false);");
            }

            code.AppendLine();
            code.AppendLine();
            if (this.IsMessageSuppressed)
            {
                return code.ToString();
            }

            code.AppendLine("       var context = new SphDataContext();");
            code.AppendLine("       foreach(var et in to.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries))");
            code.AppendLine("       {");
            code.AppendLine("           var et1 = et;");
            code.AppendLine("           var user = await context.LoadOneAsync<UserProfile>(u => u.Email == et1);");
            code.AppendLine("           if(null == user)continue;");

            code.AppendLine("           var message = new Message");
            code.AppendLine("                           {");
            code.AppendLine("                               Subject = subject,");
            code.AppendLine("                               UserName = user.UserName,");
            code.AppendLine("                               Body = body,");
            code.AppendLinf("                               Id = Strings.GenerateId()");
            code.AppendLine("                           };");
            code.AppendLine("           using (var session = context.OpenSession())");
            code.AppendLine("           {");
            code.AppendLine("               session.Attach(message);");
            code.AppendLine($"              await session.SubmitChanges(\"{this.Name}\").ConfigureAwait(false);");
            code.AppendLine("           }");
            code.AppendLine("       }");

            code.AppendLine();

            return code.ToString();
        }

        private string GenerateTranformCode(Expression<Func<NotificationActivity, string>> field)
        {
            var code = new StringBuilder();
            var func = field.Compile();
            var template = func(this);
            dynamic fdyn = field.Body;
            string fieldName = fdyn.Member.Name;
            // tranform

            if (template.StartsWith("="))
            {
                code.AppendLine($"   private Task<string> Transform{fieldName}{this.MethodName}(string template)");
                code.AppendLine("   {");
                code.AppendLine("        if (string.IsNullOrWhiteSpace(template)) return Task.FromResult(string.Empty);");
                code.AppendFormat(@"
                var item = this;
                return Task.FromResult({0});", template.Remove(0, 1));
            }
            else
            {
                code.AppendLine($"   private async Task<string> Transform{fieldName}{this.MethodName}(string template)");
                code.AppendLine("   {");
                code.Append(@"
            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            return await razor.GenerateAsync(template, this).ConfigureAwait(false);");
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
