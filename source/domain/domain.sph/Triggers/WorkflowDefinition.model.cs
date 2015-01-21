using System.Collections.Generic;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition : IProjectModel
    {
        string IProjectModel.Name { get { return this.WorkflowTypeName; } }
        public IEnumerable<Member> Members
        {
            get
            {
                var members = this.VariableDefinitionCollection
                    .Select(v => v.CreateMember())
                    .ToList();

                return members.ToArray();
            }
        }
    }
}
