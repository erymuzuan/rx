using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Extensions;
using Polly;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

// ReSharper disable UnassignedGetOnlyAutoProperty

namespace Bespoke.Sph.SqlRepository
{
    public class MessageTracker : IMessageTracker
    {
        private readonly string m_connectionString;
        public bool IsEnabled { get; set; } = true;
        public int RetryCount { get; set; } = 3;
        public TimeSpan WaitDuration { get; set; } = TimeSpan.FromMilliseconds(200);
        public WaitAlgorithm HttpRequestWaitAlgorithm { get; set; } = WaitAlgorithm.Exponential;

        public bool IsSystemTypeEnabled { get; set; } = false;
        private string[] m_trackableEntities;
        private Trigger[] m_enabledTriggers;

        public MessageTracker(string connectionString, bool readFromEnvironmentVariable = false)
        {
            m_connectionString = readFromEnvironmentVariable ? ConfigurationManager.GetEnvironmentVariable(connectionString) : connectionString;
        }

        public MessageTracker()
        {
            m_connectionString = ConfigurationManager.SqlConnectionString;
        }

        public void Initialize()
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
            //
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

            await Policy.Handle<SqlException>(e => e.Message.Contains("timeout"))
                .WaitAndRetryAsync(this.RetryCount, Wait)
                .ExecuteAsync(async () =>
                {
                    using (var conn = new SqlConnection(m_connectionString))
                    using (var cmd = new SqlCommand(@"[Sph].[InsertMessageTrackingEvent]", conn) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.Add(new SqlParameter("@DateTime", SqlDbType.SmallDateTime, 8)).Value =
                            eventData.DateTime;
                        cmd.Parameters.Add(new SqlParameter("@Entity", SqlDbType.NVarChar, 255)).Value =
                            eventData.Entity;
                        cmd.Parameters.Add(new SqlParameter("@Event", SqlDbType.NVarChar, 255)).Value = eventData.Event;
                        cmd.Parameters.Add(new SqlParameter("@ItemId", SqlDbType.NVarChar, 255)).Value =
                            eventData.ItemId;
                        cmd.Parameters.Add(new SqlParameter("@ProcessName", SqlDbType.NVarChar, 255)).Value =
                            eventData.ProcessName.ToDbNull();
                        cmd.Parameters.Add(new SqlParameter("@ProcessingTimeSpan", SqlDbType.NVarChar, 255)).Value =
                            eventData.ProcessingTimeSpan.ToEmptyString().ToDbNull();
                        cmd.Parameters.Add(new SqlParameter("@ProcessingTimeSpanInMiliseconds", SqlDbType.Float)).Value
                            = eventData.ProcessingTimeSpanInMiliseconds.ToDbNull();
                        cmd.Parameters.Add(new SqlParameter("@MachineName", SqlDbType.NVarChar, 255)).Value =
                            eventData.MachineName.ToDbNull();
                        cmd.Parameters.Add(new SqlParameter("@Worker", SqlDbType.NVarChar, 255)).Value =
                            eventData.Worker.ToDbNull();
                        cmd.Parameters.Add(new SqlParameter("@RoutingKey", SqlDbType.NVarChar, 255)).Value =
                            eventData.RoutingKey.ToDbNull();
                        cmd.Parameters.Add(new SqlParameter("@MessageId", SqlDbType.NVarChar, 255)).Value =
                            eventData.MessageId;
                        await conn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                    }
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

        public async Task<MessageTrackingStatus> GetProcessStatusAsync(MessageSlaEvent @event)
        {

            using (var conn = new SqlConnection(m_connectionString))
            using (var cmd = new SqlCommand("[Sph].[GetMessageProcessingStatus]", conn){CommandType = CommandType.StoredProcedure})
            {
                cmd.Parameters.Add(new SqlParameter("@MessageId", SqlDbType.VarChar, 255)).Value = @event.MessageId;
                cmd.Parameters.Add(new SqlParameter("@Worker", SqlDbType.VarChar, 255)).Value = @event.Worker.ToDbNull();

                await conn.OpenAsync();
                var trackings = new List<MessageTrackingEvent>();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        // ReSharper disable UseObjectOrCollectionInitializer
                        var tracking = new MessageTrackingEvent();
                        tracking.MessageId = reader.GetString(0);
                        tracking.Event = reader.GetString(1);

                        // ReSharper restore UseObjectOrCollectionInitializer
                        trackings.Add(tracking);

                    }
                }
                if (trackings.Count == 0) return MessageTrackingStatus.NotStarted;

                if (trackings.Any(x => x.Event == "ProcessingCompleted"))
                    return MessageTrackingStatus.Started | MessageTrackingStatus.Completed;

                if (trackings.Any(x => x.Event == "StartProcessing") && trackings.Count == 1)
                    return MessageTrackingStatus.Started;

                if (trackings.Any(x => x.Event == "StartProcessing") && trackings.Count > 1)
                    return MessageTrackingStatus.Started | MessageTrackingStatus.Error;

                return MessageTrackingStatus.Terminated;
            }
        }
    }
}