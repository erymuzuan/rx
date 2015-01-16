using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Bespoke.Sph.Domain.Codes
{
    [DebuggerDisplay("{Name}:{BaseClass} {FileName}")]
    public class Class
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string BaseClass { get; set; }

        private readonly ObjectCollection<Property> m_propertyCollection = new ObjectCollection<Property>();
        private readonly ObjectCollection<Method> m_methodCollection = new ObjectCollection<Method>();
        private readonly ObjectCollection<string> m_attributeCollection = new ObjectCollection<string>();
        private readonly ObjectCollection<string> m_importCollection = new ObjectCollection<string>();
        private readonly ObjectCollection<string> m_ctorCollection = new ObjectCollection<string>();
        private string m_fileName;

        public ObjectCollection<string> CtorCollection
        {
            get { return m_ctorCollection; }
        }

        public ObjectCollection<string> ImportCollection
        {
            get { return m_importCollection; }
        }

        public ObjectCollection<string> AttributeCollection
        {
            get { return m_attributeCollection; }
        }

        public ObjectCollection<Method> MethodCollection
        {
            get { return m_methodCollection; }
        }
        public ObjectCollection<Property> PropertyCollection
        {
            get { return m_propertyCollection; }
        }

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
        public void AddNamespaceImport<T>()
        {
            this.ImportCollection.Add(typeof(T).Namespace);
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

        public string Save(string folder)
        {
            var cs = Path.Combine(folder, this.FileName);
            File.WriteAllText(cs, this.GetCode());
            return cs;

        }
    }
}