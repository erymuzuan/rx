using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(SimpleVariable))]
    [XmlInclude(typeof(ComplexVariable))]
    [XmlInclude(typeof(ClrTypeVariable))]
    public partial class Variable : DomainObject
    {
        public virtual string GeneratedCode(WorkflowDefinition workflowDefinition)
        {
            throw new System.NotImplementedException();
        }

        public virtual BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            const string pattern = "^[A-Za-z][A-Za-z0-9_]*$";
            var result = new BuildValidationResult();
            var message = string.Format("[Variable] \"{0}\" is not valid identifier", this.Name);
            var validName = new Regex(pattern);
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = message });


            return result;
        }
    }
}
