using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.RoslynScriptEngines
{
    public class RoslynScriptEngine2 : IScriptEngine
    {
        private readonly Dictionary<string, dynamic> m_host = new Dictionary<string, dynamic>();
        public T Evaluate<T, T1>(string script, T1 item)
        {
            var key = string.Format("{0}:{1}:{2}", typeof(T1).FullName, typeof(T).Name, script);
            if (m_host.ContainsKey(key))
                return m_host[key].Evaluate(item);

            var references = new List<MetadataReference>
            {
                this.CreateMetadataReference<DateTime>(),
                this.CreateMetadataReference<Entity>()
            };

            try
            {
                references.Add(MetadataReference.CreateFromAssembly(item.GetType().Assembly));
            }
            catch (ArgumentException e)
            {
                if (DebuggerHelper.IsRunningInUnitTest && e.Message == "Empty path name is not legal.")
                {
                    Console.WriteLine("Load from buffer for unit test");
                    references.Add(MetadataReference.CreateFromFile(this.Stream));
                }
                else
                {
                    throw;
                }
            }


            var tree = this.GetSyntaxTree<T, T1>(script, item);
            var compilation = CSharpCompilation.Create("eval")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(references)
                .AddSyntaxTrees(tree);

            var diagnostics = compilation.GetDiagnostics();
            if (diagnostics.Length > 0)
            {
                diagnostics.ToList().ForEach(Console.WriteLine);
                throw new Exception("Compilation error in your script");
            }
            using (var stream = new MemoryStream())
            {
                compilation.Emit(stream);
                var dll = Assembly.Load(stream.GetBuffer());
                var type = dll.GetType("Bespoke.Sph.RoslynScriptEngines.Host");
                dynamic host = Activator.CreateInstance(type);

                m_host.Add(key, host);
                return host.Evaluate(item);
            }
        }

        public string Stream { get; set; }


        public T Evaluate<T, T1, T2>(string script, T1 arg1, T2 arg2)
        {
            throw new NotImplementedException();
        }

        public T Evaluate<T, T1, T2, T3>(string script, T1 arg1, T2 arg2, T3 arg3)
        {
            throw new NotImplementedException();
        }

        private CSharpSyntaxTree GetSyntaxTree<TReturn, TItem>(string script, TItem item)
        {
            var block = script;
            if (!block.EndsWith(";")) block = string.Format("return {0};", script);

            var code = new StringBuilder();
            code.AppendLine("using System;");
            code.AppendLine("namespace Bespoke.Sph.RoslynScriptEngines");
            code.AppendLine("{");
            code.AppendLine("   public class Host");
            code.AppendLine("   {");
            code.AppendLine("       public DateTime @Today { get { return DateTime.Today; } }");
            code.AppendLine("       public DateTime @Now { get { return DateTime.Now; } }");
            code.AppendLinf("       public {0} Item{{ get; set;}}", item.GetType().FullName);
            code.AppendLinf("       public {0} Evaluate({1} item)", typeof(TReturn).FullName, item.GetType().FullName);
            code.AppendLine("       {");
            code.AppendLine("           this.Item = item;");
            code.AppendLine(block);
            code.AppendLine("       }");
            code.AppendLine("   }");
            code.AppendLine("}");
            return (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(code.ToString());
        }
    }
}