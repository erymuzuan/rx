using System;
using System.Text;

namespace Bespoke.Sph.Domain.Codes
{
    public class Class
    {
        public Class()
        {
            
        }

        public Class(string code)
        {
            this.m_code = code;
        }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string BaseClass { get; set; }

        private string m_fileName;
        private string m_code;


        public ObjectCollection<string> CtorCollection { get; } = new ObjectCollection<string>();
        public ObjectCollection<string> ImportCollection { get; } = new ObjectCollection<string>();
        public ObjectCollection<string> AttributeCollection { get; } = new ObjectCollection<string>();
        public ObjectCollection<Method> MethodCollection { get; } = new ObjectCollection<Method>();
        public ObjectCollection<Property> PropertyCollection { get; } = new ObjectCollection<Property>();

        public string FileName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_fileName))
                    return this.Name + ".cs";
                return m_fileName;
            }
            set { m_fileName = value; }
        }

        public bool IsPartial { get; set; }

        public void AddNamespaceImport(Type type)
        {
            this.ImportCollection.Add(type.Namespace);
        }

        public string GetCode()
        {
            if (!string.IsNullOrWhiteSpace(m_code))
                return m_code;
            var code = new StringBuilder();
            foreach (var @import in this.ImportCollection)
            {
                var directive = @import.StartsWith("using ") ? $"{@import};" : $"using {@import};";
                code.AppendLine(directive.Replace(";;",";"));
            }
            code.AppendLine();
            code.AppendLine($"namespace {Namespace}");
            code.AppendLine("{");
            foreach (var ctor in this.AttributeCollection)
            {
                code.AppendLine(ctor);
            }
            var partial = this.IsPartial ? "partial" : "";
            var baseClass = !string.IsNullOrWhiteSpace(this.BaseClass) ? $": {BaseClass}" : "";
            code.AppendLine($"   public {partial} class {Name} {baseClass}");
            code.AppendLine("   {");

            foreach (var ctor in this.CtorCollection)
            {
                code.AppendLine(ctor);
            }
            foreach (var prop in this.PropertyCollection)
            {
                code.AppendLine(prop.Code);
            }
            code.AppendLine();
            code.AppendLine();
            foreach (var mtd in this.MethodCollection)
            {
                code.AppendLine(mtd.Comment);
                code.AppendLine(mtd.GenerateCode());
            }

            code.AppendLine("   }");
            code.AppendLine("}");

            return code.FormatCode();

        }

        public void AddMethod(string format, params object[] args)
        {
            this.MethodCollection.Add(new Method { Code = string.Format(format, args) });
        }
        public void AddProperty(string format, params object[] args)
        {
            this.PropertyCollection.Add(new Property { Code = string.Format(format, args) });
        }
        public void AddProperty(string name, Type type)
        {
            this.PropertyCollection.Add(new Property {Name = name, Type = type });
        }
    }
}