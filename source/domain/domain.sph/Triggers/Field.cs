using System;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(DocumentField))]
    [XmlInclude(typeof(FunctionField))]
    [XmlInclude(typeof(ConstantField))] 
    [XmlInclude(typeof(FieldChangeField))] 
    public partial class Field : DomainObject
    {
        public virtual object GetValue(RuleContext context)
        {
            throw new NotImplementedException("whoaaa");
        }
    }
}