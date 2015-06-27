using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Microsoft.CSharp;
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

        public string CodeNamespace
        {
            get
            {
                return "ff" + this.WebId.Replace("-", string.Empty)
                    .Substring(0, 8);
            }

        }

        private static readonly Dictionary<string, dynamic> m_ff = new Dictionary<string, dynamic>();
        public override object GetValue(RuleContext context)
        {
            if (m_ff.ContainsKey(this.WebId))
                return m_ff[this.WebId].Evaluate(context.Item);

            var dll = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, string.Format("{0}.dll", this.CodeNamespace));
            if (!File.Exists(dll)) this.Compile(context);

            var assembly = Assembly.LoadFile(dll);
            dynamic ff = Activator.CreateInstance(assembly.GetType(this.CodeNamespace + ".FunctionFieldHost"));
            m_ff.Add(this.WebId, ff);

            return ff.Evaluate(context.Item);

        }

        private string GenerateCode(RuleContext context)
        {
            var block = this.Script;
            if (!block.EndsWith(";")) block = string.Format("return {0};", this.Script);

            var code = new StringBuilder();
            code.AppendLine("using System;");
            code.AppendLine("using Bespoke.Sph.Domain;");
            code.AppendLine("using System.Linq;");

            code.AppendLine("namespace " + this.CodeNamespace);
            code.AppendLine("{");
            code.AppendLine("   public class FunctionFieldHost");
            code.AppendLine("   {");

            code.AppendLine("       public object Evaluate(Entity entity)");
            code.AppendLine("       {");
            code.AppendLine("           var item = entity as " + context.Item.GetType().FullName + ";");
            code.AppendLine("           " + block);
            code.AppendLine("       }");
            code.AppendLine("   }");

            code.AppendLine("}");

            return code.ToString();
        }


        public WorkflowCompilerResult Compile(RuleContext context)
        {
            var code = this.GenerateCode(context);

            using (var provider = new CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.WorkflowCompilerOutputPath;
                var parameters = new CompilerParameters
                {
                    OutputAssembly = Path.Combine(outputPath, string.Format("{0}.dll", this.CodeNamespace)),
                    GenerateExecutable = false,
                    IncludeDebugInformation = true
                };
                parameters.ReferencedAssemblies.Add(typeof(Entity).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Int32).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Expression<>).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(INotifyPropertyChanged).Assembly.Location);
                parameters.ReferencedAssemblies.Add((context.Item.GetType()).Assembly.Location);

                var result = provider.CompileAssemblyFromSource(parameters, code);
                var cr = new WorkflowCompilerResult
                {
                    Result = true,
                    Output = Path.GetFullPath(parameters.OutputAssembly)
                };
                cr.Result = result.Errors.Count == 0;

                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error);
                    var guid = Guid.NewGuid();
                    var log = Path.Combine(ConfigurationManager.GeneratedSourceDirectory, @"\logs");
                    if (!Directory.Exists(log)) Directory.CreateDirectory(log);

                    var logFile = $"{log}\\{guid}.log";
                    var cs = $"{log}\\{guid}.cs";

                    File.WriteAllText(logFile, error.ToString());
                    File.WriteAllText(cs, code);
                }

                return cr;
            }
        }

    }
}