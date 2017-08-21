using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IMessageSlaManager
    {
        Task PublishSlaOnAcceptanceAsync(MessageSlaEvent @event);
        Task ExecuteOnNotificationAsync(MessageTrackingStatus status, MessageSlaEvent @event);
    }
}