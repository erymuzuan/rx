using System;
using System.IO;

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
    }
}