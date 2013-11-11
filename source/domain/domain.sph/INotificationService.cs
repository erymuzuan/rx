using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface INotificationService
    {
        Task SendMessageAsync(Message message);
        Task SendMessageAsync(Message message, string to);
        IList<INotificationChannel> NotificationChannelCollection { get; }
    }
}
