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
        public string Body { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsOverride { get; set; }
        public bool IsStatic { get; set; }
        [JsonIgnore]
        public Class Class { get; set; }

        private readonly ObjectCollection<string> m_attributeCollection = new ObjectCollection<string>();
        private readonly ObjectCollection<MethodArg> m_argumentCollection = new ObjectCollection<MethodArg>();

        public ObjectCollection<MethodArg> ArgumentCollection
        {
            get { return m_argumentCollection; }
        }

        public ObjectCollection<string> AttributeCollection
        {
            get { return m_attributeCollection; }
        }

        public string GenerateCode()
        {
            if (!string.IsNullOrWhiteSpace(this.Code))
                return this.Code;
            var code = new StringBuilder("//" + this.Comment);

            var args = this.ArgumentCollection.Select(x => string.Format("{0} {1}", x.Type.ToCSharp(), x.Name));

            foreach (var attr in this.AttributeCollection)
            {
                code.AppendLine(attr);
            }
            var asyncModifier = this.Body.Contains("await ") ? "async " : "";
            var overrideModifier = this.IsOverride ? "override " : "";
            var staticModifier = this.IsStatic ? "static " : "";
            var virtualModifier = this.IsStatic ? "static " : "";

            code.AppendFormat("{0} {7}{6}{5}{3}{1} {2}({4})", this.AccessModifier,
                this.ReturnType.ToCSharp(),
                this.Name,
                asyncModifier,
                string.Join(",", args),
                overrideModifier,
                staticModifier,
                virtualModifier);

            code.AppendLine("{");
            code.AppendLine(this.Body);
            code.AppendLine("}");

            return code.ToString();
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