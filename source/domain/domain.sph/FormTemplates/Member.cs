using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class Member : DomainObject
    {
        public string FullName { get; set; }
        public string PropertyAttribute { get; set; }

        public virtual string GetDefaultValueCode(int count)
        {
            return null;
        }
        public virtual string GenerateJavascriptMember(string ns)
        {
            return null;
        }
        public virtual string GenerateJavascriptContructor(string ns)
        {
            return null;
        }
        public virtual string GenerateJavascriptInitValue(string ns)
        {
            return null;
        }
        public virtual string GenerateParameterCode()
        {
            return null;
        }

        /// <summary>
        /// Return the TypeName for used in C# code
        /// </summary>
        /// <returns></returns>
        public virtual string GetMemberTypeName()
        {
            return null;
        }

        public new virtual BuildError[] Validate()
        {
            return new BuildError[] { };
        }

        public virtual string GeneratedCode(string padding = "      ")
        {
            return null;
        }

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

        public virtual string GenerateJavascriptClass(string jns, string csNs, string assemblyName)
        {
            return null;
        }
    }
}