using System;

namespace Bespoke.Sph.Domain
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BoundedContextAttribute : Attribute
    {
        public string Name { get; set; }
        public BoundedContextAttribute(string name)
        {
            this.Name = name;
        }
    }
}
