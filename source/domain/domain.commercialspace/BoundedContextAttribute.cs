using System;

namespace Bespoke.SphCommercialSpaces.Domain
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
