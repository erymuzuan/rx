using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Net.Mail;
using System.Threading.Tasks;
using Humanizer;
using Polly;

namespace Bespoke.Sph.Domain
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "Email", TypeName = "Bespoke.Sph.Domain.EmailAction, domain.sph", Description = "Send a email message", FontAwesomeIcon = "envelope")]
    public partial class EmailAction : CustomAction
    {
        public override void Execute(RuleContext context)
        {
            throw new Exception("Not implemented, use the asynhronous execute");
        }

        public override async Task ExecuteAsync(RuleContext context)
        {
            var item = context.Item;
            var templateEngine = ObjectBuilder.GetObject<ITemplateEngine>();
            var subject = await templateEngine.GenerateAsync(this.SubjectTemplate, item).ConfigureAwait(false);
            var body = await templateEngine.GenerateAsync(this.BodyTemplate, item).ConfigureAwait(false);
            var to = await templateEngine.GenerateAsync(this.To, item).ConfigureAwait(false);
            var cc = await templateEngine.GenerateAsync(this.Cc, item).ConfigureAwait(false);
            var from = await templateEngine.GenerateAsync(this.From, item).ConfigureAwait(false);


            var message = new MailMessage
            {
                Subject = subject,
                Body = body,
                From = new MailAddress(from)

            };
            message.To.Add(to);
            if (!string.IsNullOrWhiteSpace(cc))
                message.CC.Add(cc);

            var logger = ObjectBuilder.GetObject<ILogger>();
            var smtp = new SmtpClient();
            await Policy.Handle<InvalidOperationException>()
                    .Or<System.Net.WebException>()
                    .Or<SmtpException>()
                    .Or<System.IO.DirectoryNotFoundException>()
                    .Or<Exception>()
                    .WaitAndRetryAsync(3, cx => 5.Minutes(),
                            (exc, time) =>
                                   {
                                       logger.LogAsync(new LogEntry(exc)).ContinueWith(_ =>
                                       {
                                           logger.Log(new LogEntry { Message = $"Fail to send email, retry again in {time.TotalSeconds} seconds", Severity = Severity.Info });
                                       });
                                   })
                    .ExecuteAsync(() => smtp.SendMailAsync(message))
                    .ConfigureAwait(false);
        }

        public override bool UseAsync => true;

        public override Bitmap GetPngIcon()
        {
            return Properties.Resources.Message_Mail;
        }

        public override string GetEditorView()
        {
            return Properties.Resources.EmailActionHtml;
        }

        public override string GetEditorViewModel()
        {
            return Properties.Resources.EmailActionJs;
        }
    }
}