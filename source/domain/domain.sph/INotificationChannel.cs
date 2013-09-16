using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface INotificationChannel
    {
        Task Send(EmailMessage message);
    }
}
