using System;

namespace Bespoke.Sph.Mangements
{
    public class DeploymentHistory
    {
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public string Tag { get; set; }
        public string Revision { get; set; }
    }
}