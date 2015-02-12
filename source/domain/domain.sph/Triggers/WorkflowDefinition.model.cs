using System.Collections.Generic;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition : IProjectModel
    {
        string IProjectModel.Name { get { return this.WorkflowTypeName; } }
        public IEnumerable<Member> GetMembers()
        {

            var members = this.VariableDefinitionCollection
                .Select(v => v.CreateMember(this))
                .ToList();

            return members;

        }
    }
}
