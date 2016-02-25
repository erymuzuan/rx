using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class SimpleMember : Member
    {
        public override string GenerateJavascriptMember(string ns)
        {
            return this.AllowMultiple ?
                $"     {Name}: ko.observableArray([])," :
                $"     {Name}: ko.observable(),";
        }
        public override string GetDefaultValueCode(int count)
        {
            if (null == this.DefaultValue) return null;

            var json = this.DefaultValue.ToJsonString().Replace("\"", "\\\"");
            var typeName = this.DefaultValue.GetType().Name;

            var code = new StringBuilder();
            code.AppendLine($"           var mj{count} = \"{json}\";");
            code.AppendLine($"           var field{count} = mj{count}.DeserializeFromJson<{typeName}>();");
            code.AppendLine($"           var val{count} = field{count}.GetValue(rc);");
            code.AppendLine($"           this.{Name} = ({Type.FullName})val{count};");

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
            return "?";
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
            get
            {
                return Strings.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }

        public override string ToString()
        {
            return $"[SimpleMember]{this.Name}->{this.Type.ToCSharp()}";
        }
    }
}