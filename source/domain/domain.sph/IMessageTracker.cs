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
    }

    public interface IMessageSlaManager
    {
        Task PublishSlaOnAcceptanceAsync(MessageSlaEvent @event);
    }
}