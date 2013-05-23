using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Dependencies
{
    public class EmailService : INotificationService
    {
        public void Send(string[] to, string[] cc, string subject, string body)
        {
            Console.WriteLine("hantar email");
            var message = new MailMessage
            {
                Body = body,
                Subject = subject,
                From = new MailAddress("sph@bph.gov.my"),
                IsBodyHtml = true
            };


            to.ToList().ForEach(message.To.Add);
            if (null != cc)
                cc.ToList().ForEach(message.CC.Add);
            var s = new SmtpClient();
            s.Send(message);
        }

        public IList<INotificationChannel> NotificationChannelCollection { get; private set; }
    }
}