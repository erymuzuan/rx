using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IMessageTracker
    {
        Task RegisterAcceptanceAsync(MessageTrackingEvent eventData);
        Task RegisterSendingToWorkerAsync(MessageTrackingEvent eventData);
        Task RegisterStartProcessingAsync(MessageTrackingEvent eventData);
        Task RegisterCompletedAsync(MessageTrackingEvent eventData);
        Task RegisterDlqedAsync(MessageTrackingEvent eventData);
        Task RegisterRetriedAsync(MessageTrackingEvent eventData);
        Task RegisterDelayedAsync(MessageTrackingEvent eventData);
        Task<MessageTrackingStatus> GetProcessStatusAsync(MessageSlaEvent @event);
    }

    [Flags]
    public enum MessageTrackingStatus
    {
        NotStarted = 0,
        Started = 1,
        Completed = 2,
        Error = 4,
        Terminated = 8
    }
}