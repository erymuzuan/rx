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

        public virtual string GeneratedCode(WorkflowDefinition workflowDefinition)
        {
            throw new System.NotImplementedException();
        }

    }
}
