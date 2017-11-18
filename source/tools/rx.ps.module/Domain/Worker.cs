using System;

namespace Bespoke.Sph.RxPs.Domain
{
    public class Worker
    {
        public string Name { get; set; }
        public int? Pid { get; set; }
        public string Environment { get; set; }
        public string Configuration { get; set; }
        public DateTime? StartTime { get; set; }
        public Subscriber[] Subscribers { get; set; }
    }

    public class Subscriber
    {
        public string Name { get; set; }
        public string Queue { get; set; }
        public int InstancesCount { get; set; }
    }
}
