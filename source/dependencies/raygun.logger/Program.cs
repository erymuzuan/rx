using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Mindscape.Raygun4Net;

namespace Bespoke.Sph.RayGunLoggers
{
    [Export(typeof(ILogger))]
    public class Logger : ILogger
    {


        private readonly RaygunClient m_client = new RaygunClient("imHU3x8eZamg84BwYekfMQ==");
        private readonly List<string> m_tags = new List<string>();
        public Logger()
        {
            m_tags.Add("ApplicationName");
        }

        public Severity TraceSwitch { get; set; }

        public Task LogAsync(LogEntry entry)
        {
            if ((int)entry.Severity < (int)this.TraceSwitch) return Task.FromResult(0);
            if (null == entry.Exception) return Task.FromResult(0);

            m_client.SendInBackground(entry.Exception);

            return Task.FromResult(0);
        }


        public void Log(LogEntry entry)
        {
            if ((int)entry.Severity < (int)this.TraceSwitch) return;
            if (null == entry.Exception) return;

            m_client.Send(entry.Exception);
        }
    }
}
