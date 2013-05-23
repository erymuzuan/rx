using System.Collections.Generic;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface INotificationService
    {
        IList<INotificationChannel> NotificationChannelCollection { get; }
    }
}
