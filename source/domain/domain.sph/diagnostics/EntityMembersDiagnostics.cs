using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    sealed class EntityMembersDiagnostics : BuilDiagnostic
    {
        public override Task<BuildError[]> ValidateErrorsAsync(EntityDefinition entity)
        {
            var errors = new List<BuildError>();
            foreach (var member in entity.MemberCollection)
            {
                this.ValidateMember(member, errors, entity);
            }
            return Task.FromResult(errors.ToArray());
        }
        private void ValidateMember(Member member, IList<BuildError> errors, EntityDefinition ed)
        {
            var forbiddenNames =
                typeof(Entity).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Select(p => p.Name)
                    .ToList();
            forbiddenNames.AddRange(new[] { ed.Name + "Id", "WebId", "CreatedDate", "CreatedBy", "ChangedBy", "ChangedDate" });

            const string PATTERN = "^[A-Za-z][A-Za-z0-9_]*$";
            var message = $"[Member] \"{member.Name}\" is not valid identifier";
            var validName = new Regex(PATTERN);
            if (!validName.Match(member.Name).Success)
                errors.Add(new BuildError(member.WebId) { Message = message });
            if (forbiddenNames.Contains(member.Name))
                errors.Add(new BuildError(member.WebId) { Message = $"[Member] {member.Name} is a reserved name" });

            var list = member.Validate();
            list.ToList().ForEach(errors.Add);

        }
    }
}