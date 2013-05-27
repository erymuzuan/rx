using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface INotificationChannel
    {
        Task Send(EmailMessage message);
    }
}
