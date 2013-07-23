using System;
using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    [XmlInclude(typeof(DocumentField))]
    [XmlInclude(typeof(FunctionField))]
    [XmlInclude(typeof(ConstantField))]
    public partial class Field : DomainObject
    {
        public virtual object GetValue(Entity item)
        {
            throw new NotImplementedException("whoaaa");
        }
    }
}