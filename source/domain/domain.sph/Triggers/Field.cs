using System;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(DocumentField))]
    [XmlInclude(typeof(FunctionField))]
    [XmlInclude(typeof(ConstantField))]
    [XmlInclude(typeof(JavascriptExpressionField))]
    [XmlInclude(typeof(PropertyChangedField))]
    [XmlInclude(typeof(AssemblyField))]
    public partial class Field : DomainObject
    {
        public virtual object GetValue(RuleContext context)
        {
            throw new NotImplementedException("whoaaa");
        }

        public virtual  string GenerateCode()
        {
            return string.Format("// NOT IMPLEMENTED => {0}", this.GetType().Name);
        }
    }
}