using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class ValueObjectMember : Member
    {
        public override string GetDefaultValueCode(int count, string itemIdentifier = "this")
        {
            return this.AllowMultiple ? null : $"{itemIdentifier}.{Name} = new {ValueObjectName}();";
        }


        public override string GeneratedCode(string padding = "      ")
        {
            if (this.AllowMultiple)
                return padding +
                       $"public ObjectCollection<{ValueObjectName}> {Name} {{get;}} = new ObjectCollection<{ValueObjectName}>();";
            return padding + $"public {ValueObjectName} {Name} {{ get; set;}}";
        }


        public override IEnumerable<Class> GeneratedCustomClass(string codeNamespace, string[] usingNamespaces = null)
        {
            var @class = new Class { Name = this.ValueObjectName, BaseClass = nameof(DomainObject), FileName = $"{ValueObjectName}.cs", Namespace = codeNamespace };
            if (null != usingNamespaces)
            {
                @class.ImportCollection.AddRange(usingNamespaces);
            }
            else
            {
                @class.ImportCollection.Add(typeof(DateTime).Namespace);
                @class.ImportCollection.Add(typeof(Entity).Namespace);
            }
            var classes = new ObjectCollection<Class> { @class };


            var ctor = new StringBuilder();
            // ctor
            ctor.AppendLine($"       public {ValueObjectName}()");
            ctor.AppendLine("       {");
            ctor.AppendLinf("           var rc = new RuleContext(this);");
            var count = 0;
            foreach (var member in this.MemberCollection)
            {
                count++;
                var defaultValueCode = member.GetDefaultValueCode(count);
                if (!string.IsNullOrWhiteSpace(defaultValueCode))
                    ctor.AppendLine(defaultValueCode);
            }
            ctor.AppendLine("       }");
            @class.CtorCollection.Add(ctor.ToString());


            var properties = from m in this.MemberCollection
                             let prop = m.GeneratedCode("   ")
                             select new Property { Code = prop };
            @class.PropertyCollection.ClearAndAddRange(properties);

            var childClasses = this.MemberCollection
                .Select(x => x.GeneratedCustomClass(codeNamespace))
                .Where(x => null != x)
                .SelectMany(x => x.ToArray());
            @classes.AddRange(childClasses);


            return classes;
        }




        public override BuildError[] Validate()
        {
            var errors = new ObjectCollection<BuildError>();

            if (string.IsNullOrWhiteSpace(this.ValueObjectName))
                errors.Add(new BuildError(this.WebId) { Message = $"[Member] {Name} has no ValueObjectDefinition defined" });

            foreach (var m in this.MemberCollection)
            {
                var list = m.Validate();
                errors.AddRange(list);
            }

            return errors.ToArray();
        }

   


        public new ObjectCollection<Member> MemberCollection
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.ValueObjectName))
                    return new ObjectCollection<Member>();

                var context = new SphDataContext();
                var vod = context.LoadOne<ValueObjectDefinition>(d => d.Name == this.ValueObjectName);
                return null == vod ? new ObjectCollection<Member>() : vod.MemberCollection;
            }
        }


        public override IEnumerable<string> GetMembersPath(string root)
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