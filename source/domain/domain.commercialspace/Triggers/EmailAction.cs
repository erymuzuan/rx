using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class EmailAction : CustomAction
    {
        public override void Execute(Entity item)
        {
            throw new Exception("Not implemented, use the asynhronous execute");
        }

        public async override Task ExecuteAsync(Entity item)
        {
            var templateEngine = ObjectBuilder.GetObject<ITemplateEngine>();
            var subject = await templateEngine.GenerateAsync(this.SubjectTemplate, item).ConfigureAwait(false);
            var body = await templateEngine.GenerateAsync(this.BodyTemplate, item).ConfigureAwait(false);
            var message = new MailMessage
            {
                Subject = subject,
                Body = body,
                From = new MailAddress(this.From)

            };
            message.To.Add(this.To);
            if (!string.IsNullOrWhiteSpace(this.Cc))
                message.CC.Add(this.Cc);

            var smtp = new SmtpClient();
            await smtp.SendMailAsync(message).ConfigureAwait(false);
        }

        public override bool UseAsync
        {
            get { return true; }
        }
    }
}