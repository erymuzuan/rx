using System;

namespace Bespoke.Station.Domain
{
    public class EntityTypeAttribute : Attribute
    {
        public EntityTypeAttribute(Type type)
        {
            this.Type = type;
        }
        public Type Type { get; set; }
    }
}