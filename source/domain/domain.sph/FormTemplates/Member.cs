using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class Member : DomainObject
    {
        public string FullName { get; set; }
        public string PropertyAttribute { get; set; }

        public virtual string GetDefaultValueCode(int count, string itemIdentifier = "this")
        {
            return null;
        }
        [Obsolete("Move to Csharp compiler")]
        public virtual string GenerateParameterCode()
        {
            return null;
        }

        [Obsolete("Move to Csharp compiler")]
        public virtual string GenerateInitializeValueCode(Field initialValue, string itemIdentifier = "item")
        {
            if (this.AllowMultiple)
                return $"{itemIdentifier}.{Name}.ClearAndAddRange({initialValue.GenerateCode()});";
            return $"{itemIdentifier}.{Name} = {initialValue.GenerateCode()};";

        }

        /// <summary>
        /// Return the TypeName for used in C# code
        /// </summary>
        /// <returns></returns>
        [Obsolete("Move to Csharp compiler")]
        public virtual string GetMemberTypeName()
        {
            return null;
        }

        public new virtual BuildError[] Validate()
        {
            return new BuildError[] { };
        }

        [Obsolete("Move to Csharp compiler")]
        public virtual string GeneratedCode(string padding = "      ")
        {
            return null;
        }

        [Obsolete("Move to Csharp compiler")]
        public virtual IEnumerable<Class> GeneratedCustomClass(string codeNamespace, string[] usingNamespaces = null)
        {
            return new Class[] { };
        }

        public new Member this[string index]
        {
            get { return this.MemberCollection.Single(m => m.Name == index); }
        }

        public virtual IEnumerable<string> GetMembersPath(string root)
        {
            var list = new List<string>();
            list.AddRange(this.MemberCollection.Select(a => $"{root}{this.Name}.{a.Name}"));
            foreach (var member in this.MemberCollection)
            {
                list.AddRange(member.GetMembersPath($"{root}{this.Name}."));
            }
            return list.ToArray();
        }

    }
}