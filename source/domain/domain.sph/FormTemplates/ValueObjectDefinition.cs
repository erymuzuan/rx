using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Name = {Name}")]
    [StoreAsSource(HasDerivedTypes = true)]
    public partial class ValueObjectDefinition : Entity
    {
        public void Save()
        {
            string childJsonFile = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(ValueObjectDefinition)}\\{Id}.json";
            File.WriteAllText(childJsonFile, this.ToJsonString(true));
        }

        public BuildValidationResult CanSave()
        {
            return new BuildValidationResult { Result = true };
        }

        public Member AddMember<T>(string name, bool allowMultiple = false, bool nullalbe = true, bool filterable = false, int boost = 1)
        {
            var guid = Guid.NewGuid().ToString();
            var member = new SimpleMember
            {
                WebId = guid,
                Name = name,
                Type = typeof(T),
                IsNullable = nullalbe,
                IsFilterable = filterable,
                Boost = boost,
                AllowMultiple = allowMultiple
            };
            this.MemberCollection.Add(member);

            return member;
        }

        public Task<string[]> CanDeleteAsync()
        {
            var warnings = new List<string>();
            var context = new SphDataContext();
            var wds = context.LoadFromSources<WorkflowDefinition>(x => x.Id != "0");
            foreach (var wd in wds)
            {
                if (wd.VariableDefinitionCollection.OfType<ValueObjectVariable>().Any(c => c.TypeName == this.Name))
                    warnings.Add($"You cannot delete {Name} because {wd.Name} WorkflowDefinition is referring to it");
            }
            var eds = context.LoadFromSources<EntityDefinition>(x => x.Id != "0");
            foreach (var ed in eds)
            {
                var results = this.CanDelete(ed.MemberCollection, ed.Name);
                warnings.AddRange(results);
            }
            var vods = context.LoadFromSources<ValueObjectDefinition>(x => x.Id != this.Id);
            foreach (var vo in vods)
            {
                var results = this.CanDelete(vo.MemberCollection, vo.Name, nameof(ValueObjectDefinition));
                warnings.AddRange(results);
            }

            return Task.FromResult(warnings.ToArray());
        }

        private string[] CanDelete(IReadOnlyCollection<Member> members, string parent, string parentType = "EntityDefinition")
        {
            var warnings = new List<string>();
            var ok = members.OfType<ValueObjectMember>().Any(x => x.Name == Name);
            if (ok) warnings.Add($"{Name} is referred by {parent} {parentType}");
            foreach (var mb in members.OfType<ComplexMember>())
            {
                var results = this.CanDelete(mb.MemberCollection, parent);
                warnings.AddRange(results);
            }

            return warnings.ToArray();

        }
    }
}