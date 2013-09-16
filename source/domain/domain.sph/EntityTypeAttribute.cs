using System;

namespace Bespoke.Sph.Domain
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