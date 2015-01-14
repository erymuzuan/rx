using System.Linq;
using System.Reflection;
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
            var forbiddenNames = typeof(Workflow).GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(p => p.Name).ToArray();
            const string PATTERN = "^[A-Za-z][A-Za-z0-9_]*$";
            var result = new BuildValidationResult();
            var message = string.Format("[Variable] \"{0}\" is not valid identifier", this.Name);
            var validName = new Regex(PATTERN);
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = message });
            if (forbiddenNames.Contains(this.Name))
                result.Errors.Add(new BuildError(this.WebId) { Message = "[Variable] " + this.Name + " is a reserved variable name" });


            return result;
        }

        public virtual string GetJsonIntance(WorkflowDefinition wd)
        {
            return string.Format("\"{0}\" : null", this.Name);
        }
    }
}
