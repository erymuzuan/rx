using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class Variable : DomainObject
    {
        public virtual string GeneratedCode(WorkflowDefinition wd)
        {
            throw new System.NotImplementedException();
        }
        public virtual string GeneratedCtorCode(WorkflowDefinition wd)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<IEnumerable<Class>> GenerateCustomTypesAsync(WorkflowDefinition wd)
        {
            var tcs = new TaskCompletionSource<IEnumerable<Class>>();
            tcs.SetResult(new Class[] { });

            return tcs.Task;
        }
        public virtual Task<string> GenerateCustomJavascriptAsync(WorkflowDefinition wd)
        {
            return Task.FromResult(default(string));
        }

        public virtual BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var forbiddenNames = typeof(Workflow).GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(p => p.Name).ToArray();
            const string PATTERN = "^[A-Za-z][A-Za-z0-9_]*$";
            var result = new BuildValidationResult();
            var message = $"[Variable] \"{this.Name}\" is not valid identifier";
            var validName = new Regex(PATTERN);
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = message });
            if (forbiddenNames.Contains(this.Name))
                result.Errors.Add(new BuildError(this.WebId) { Message = "[Variable] " + this.Name + " is a reserved variable name" });


            return result;
        }

        public virtual Task<string[]> GetMembersPathAsync(WorkflowDefinition wd)
        {
            return Task.FromResult(new string[] { });
        }
    }
}
