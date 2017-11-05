using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Codes
{

    public class AssemblyInfoClass
    {
        private AssemblyInfoClass()
        {

        }
        public List<string> Attributes { get; } = new List<string>();
        public string FileName { get; private set; }

        public static async Task<AssemblyInfoClass> GenerateAssemblyInfoAsync<T>(T asset) where T : Entity
        {
            var cvs = ObjectBuilder.GetObject<ICvsProvider>();
            var source = $@"{ConfigurationManager.SphSourceDirectory}\{typeof(T).Name}\{asset.Id}.json";
            var logs = await cvs.GetCommitLogsAsync(source, 1, 1);

            var fileInfo = new FileInfo(source);
            var y2012 = new DateTime(2012, 1, 1);
            var commit = logs.ItemCollection.FirstOrDefault() ?? new CommitLog
            {
                CommitId = "NA",
                DateTime = fileInfo.LastWriteTime,
                Commiter = "NA",
                Comment = "NA"
            };
            var version = $"{ConfigurationManager.MajorVersion}.{ConfigurationManager.MinorVersion}.{Convert.ToInt32((commit.DateTime - y2012).TotalDays)}.{logs.TotalRows}";

            var @class = new AssemblyInfoClass();
            @class.Attributes.Add($@"[assembly:System.Reflection.AssemblyInformationalVersion(""{version}-{commit.CommitId}"")]");
            @class.Attributes.Add($@"[assembly:System.Reflection.AssemblyFileVersion(""{version}"")]");

            dynamic assetD = asset;
            @class.FileName = $"{ConfigurationManager.GeneratedSourceDirectory}\\{assetD.Name}\\AssemblyInfo.cs";
            return @class;

        }

        public override string ToString()
        {
            return this.Attributes.ToString("\r\n");
        }
    }
    [DebuggerDisplay("{Name}, Properties:{PropertyCollection.Count}, Methods : {MethodCollection.Count}")]
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
            set => m_fileName = value;
        }


        public bool IsPartial { get; set; }

        public void AddNamespaceImport(params Type[] types)
        {
            foreach (var type in types)
            {
                if (!this.ImportCollection.Contains(type.Name))
                    this.ImportCollection.Add(type.Namespace);
            }
        }
        public void AddNamespaceImport<T, T2>()
        {
            var types = new[] { typeof(T), typeof(T2) };
            AddNamespaceImport(types);
        }
        public void AddNamespaceImport<T, T2, T3>()
        {
            var types = new[] { typeof(T), typeof(T2), typeof(T3) };
            AddNamespaceImport(types);
        }
        public void AddNamespaceImport<T, T2, T3, T4>()
        {
            var types = new[] { typeof(T), typeof(T2), typeof(T3), typeof(T4) };
            AddNamespaceImport(types);
        }
        public void AddNamespaceImport<T>()
        {
            var type = typeof(T);
            if (!this.ImportCollection.Contains(type.Name))
                this.ImportCollection.Add(type.Namespace);

        }

        public string GetCode()
        {
            if (!string.IsNullOrWhiteSpace(m_code))
                return m_code;
            var code = new StringBuilder();
            var imported = new List<string>();
            foreach (var import in this.ImportCollection)
            {
                var directive = import.StartsWith("using ") ? $"{import};" : $"using {import};";
                var importUsing = directive.Replace(";;", ";");
                if (imported.Contains(importUsing)) continue;

                code.AppendLine(importUsing);
                imported.Add(importUsing);
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
            var file = $"{dir.Replace(@"\\", @"\")}{FileName}{ (FileName.EndsWith(".cs") ? "" : ".cs")}";
            File.WriteAllText(file, this.GetCode());


            return file;
        }
    }
}