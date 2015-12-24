using System;
using System.Text;

namespace Bespoke.Sph.Domain.Codes
{
    public class Class
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string BaseClass { get; set; }

        private string m_fileName;

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
            var code = new StringBuilder();
            foreach (var @import in this.ImportCollection)
            {
                code.AppendFormat("using {0};", @import);
                code.AppendLine();
            }
            code.AppendLine();
            code.AppendLinf("namespace {0}", this.Namespace);
            code.AppendLine("{");
            foreach (var ctor in this.AttributeCollection)
            {
                code.AppendLine(ctor);
            }
            code.AppendLinf("   public {2} class {0} {3} {1}", this.Name, this.BaseClass, this.IsPartial ? "partial" : "", !string.IsNullOrWhiteSpace(this.BaseClass) ? ":" : "");
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

        public void AddProperty(string format, params object[] args)
        {
            this.PropertyCollection.Add(new Property { Code = string.Format(format, args) });
        }
    }
}