using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticSearchLogger
{
    class Log
    {
        public string Message { get; set; }
        public string Operation { get; set; }
        public DateTime DateTime { get; set; }
        public Severity Severity { get; set; }
        public LogEntry Entry { get; set; }
        public string UserName { get; set; }
        public string ExceptionType { get; set; }
    }
}