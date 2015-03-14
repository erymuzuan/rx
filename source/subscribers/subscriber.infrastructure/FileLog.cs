using System;
using System.IO;
using System.Text;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public class FileLog : INotificationService
    {
        public string LogFile { get; set; }

        public void Write(string format, params object[] args)
        {
            File.AppendAllText(LogFile, string.Format(format, args) + "\r\n");
        }

        public void WriteError(string format, params object[] args)
        {
            File.AppendAllText(LogFile, "Error : " + string.Format(format, args) + "\r\n");
        }

        public void WriteError(Exception exception, string message2)
        {
            var message = new StringBuilder();
            var exc = exception;
            var aeg = exc as AggregateException;
            if (null != aeg)
            {
                foreach (var ie in aeg.InnerExceptions)
                {
                    this.WriteError(ie,"");
                }

            }
            while (null != exc)
            {
                message.AppendLine(exc.GetType().FullName);
                message.AppendLine(exc.Message);
                message.AppendLine(exc.StackTrace);
                message.AppendLine();
                message.AppendLine();
                exc = exc.InnerException;
            }
            this.WriteError(message.ToString());
        }
    }
}