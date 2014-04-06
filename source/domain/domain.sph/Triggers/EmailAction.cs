using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class EmailAction : CustomAction
    {
        public override void Execute(RuleContext context)
        {
            throw new Exception("Not implemented, use the asynhronous execute");
        }

        public async override Task ExecuteAsync(RuleContext context)
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

            var smtp = new SmtpClient();
            await smtp.SendMailAsync(message).ConfigureAwait(false);
        }

        public override bool UseAsync
        {
            get { return true; }
        }
    }
}