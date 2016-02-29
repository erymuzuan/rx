using System;
using System.IO;
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
        public bool IsStatic { get; set; }

        private string m_fileName;
        private readonly string m_code;


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
                code.AppendLine(directive.Replace(";;", ";"));
            }
            code.AppendLine();
            code.AppendLine($"namespace {Namespace}");
            code.AppendLine("{");
            foreach (var ctor in this.AttributeCollection)
            {
                code.AppendLine(ctor);
            }
            var partial = this.IsPartial ? "partial " : "";
            var @static = this.IsStatic ? "static " : "";
            var baseClass = !string.IsNullOrWhiteSpace(this.BaseClass) ? $": {BaseClass}" : "";
            code.AppendLine($"   public {@static}{partial}class {Name} {baseClass}");
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
                if (!string.IsNullOrWhiteSpace(mtd.Comment))
                    code.AppendLine(mtd.Comment);
                code.AppendLine(mtd.GenerateCode());
            }

            code.AppendLine("   }");
            code.AppendLine("}");

            return code.FormatCode();

        }

        public void AddMethod(string format, params object[] args)
        {
            var code = format;
            if (args.Length > 0)
                code = string.Format(format, args);
            this.MethodCollection.Add(new Method { Code = code });
        }
        public void AddMethod(Method method)
        {
            this.MethodCollection.Add(method);
        }
        public void AddProperty(string format, params object[] args)
        {
            var code = format;
            if (args.Length > 0)
                code = string.Format(format, args);
            this.PropertyCollection.Add(new Property { Code = code });
        }
        public void AddProperty(string code)
        {
            this.PropertyCollection.Add(new Property { Code = code });
        }
        public void AddProperty(string name, Type type)
        {
            this.PropertyCollection.Add(new Property { Name = name, Type = type });
        }

        public string Save(string folder)
        {
            var dir = $"{ConfigurationManager.GeneratedSourceDirectory}\\{folder}\\";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var file = $"{dir}{FileName}.cs";
            File.WriteAllText(file, this.GetCode());

            return file;
        }
    }
}