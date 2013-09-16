using System.Collections.Generic;

namespace Bespoke.Sph.Domain
{
    public interface INotificationService
    {
        IList<INotificationChannel> NotificationChannelCollection { get; }
    }
}
