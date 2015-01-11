﻿using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public class ExpressionCompiler
    {

        [ImportMany(typeof(CustomObjectSyntaxWalker), RequiredCreationPolicy = CreationPolicy.Shared, AllowRecomposition = true)]
        public CustomObjectSyntaxWalker[] MefWalkers { get; set; }



        protected CustomObjectSyntaxWalker[] Walkers
        {
            get
            {
                if (null == this.MefWalkers)
                    ObjectBuilder.ComposeMefCatalog(this);
                if (null == this.MefWalkers)
                    throw new InvalidOperationException("Cannot import MEF");
                return this.MefWalkers
                    .Distinct(new CustomObjectSyntaxWalker.Comparer())
                    .ToArray();

            }
        }
        public Task<SnippetCompilerResult> CompileAsync<T>(string expression, IProjectProvider project)
        {
         
            if (string.IsNullOrWhiteSpace(expression))
                return Task.FromResult(new SnippetCompilerResult { Code = "true" });

            var walkersObjectModels = this.Walkers
                .Select(x => x.GetObjectModel(project))
                .Where(x => null != x)
                .ToList();

            var parameters = string.Join(", ", walkersObjectModels
                .Where(x => x.IncludeAsParameter)
                .Select(x => x.ClassName + " " + x.IdentifierText));

            var file = new StringBuilder();
            file.AppendLine("using System;");
            file.AppendLine("using System.Linq;");
            file.AppendLine();
            file.AppendLine("namespace " + project.DefaultNamespace);
            file.AppendLine("{");
            file.AppendLine("   public class ExpressionCompilerSnippet");
            file.AppendLine("   {");
            file.AppendLinf("       public {2} Evaluate({0} item, {1})  ", project.Name, parameters, typeof(T).ToCSharp());
            file.AppendLine("       {");
            file.AppendLinf("           return {0};", expression);
            file.AppendLine("       }");
            file.AppendLine("   }");
            file.AppendLine("}");

            var trees = new ObjectCollection<SyntaxTree>();

            var tree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(file.ToString());
            var root = (CompilationUnitSyntax)tree.GetRoot();
            trees.Add(tree);


            var codes = (from c in project.GenerateCode()
                         where !c.FileName.EndsWith("Controller")
                         where !c.FileName.EndsWith("Controller.cs")
                         let x = c.GetCode().Replace("using Bespoke.Sph.Web.Helpers;", string.Empty)
                         .Replace("using System.Web.Mvc;", string.Empty)
                         select (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(x)).ToList();
            trees.AddRange(codes);
            trees.AddRange(walkersObjectModels.Select(x => x.SyntaxTree));

            var compilation = CSharpCompilation.Create("eval")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReference<object>()
                .AddReference<XmlAttributeAttribute>()
                .AddReference<EntityDefinition>()
                .AddReference<EnumerableQuery>()
                .AddSyntaxTrees(trees.ToArray());

            var model = compilation.GetSemanticModel(tree);


            var diagnostics = compilation.GetDiagnostics();

            var result = new SnippetCompilerResult { Success = true };
            result.DiagnosticCollection.AddRange(diagnostics.Where(x => x.Id != "CS8019"));
            result.Success = result.DiagnosticCollection.Count == 0;
            result.DiagnosticCollection.ForEach(Console.WriteLine);
            if (!result.Success)
                return Task.FromResult(result);


            var statement = root.DescendantNodes().OfType<ReturnStatementSyntax>()
                .Single()
                .Expression;

            result.Code = CompileExpression(statement, model);
            return Task.FromResult(result);
        }


        private string CompileExpression(SyntaxNode statement, SemanticModel model)
        {
            var code = this.Walkers
                .Where(x => x.Filter(statement, model))
                .Select(x => x.Walk(statement, model))
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

            return code;
        }
    }
}
