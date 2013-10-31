using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(SimpleVariable))]
    [XmlInclude(typeof(ComplexVariable))]
    public partial class Variable : DomainObject
    {
        public virtual string GetEmptyJson(WorkflowDefinition wd)
        {
            return "null";
        }
    }
}
