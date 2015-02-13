using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.Domain
{
    public partial class FunctionField : Field
    {
        public override string GetCSharpExpression()
        {
            return string.Format("{0}", this.Script);
        }

        public FunctionField()
        {
            this.WebId = "ff" + Guid.NewGuid().ToString().Replace("-", string.Empty)
                .Substring(0, 8);
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

            using (var stream = new MemoryStream())
            {
                var result = this.Compile(context, stream);
                if (!result.Result)
                    throw new InvalidOperationException("Cannot compile your script");

                var assembly = Assembly.Load(stream.GetBuffer());
                dynamic ff = Activator.CreateInstance(assembly.GetType(this.CodeNamespace + ".FunctionFieldHost"));
                m_ff.Add(this.WebId, ff);

                return ff.Evaluate(context.Item);
            }



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


        private SphCompilerResult Compile(RuleContext context, Stream stream)
        {
            var code = this.GenerateCode(context);
            var trees = new List<SyntaxTree>() { CSharpSyntaxTree.ParseText(code) };
            var references = new List<MetadataReference>(){
                this.CreateMetadataReference<Entity>(),
                this.CreateMetadataReference<EnumerableQuery>(),
                this.CreateMetadataReference<DateTime>(),
                MetadataReference.CreateFromAssembly(context.Item.GetType().Assembly)
                };

            var compilation = CSharpCompilation.Create(string.Format("{0}.{1}", ConfigurationManager.ApplicationName, this.WebId))
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(references)
                .AddSyntaxTrees(trees);

            var errors = compilation.GetDiagnostics()
                .Where(d => d.Id != "CS8019")
                .Select(d => new BuildError(d));

            var result = new SphCompilerResult { Result = true };
            result.Errors.AddRange(errors);
            result.Result = result.Errors.Count == 0;
            if (DebuggerHelper.IsVerbose)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                result.Errors.ForEach(Console.WriteLine);
                Console.ForegroundColor = color;
            }

            if (null == stream)
                throw new ArgumentException("To emit please provide a stream in your options", "stream");

            var emitResult = compilation.Emit(stream);
            result.Result = emitResult.Success;
            var errors2 = emitResult.Diagnostics.Select(v => new BuildError(v));
            result.Errors.AddRange(errors2);

            return result;
        }
    }

}
