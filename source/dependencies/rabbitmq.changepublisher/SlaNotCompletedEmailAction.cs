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
            var email = ObjectBuilder.GetObject<INotificationService>();
            if (null == item)
            {

                await email.SendMessageAsync(new Message
                {
                    Subject = $"Action is cancelled : {@event.MessageId}",
                    Body = $@"Item {@event.ItemId} cannot be found",
                    Id = Guid.NewGuid().ToString("N")
                }, ToAddresses);
                return false;
            }

            var entityName = item.GetType().Name;
            var templateId = this.EmailTemplateMapping.ToEmptyString()
                .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(x => x.StartsWith($"{entityName}"))
                .ToEmptyString()
                .Replace($"{entityName}:", "");
            if (string.IsNullOrWhiteSpace(templateId)) return true;

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
            await email.SendMessageAsync(message, ToAddresses);
            return true;

        }
    }
}