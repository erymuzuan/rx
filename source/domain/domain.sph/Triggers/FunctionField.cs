using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class FunctionField : Field
    {
        public FunctionField()
        {
            this.WebId = "ff" + Guid.NewGuid().ToString().Replace("-", string.Empty)
                .Substring(0, 8);
        }

        [XmlIgnore]
        [JsonIgnore]
        private IScriptEngine m_scriptEngine;

        [XmlIgnore]
        [JsonIgnore]
        public IScriptEngine ScriptEngine
        {
            get { return m_scriptEngine ?? (m_scriptEngine = ObjectBuilder.GetObject<IScriptEngine>()); }
            set { m_scriptEngine = value; }
        }

        public string CodeNamespace => "ff" + this.WebId.Replace("-", string.Empty)
            .Substring(0, 8);

        private static readonly ConcurrentDictionary<string, dynamic> m_ff = new ConcurrentDictionary<string, dynamic>();
        public override object GetValue(RuleContext context)
        {
            dynamic obj;
            if (m_ff.TryGetValue(this.WebId, out obj))
                return obj.Evaluate(context.Item);

            var dll = Path.Combine(ConfigurationManager.CompilerOutputPath, $"{this.CodeNamespace}.dll");
            if (File.Exists(dll))
            {
                // by this time it may already there
                if (m_ff.TryGetValue(this.WebId, out obj))
                    return obj.Evaluate(context.Item);
            }
            else
            {
                this.Compile(context);
            }

            var assembly = Assembly.LoadFile(dll);
            dynamic ff = Activator.CreateInstance(assembly.GetType(this.CodeNamespace + ".FunctionFieldHost"));
            m_ff.TryAdd(this.WebId, ff);

            return ff.Evaluate(context.Item);

        }

        public override string GenerateCode()
        {
            return this.Script;
        }

        private string GenerateCode(RuleContext context)
        {
            var block = this.Script;
            if (!block.EndsWith(";")) block = $"return {this.Script};";

            var code = new StringBuilder();
            code.AppendLine("using System;");
            code.AppendLine("using Bespoke.Sph.Domain;");
            code.AppendLine("using System.Linq;");

            code.AppendLine("namespace " + this.CodeNamespace);
            code.AppendLine("{");
            code.AppendLine("   public class FunctionFieldHost");
            code.AppendLine("   {");

            if (null != context.Item)
            {
                code.AppendLine("       public object Evaluate(Entity entity)");
                code.AppendLine("       {");
                code.AppendLine("           var item = entity as " + context.Item.GetType().FullName + ";");
                code.AppendLine("           " + block);
                code.AppendLine("       }");
            }
            if (null != context.Object)
            {
                code.AppendLine("       public object Evaluate(DomainObject domainObject)");
                code.AppendLine("       {");
                code.AppendLine("           var @object = domainObject as " + context.Object.GetType().FullName + ";");
                code.AppendLine("           " + block);
                code.AppendLine("       }");
            }
            code.AppendLine("   }");

            code.AppendLine("}");

            return code.ToString();
        }


        public RxCompilerResult Compile(RuleContext context)
        {
            var code = this.GenerateCode(context);

            using (var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.CompilerOutputPath;
                var parameters = new CompilerParameters
                {
                    OutputAssembly = Path.Combine(outputPath, $"{this.CodeNamespace}.dll"),
                    GenerateExecutable = false,
                    IncludeDebugInformation = true
                };
                parameters.ReferencedAssemblies.Add(typeof(Entity).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Int32).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Expression<>).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(INotifyPropertyChanged).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Xml.XmlDocument).Assembly.Location);
                if (null != context.Item)
                    parameters.ReferencedAssemblies.Add((context.Item.GetType()).Assembly.Location);
                if (null != context.Object)
                    parameters.ReferencedAssemblies.Add((context.Object.GetType()).Assembly.Location);

                var result = provider.CompileAssemblyFromSource(parameters, code);
                var cr = new RxCompilerResult
                {
                    Result = true,
                    Output = Path.GetFullPath(parameters.OutputAssembly)
                };
                cr.Result = result.Errors.Count == 0;

                foreach (CompilerError error in result.Errors)
                {
                    Console.WriteLine(error);
                    var guid = $"{DateTime.Now:yyyyMMdd-HHmm-ss}" + Guid.NewGuid();
                    var log = Path.Combine(ConfigurationManager.GeneratedSourceDirectory, @"function.logs");
                    if (!Directory.Exists(log)) Directory.CreateDirectory(log);

                    var logFile = $"{log}\\{guid}.log";
                    var cs = $"{log}\\{guid}.cs";

                    File.WriteAllText(logFile, error.ToString());
                    File.WriteAllText(cs, code);

                    var entry = new LogEntry
                    {
                        Severity = Severity.Critical,
                        Details = $"{code}\r\n{error}",
                        Message = $"Error to compile your {Name} FunctionField",
                        CallerFilePath = cs,
                        CallerLineNumber =  error.Line,
                        CallerMemberName = Name,
                        Keywords = new[] {WebId, Name},
                        Time = DateTime.Now,
                        Source = "Application"
                    };
                    entry.OtherInfo.Add("code", code);
                    entry.OtherInfo.Add("error", error.ToString());
                    ObjectBuilder.GetObject<ILogger>()
                        .Log(entry);
                }

                return cr;
            }
        }

    }
}