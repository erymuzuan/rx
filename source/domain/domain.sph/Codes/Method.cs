using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Codes
{
    public class Method
    {
        public string Comment { get; set; }
        public Modifier AccessModifier { get; set; }
        public Type ReturnType { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Code { get; set; }

        public string Body
        {
            get
            {
                return !string.IsNullOrWhiteSpace(m_body) ? m_body : m_tempBody.ToString();
            }
            set { m_body = value; }
        }

        public bool IsVirtual { get; set; }
        public bool IsOverride { get; set; }
        public bool IsStatic { get; set; }
        [JsonIgnore]
        public Class Class { get; set; }

        public ObjectCollection<MethodArg> ArgumentCollection { get; } = new ObjectCollection<MethodArg>();

        public ObjectCollection<string> AttributeCollection { get; } = new ObjectCollection<string>();

        public bool IsPartial { get; set; }
        public string ReturnTypeName { get; set; }

        public string GenerateCode()
        {
            if (!string.IsNullOrWhiteSpace(this.Code))
                return this.Code;
            var code = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(this.Comment))
                code.AppendLine("//" + this.Comment);
            
            var args = this.ArgumentCollection.Select(x => x.ToString());

            foreach (var attr in this.AttributeCollection)
            {
                code.AppendLine(attr);
            }
            var asyncModifier = this.Body.Contains("await ") ? "async " : "";
            var overrideModifier = this.IsOverride ? "override " : "";
            var staticModifier = this.IsStatic ? "static " : "";
            var virtualModifier = this.IsStatic ? "static " : "";
            var partialModifier = this.IsPartial ? " partial " : "";

            var argSignature = string.Join(",", args);
            var retType =this.ReturnType == null ? this.ReturnTypeName: this.ReturnType.ToCSharp();
            var signature = string.Format("{0} {8}{7}{6}{5}{3}{1} {2}({4})", AccessModifier.ToString().ToLowerInvariant(),
                retType,
                this.Name,
                asyncModifier,
                argSignature,
                overrideModifier,
                staticModifier,
                virtualModifier,
                partialModifier);
            if (IsPartial)
                signature = $"partial void {Name}({argSignature})";
            code.AppendLine();
            code.AppendLine(signature);
            code.AppendLine("{");
            code.AppendLine(this.Body);
            code.AppendLine("}");

            return code.ToString();
        }

        private readonly StringBuilder m_tempBody = new StringBuilder();
        private string m_body;

        public void AppendLine(string line = null)
        {
            if (string.IsNullOrWhiteSpace(line))
                m_tempBody.AppendLine();
            else
                m_tempBody.AppendLine(line);
        }
    }

    public enum Modifier
    {
        Internal,
        Public,
        Private,
        Protected
    }
}