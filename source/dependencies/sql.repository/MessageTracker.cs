using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Extensions;
using Newtonsoft.Json;
using Polly;

namespace Bespoke.Sph.SqlRepository
{
    public class MessageTracker : IMessageTracker
    {
        private readonly string m_connectionString;
        public bool IsEnabled { get; }
        public int RetryCount { get; set; } = 3;
        public TimeSpan WaitDuration { get; set; } = TimeSpan.FromMilliseconds(200);
        public WaitAlgorithm HttpRequestWaitAlgorithm { get; set; } = WaitAlgorithm.Exponential;

        public bool IsSystemTypeEnabled { get; set; } = false;
        private string[] m_trackableEntities;
        private Trigger[] m_enabledTriggers;

        public MessageTracker(string connectionString)
        {
            m_connectionString = connectionString;
        }
        public MessageTracker()
        {
            m_connectionString = ConfigurationManager.SqlConnectionString;
        }

        public void Initialize()
        {
            this.InitializeAsync().Wait(9000);
        }

        public async Task InitializeAsync()
        {
            var logger = new ConsoleLogger { TraceSwitch = Severity.Debug };
            // NOTE : do not use SphDataContext since it required the calls to RegistryContext, this is initialization code
            m_enabledTriggers = Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\Trigger", "*.json")
                .Select(x => x.DeserializeFromJsonFile<Trigger>())
                .Where(x => x.EnableTracking)
                .ToArray();

            m_trackableEntities = Directory
                .GetFiles($"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition", "*.json")
                .Select(x => x.DeserializeFromJsonFile<EntityDefinition>())
                .Where(x => x.EnableTracking)
                .Select(x => x.Name)
                .ToArray();
            logger.WriteInfo($"Tracking entities : {m_trackableEntities.ToString(", ")}");
            logger.WriteInfo($"Tracking triggers : {m_enabledTriggers.ToString(", ")}");

            const string TABLE_SQL = @"

CREATE TABLE [Sph].[MessageEvent]
()";
            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand(TABLE_SQL, conn))
            {
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

        }


        private bool CanTrack(MessageTrackingEvent @event)
        {
            if (!this.IsEnabled) return false;
            if (!IsSystemTypeEnabled && @event.EntityNamespace == typeof(EntityDefinition).Namespace) return false;
            if (!IsSystemTypeEnabled && @event.EntityNamespace == typeof(Adapter).Namespace) return false;

            if (!m_trackableEntities.Contains(@event.Entity)) return false;

            var workerIsTracked = m_enabledTriggers.Any(x => $"trigger_subs_{x.Id}" == @event.Worker);
            if (!string.IsNullOrWhiteSpace(@event.Worker))
                return workerIsTracked;

            return true;
        }

        private async Task PostEventAsync(MessageTrackingEvent eventData)
        {
            if (!CanTrack(eventData)) return;
            if (!IsEnabled) return;

            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(eventData, setting);


            TimeSpan Wait(int c)
            {
                switch (HttpRequestWaitAlgorithm)
                {
                    case WaitAlgorithm.Linear:
                        return TimeSpan.FromMilliseconds(this.WaitDuration.TotalMilliseconds * c);
                    case WaitAlgorithm.Exponential:
                        return TimeSpan.FromMilliseconds(this.WaitDuration.TotalMilliseconds * Math.Pow(2, c));
                    case WaitAlgorithm.Constant:
                        return WaitDuration;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(this.RetryCount, Wait)
                .ExecuteAsync(async () =>
                {

                    await Task.Delay(100);
                });
        }
        public async Task RegisterAcceptanceAsync(MessageTrackingEvent @event)
        {
            ObjectBuilder.GetObject<ILogger>().WriteVerbose($"Accepted {@event.Entity}{{{@event.ItemId}}}");
            @event.Event = "Accepted";
            await PostEventAsync(@event);

            var should = m_trackableEntities.Contains(@event.Entity);
            if (should)
            {
                await this.InitializeMessageSlaAsyc("Accepted", @event);
            }
        }


        private async Task InitializeMessageSlaAsyc(string eventName, MessageTrackingEvent data)
        {
            var events = (from trg in m_enabledTriggers
                          where trg.ShouldProcessedOnceAccepted > 0
                                && trg.Entity == data.Entity
                          select new MessageSlaEvent
                          {
                              Event = eventName,
                              ItemId = data.ItemId,
                              Entity = data.Entity,
                              DateTime = data.DateTime,
                              MessageId = data.MessageId,
                              ProcessingTimeSpanInMiliseconds = trg.ShouldProcessedOnceAccepted.Value,
                              Worker = $"trigger_subs_{trg.Id}"
                          }).ToArray();
            //
            var slaManager = ObjectBuilder.GetObject<IMessageSlaManager>();
            var monitorTasks = events.Select(c => slaManager.PublishSlaOnAcceptanceAsync(c));
            await Task.WhenAll(monitorTasks);
        }
        public async Task RegisterSendingToWorkerAsync(MessageTrackingEvent eventData)
        {
            eventData.Event = "SendingToWorker";
            await PostEventAsync(eventData);
        }

        public async Task RegisterStartProcessingAsync(MessageTrackingEvent eventData)
        {
            eventData.Event = "StartProcessing";
            await PostEventAsync(eventData);
        }

        public async Task RegisterCompletedAsync(MessageTrackingEvent eventData)
        {
            eventData.Event = "ProcessingCompleted";
            await PostEventAsync(eventData);
        }

        public async Task RegisterCancelledAsync(MessageTrackingEvent @event)
        {
            @event.Event = "Cancelled";
            await PostEventAsync(@event);
        }

        public async Task RegisterCancelRequestedAsync(MessageTrackingEvent @event)
        {
            @event.Event = "CancelRequested";
            await PostEventAsync(@event);
        }

        public async Task RegisterDlqedAsync(MessageTrackingEvent eventData)
        {
            eventData.Event = "DLQ";
            await PostEventAsync(eventData);
        }

        public async Task RegisterRetriedAsync(MessageTrackingEvent eventData)
        {
            eventData.Event = "Retried";
            await PostEventAsync(eventData);
        }

        public async Task RegisterDelayedAsync(MessageTrackingEvent eventData)
        {
            eventData.Event = "Delayed";
            await PostEventAsync(eventData);
        }

        public Task<MessageTrackingStatus> GetProcessStatusAsync(MessageSlaEvent @event)
        {
            throw new NotImplementedException();
        }

    }
}
