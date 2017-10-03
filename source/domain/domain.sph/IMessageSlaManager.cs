using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IMessageSlaManager
    {
        Task PublishSlaOnAcceptanceAsync(MessageSlaEvent @event);
        Task ExecuteOnNotificationAsync(MessageTrackingStatus status, MessageSlaEvent @event);
        Task<bool> CheckMessageIsValidAndMarkReceivedAsync(string messageId, string worker);
    }

    public interface ICancelledMessageRepository
    {
        // TODO : profile this method... got to be very fast, a few ms
        Task<bool> CheckMessageAsync(string messageId, string worker);
        Task PutAsync(string messageId, string worker);
        Task RemoveAsync(string messageId, string worker);
    }
}