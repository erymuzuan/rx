using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.EmailServiceNotification
{
    public class EmailService : INotificationService
    {
        public string From { get; set; }
        public async Task SendMessageAsync(Message message)
        {
            var context = new SphDataContext();
            var email = await context.GetScalarAsync<UserProfile, string>(u => u.UserName == message.UserName, u => u.Email);
            var smtp = new SmtpClient();
            await smtp.SendMailAsync(this.From ?? "admin@sph.my", email, message.Subject, message.Body);
        }

        public async Task SendMessageAsync(Message message, string to)
        {
            var smtp = new SmtpClient();
            await smtp.SendMailAsync(this.From ?? "admin@sph.my", to, message.Subject, message.Body);
        }

        public IList<INotificationChannel> NotificationChannelCollection { get; } = new List<INotificationChannel>();
    }
}