using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class SimpleMember : Member
    {

        
        public override string GetDefaultValueCode(int count, string itemIdentifier = "this")
        {
            if (null == this.DefaultValue) return null;
            var codeg = this.DefaultValue.GenerateCode();
            if (!string.IsNullOrWhiteSpace(codeg))
                return $"{itemIdentifier}.{Name} = {codeg};";

            var json = this.DefaultValue.ToJsonString().Replace("\"", "\\\"");
            var typeName = this.DefaultValue.GetType().Name;

            var code = new StringBuilder();
            code.AppendLine($"           var mj{count} = \"{json}\";");
            code.AppendLine($"           var field{count} = mj{count}.DeserializeFromJson<{typeName}>();");
            code.AppendLine($"           var val{count} = field{count}.GetValue(rc);");
            code.AppendLine($"           {itemIdentifier}.{Name} = ({Type.FullName})val{count};");

            return code.ToString();

        }

        public override string GetMemberTypeName()
        {
            return this.Type.ToCSharp();
        }

        public override string GenerateParameterCode()
        {
            return $"{Type.ToCSharp()} {Name.ToCamelCase()}";
        }

        protected string GetCsharpType()
        {
            return this.Type.ToCSharp();
        }

        public void Add(Dictionary<string, Type> members)
        {
            foreach (var k in members.Keys)
            {
                this.MemberCollection.Add(new SimpleMember { Name = k, Type = members[k] });
            }
        }
        protected string GetNullable()
        {
            if (!this.IsNullable) return string.Empty;
            if (typeof(byte[]) == this.Type) return string.Empty;
            if (typeof(string) == this.Type) return string.Empty;
            if (typeof(object) == this.Type) return string.Empty;
            if (typeof(Array) == this.Type) return string.Empty;
            if (this.Type.IsClass) return string.Empty;
            return "?";
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
                var returnType = this.AllowMultiple ? $"IEnumerable<{Type.ToCSharp()}>" : $"{Type.ToCSharp()}";
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
            if (null == this.Type)
                throw new InvalidOperationException(this + " doesn't have a type");
            var code = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(PropertyAttribute))
                code.AppendLine(padding + PropertyAttribute);

            if (this.AllowMultiple)
                code.AppendLine(padding + $"public  ObjectCollection<{this.GetCsharpType()}> {Name} {{ get; }} = new ObjectCollection<{this.GetCsharpType()}>();");
            else
                code.AppendLine(padding + $"public {this.GetCsharpType()}{this.GetNullable()} {Name} {{ get; set; }}");
            return code.ToString();
        }

        [JsonIgnore]
        public virtual Type Type
        {
            get => Strings.GetType(this.TypeName);
            set => this.TypeName = value.GetShortAssemblyQualifiedName();
        }

        public override string ToString()
        {
            return $"[SimpleMember]{this.Name}->{this.Type.ToCSharp()}";
        }
    }
}