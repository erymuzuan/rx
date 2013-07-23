using System;

namespace Bespoke.SphCommercialSpaces.Domain
{
    [System.Xml.Serialization.XmlInclude(typeof(DocumentField))]
    [System.Xml.Serialization.XmlInclude(typeof(FuctionField))]
    [System.Xml.Serialization.XmlInclude(typeof(ConstantField))]
    public abstract class Field : DomainObject
    {
        public virtual object GetValue(Entity item)
        {
            throw new NotImplementedException("whoaaa");
        }
    }
}