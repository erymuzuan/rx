using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
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


        public override async Task<bool> ExecuteAsync(MessageTrackingStatus status, MessageSlaEvent @event)
        {
            var context = new SphDataContext();
            Entity item = null;
            var ed = context.LoadOneFromSources<EntityDefinition>(x => x.Name == @event.Entity);
            var repos = this.GetRepository(@event.Entity);
            if (ed.GetPersistenceOption().IsSqlDatabase)
                item = repos.LoadOneAsync(@event.ItemId);

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

        private readonly ConcurrentDictionary<string, dynamic> m_entityRepositories = new ConcurrentDictionary<string, dynamic>();
        private dynamic GetRepository(string entityName)
        {
            if (m_entityRepositories.TryGetValue(entityName, out dynamic r))
                return r;

            var context = new SphDataContext();

            var ed = context.LoadOne<EntityDefinition>(x => x.Name == entityName);
            var sqlAssembly = Assembly.Load("sql.repository");
            var sqlRepositoryType = sqlAssembly.GetType("Bespoke.Sph.SqlRepository.SqlRepository`1");

            var edAssembly = Assembly.Load($"{ConfigurationManager.ApplicationName}.{ed.Name}");
            var edTypeName = $"{ed.CodeNamespace}.{ed.Name}";
            var edType = edAssembly.GetType(edTypeName);
            if (null == edType)
                Console.WriteLine(@"Cannot create type " + edTypeName);

            var reposType = sqlRepositoryType.MakeGenericType(edType);
            dynamic repository = Activator.CreateInstance(reposType);

            m_entityRepositories.TryAdd(entityName, repository);

            return repository;
        }
    }
}