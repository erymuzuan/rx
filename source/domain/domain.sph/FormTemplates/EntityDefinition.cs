using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Bespoke.Sph.Domain
{
    public partial class EntityDefinition : Entity
    {
        private void ValidateMember(Member member, BuildValidationResult result)
        {
            var forbiddenNames =
                typeof(Entity).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Select(p => p.Name)
                    .ToList();
            forbiddenNames.AddRange(new[] { this.Name + "Id", "WebId", "CreatedDate", "CreatedBy", "ChangedBy", "ChangedDate" });

            const string pattern = "^[A-Za-z][A-Za-z0-9_]*$";
            var message = string.Format("[Member] \"{0}\" is not valid identifier", member.Name);
            var validName = new Regex(pattern);
            if (!validName.Match(member.Name).Success)
                result.Errors.Add(new BuildError(member.WebId) { Message = message });
            if (forbiddenNames.Contains(member.Name))
                result.Errors.Add(new BuildError(member.WebId) { Message = "[Member] " + member.Name + " is a reserved name" });

            foreach (var m in member.MemberCollection)
            {
                this.ValidateMember(m, result);
            }
        }

        public BuildValidationResult ValidateBuild()
        {
            var result = new BuildValidationResult();

            var validName = new Regex(@"^[A-Za-z][A-Za-z0-9_]*$");
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Name must be started with letter.You cannot use symbol or number as first character" });

            foreach (var member in this.MemberCollection)
            {
                this.ValidateMember(member, result);
            }


            result.Result = !result.Errors.Any();
            return result;
        }
    }
}