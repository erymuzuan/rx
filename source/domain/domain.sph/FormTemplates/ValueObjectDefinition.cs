using System;
using System.Diagnostics;
using System.IO;

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
    }
}