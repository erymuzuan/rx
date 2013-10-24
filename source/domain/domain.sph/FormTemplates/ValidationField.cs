using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(DocumentField))]
    [XmlInclude(typeof(FunctionField))]
    [XmlInclude(typeof(ConstantField))]
    [XmlInclude(typeof(FieldChangeField))] 
    partial class ValidationField : DomainObject
    {
    }
}
