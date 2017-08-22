using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.RabbitMqPublisher
{
    public class SlaNotCompletedEmailAction : MessageSlaNotificationAction
    {
        public string EmailTemplateMapping { get; }
        public string ToAddresses { get; }

        public SlaNotCompletedEmailAction(string emailTemplateMapping, string toAddresses)
        {
            EmailTemplateMapping = emailTemplateMapping;
            ToAddresses = toAddresses;
        }
        public override bool UseAsync => true;
       

        public override async Task<bool> ExecuteAsync(MessageTrackingStatus status, Entity item, MessageSlaEvent @event)
        {
            var templateId = this.EmailTemplateMapping.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(x => x.StartsWith($"{item.GetType().Name}"))
                .ToEmptyString()
                .Replace($"{item.GetType().Name}:", "");

            var context = new SphDataContext();
            var template = await context.LoadOneAsync<EmailTemplate>(x => x.Id == templateId);
            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            var subject = await razor.GenerateAsync(template.SubjectTemplate, item);
            var mailBody = await razor.GenerateAsync(template.BodyTemplate, item);
            
            var message = new Message
            {
                Subject = $"{status.ToString().ToUpperInvariant()} : " + subject,
                Body = mailBody,
                Id = Strings.GenerateId()
            };
            var email = ObjectBuilder.GetObject<INotificationService>();
            await email.SendMessageAsync(message, ToAddresses);

            return true;

        }
    }
}