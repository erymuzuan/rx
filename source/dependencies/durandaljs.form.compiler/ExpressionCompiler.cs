﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export]
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

        public async Task<SnippetCompilerResult> CompileAsync<T>(string expression, IProjectProvider project)
        {

            if (string.IsNullOrWhiteSpace(expression))
            {
                expression = GetDefaultExpression<T>();
            }

            var walkersObjectModels = this.Walkers
                .Select(x => x.GetObjectModel(project))
                .Where(x => null != x)
                .ToList();

            var parameters = string.Join(", ", walkersObjectModels
                .Where(x => x.IncludeAsParameter)
                .Select(x => x.InterfaceName + " " + x.IdentifierText));

            var snippet = BuilExpressionClass<T>(expression, project, parameters);
            var projectDocuments = (await project.GenerateCodeAsync()).ToList();
            var codes = (from c in projectDocuments
                         where !c.FileName.Contains("Controller")
                         let x = c.GetCode().Replace("using Bespoke.Sph.Web.Helpers;", string.Empty)
                         .Replace("using System.Web.Mvc;", string.Empty)
                         select (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(x)).ToArray();



            var trees = new List<SyntaxTree> { snippet };
            var root = (CompilationUnitSyntax)snippet.GetRoot();

            trees.AddRange(codes);
            trees.AddRange(walkersObjectModels.Select(x => x.SyntaxTree));

            var compilation = CSharpCompilation.Create("eval")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(project.References)
                .AddSyntaxTrees(trees.ToArray());

            var model = compilation.GetSemanticModel(snippet);


            var diagnostics = compilation.GetDiagnostics();

            var result = new SnippetCompilerResult { Success = true };
            result.DiagnosticCollection.AddRange(diagnostics.Where(x => x.Id != "CS8019"));
            result.Success = result.DiagnosticCollection.Count == 0;
            if (DebuggerHelper.IsVerbose)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                result.DiagnosticCollection.ForEach(Console.WriteLine);
                Console.ForegroundColor = color;
            }
            if (!result.Success)
                return (result);


            var statement = root.DescendantNodes().OfType<ReturnStatementSyntax>()
                .Single()
                .Expression;

            result.Code = CompileExpression(statement, model);
            dynamic tag = new ExpandoObject();
            tag.Statement = statement;
            tag.SemanticModel = model;
            result.Tag = tag;

            return (result);
        }

        private static string GetDefaultExpression<T>()
        {
            var type = typeof(T);
            if (type == typeof(bool)) return "false";
            if (type == typeof(string)) return "null";
            if (type == typeof(DateTime)) return "DateTime.MinValue";

            return string.Format("{0}", default(T));
        }

        private static CSharpSyntaxTree BuilExpressionClass<T>(string expression, IProjectProvider project, string parameters)
        {
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

            return (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(file.ToString());
        }


        private string CompileExpression(SyntaxNode statement, SemanticModel model)
        {
            this.Walkers.ToList().ForEach(x => x.SemanticModel = model);
            var walkers = this.Walkers
                .Where(x => x.Filter(statement))
                .ToList();

            if (walkers.Count > 1)
            {
                Console.WriteLine("!!! " + statement.CSharpKind());
                foreach (var w in walkers)
                {
                    Console.WriteLine(statement + " -> " + w.GetType().Name);
                }
            }


            return walkers
                .Select(x => x.Walk(statement, model))
                .LastOrDefault(x => !string.IsNullOrWhiteSpace(x));
        }
    }
}
