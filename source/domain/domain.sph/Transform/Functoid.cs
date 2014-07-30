using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(ScriptFunctoid))]
    [XmlInclude(typeof(ConstantFunctoid))]
    [XmlInclude(typeof(StringConcateFunctoid))]
    public partial class Functoid : DomainObject
    {

        public virtual string GeneratePreCode(FunctoidMap map)
        {
            return string.Empty;
        }
        public virtual string GenerateCode()
        {
            return string.Format("// NOT IMPLEMENTED => {0}", this.GetType().Name);
        }
    }
}