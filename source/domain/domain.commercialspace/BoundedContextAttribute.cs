using System;

namespace Bespoke.Station.Domain
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
