using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bespoke.Sph.Domain
{

    public abstract class Project : Entity, IProjectProvider
    {
        public abstract string DefaultNamespace { get; }

        public abstract string Name { get; }

        public virtual MetadataReference[] References
        {
            get { throw new NotImplementedException(); }
        }

        public abstract Task<IEnumerable<Class>> GenerateCodeAsync();

        public virtual Task<IProjectModel> GetModelAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<SphCompilerResult> CompileAsync(CompilerOptions options)
        {
            var project = (IProjectProvider)this;
            var projectDocuments = (await project.GenerateCodeAsync()).ToList();
            var trees = (from c in projectDocuments
                         let x = c.GetCode()
                         let root = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(x)
                         select CSharpSyntaxTree.Create(root.GetRoot(), path: c.FileName)).ToList();

            var compilation = CSharpCompilation.Create(string.Format("{0}.{1}", ConfigurationManager.ApplicationName, this.Id))
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(project.References)
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
            if (!result.Result || !options.Emit)
                return result;

            if (null == options.Stream)
                throw new ArgumentException("To emit please provide a stream in your options", "options");

            var emitResult = compilation.Emit(options.Stream);
            result.Result = emitResult.Success;
            var errors2 = emitResult.Diagnostics.Select(v => new BuildError(v));
            result.Errors.AddRange(errors2);

            return result;
        }
    }
}