using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ComplexMember : Member
    {
        public override string ToString()
        {
            return $"[ComplexMember]{this.Name}; AllowMultiple = {AllowMultiple}, Members = {MemberCollection.Count}";
        }

        public Member AddMember<T>(string name, bool filterable = false, bool nullable = false, bool allowMultiple = false, Field defaultValue = null)
        {
            if (typeof(T).IsSubclassOf(typeof(Member)))
            {
                var ctor = typeof(T).GetConstructor(Array.Empty<Type>());
                if (null == ctor) throw new ArgumentException("No default ctor");
                if (ctor.Invoke(new object[] { }) is Member mbr)
                {
                    mbr.Name = name;
                    mbr.WebId = Strings.GenerateId();
                    this.MemberCollection.Add(mbr);
                    return mbr;
                }
            }
            var sm = new SimpleMember
            {
                Name = "Created",
                Type = typeof(T),
                IsFilterable = filterable,
                IsNullable = nullable,
                DefaultValue = defaultValue,
                AllowMultiple = allowMultiple

            };
            this.MemberCollection.Add(sm);
            return sm;
        }


        public override string GetDefaultValueCode(int count, string itemIdentifier = "this")
        {
            if (null == this.DefaultValue)
            {
                return this.AllowMultiple ?
                    null :
                    $"           {itemIdentifier}.{Name} = new {TypeName}();";
            }
            var dv = this.DefaultValue.GenerateCode();
            if (!string.IsNullOrWhiteSpace(dv))
                return $"{itemIdentifier}.{Name} = {dv};";

            return this.AllowMultiple ?
                null :
                $"           {itemIdentifier}.{Name} = new {TypeName}();";
        }

        public override string GenerateInitializeValueCode(Field initialValue, string itemIdentifier = "item")
        {

            var code = new StringBuilder();
            var sc = initialValue.GenerateCode();
            var count = Name.ToPascalCase();
            if (sc.EndsWith(";"))
            {
                var asyncLambda = sc.Contains("await ");
                var assignment = $"__f{count}()";
                var returnType = this.AllowMultiple ? $"IEnumerable<{TypeName}>" : $"{TypeName}";
                if (asyncLambda)
                {
                    code.AppendLine($"          Func<Task<{returnType}>> __f{count}Async = async () =>{{{sc}}};");
                    assignment = $"await __f{count}Async()";
                }
                else
                {
                    code.AppendLine($"          Func<{returnType}> __f{count} = () =>{{{sc}}};");

                }

                // assigment
                if (this.AllowMultiple)
                {
                    code.AppendLine($"          item.{Name}.Clear();");
                    code.AppendLine($"          item.{Name}.AddRange({assignment});");
                }
                else
                {
                    code.AppendLine($"          item.{Name} = {assignment};");
                }
                return code.ToString();
            }
            return base.GenerateInitializeValueCode(initialValue, itemIdentifier);

        }

        public override string GeneratedCode(string padding = "      ")
        {
            var code = new StringBuilder();
            if (this.AllowMultiple)
                code.AppendLine(padding + $"public  ObjectCollection<{TypeName}> {Name} {{ get; }} = new ObjectCollection<{TypeName}>();");
            else
                code.AppendLine(padding + $"public {TypeName} {Name} {{ get; set;}}");

            return code.ToString();

        }
        public override IEnumerable<Class> GeneratedCustomClass(string codeNamespace, string[] usingNamespaces = null)
        {
            var @class = new Class { Name = this.TypeName, BaseClass = nameof(DomainObject), FileName = $"{TypeName}.cs", Namespace = codeNamespace };
            if (null != usingNamespaces)
            {
                @class.ImportCollection.AddRange(usingNamespaces);
            }
            else
            {
                @class.AddNamespaceImport<DateTime, Entity, JsonPropertyAttribute>();
            }

            var classes = new ObjectCollection<Class> { @class };



            var ctor = new StringBuilder($"public {@class.Name}()");
            ctor.AppendLine("{");
            ctor.AppendLine("  var rc = new RuleContext(this);");
            var count = 0;
            foreach (var member in this.MemberCollection)
            {
                count++;
                var defaultValueCode = member.GetDefaultValueCode(count);
                if (!string.IsNullOrWhiteSpace(defaultValueCode))
                    ctor.AppendLine(defaultValueCode);
            }
            ctor.AppendLine("}");
            @class.CtorCollection.Add(ctor.ToString());


            var properties = from m in this.MemberCollection
                             let prop = m.GeneratedCode("   ")
                             select new Property { Code = prop };
            @class.PropertyCollection.ClearAndAddRange(properties);

            var childClasses = this.MemberCollection
                .Select(x => x.GeneratedCustomClass(codeNamespace))
                .Where(x => null != x)
                .SelectMany(x => x.ToArray());
            classes.AddRange(childClasses);

            return classes;
        }

        public void Add(Dictionary<string, Type> dictionary)
        {
            foreach (var member in dictionary.Keys)
            {
                this.MemberCollection.Add(new SimpleMember { Name = member, Type = dictionary[member] });
            }
        }

        public override BuildError[] Validate()
        {
            var errors = new List<BuildError>(base.Validate());

            const string PATTERN = "^[A-Za-z][A-Za-z0-9_]*$";
            var validName = new Regex(PATTERN);

            var typeNameInvalid = $"[Member] \"{Name}\" TypeName is not valid identifier";
            if (string.IsNullOrWhiteSpace(TypeName))
                errors.Add(new BuildError(WebId) { Message = $"[Member] \"{Name}\" TypeName cannot be empty" });
            if (!validName.Match(TypeName).Success)
                errors.Add(new BuildError(WebId) { Message = typeNameInvalid });

            return errors.ToArray();
        }
    }
}