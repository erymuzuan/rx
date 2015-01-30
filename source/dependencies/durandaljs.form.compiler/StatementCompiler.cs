using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
    public class StatementCompiler
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
            {
                expression = GetDefaultExpression<T>();
            }

            var walkersObjectModels = this.Walkers
                .Select(x => x.GetObjectModel(project))
                .Where(x => null != x)
                .ToList();

            var parameters = string.Join(", ", walkersObjectModels
                .Where(x => x.IncludeAsParameter)
                .Select(x => x.ClassName + " " + x.IdentifierText));

            var snippet = BuilExpressionClass<T>(expression, project, parameters);
            var projectDocuments = project.GenerateCode().ToList();
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
                .AddReference<Task>()
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
                return Task.FromResult(result);


            var statements = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Single(x => x.Identifier.Text == "Evaluate")
                .Body
                .Statements;

            result.Code = CompileStatements(statements, model);
            return Task.FromResult(result);
        }

        private string CompileStatements(SyntaxList<SyntaxNode> statements, SemanticModel model)
        {
            var code = new StringBuilder();
            if (statements.Any(x => x.DescendantNodes().OfType<AwaitExpressionSyntax>().Any()))
                return this.BuildAwaitStatementTree(statements, model, true);


            foreach (var statement in statements)
            {
                var st1 = statement;
                var walkers = this.Walkers.Where(x => x.Filter(st1)).ToList();
                foreach (var w in walkers)
                {
                    var f = w.Walk(st1, model);
                    if (string.IsNullOrWhiteSpace(f)) continue;
                    code.AppendLine(f.TrimEnd());
                }
                if (!walkers.Any())
                {
                    Console.WriteLine("!!!!!!");
                    Console.WriteLine("Cannot find statement walker for " + st1.CSharpKind());
                }
            }

            return code.ToString();
        }

        private string BuildAwaitStatementTree(SyntaxList<StatementSyntax> statements, SemanticModel model, bool initialize = false)
        {
            var code = new StringBuilder();

            var awaitResults = statements
                .OfType<LocalDeclarationStatementSyntax>()
                .Where(x => x.DescendantNodes().OfType<AwaitExpressionSyntax>().Any())
                .Select(x => x.Declaration.Variables[0].Identifier.Text)
                .ToList();
            if (initialize && awaitResults.Count > 0)
                code.AppendLine("var " + string.Join(", ", awaitResults) + ";");

            // loop all the statements before the await, once hitting an await then break to do the promise.then.then
            foreach (var statement in statements)
            {
                var st1 = statement;
                var hasAwait = st1.DescendantNodes().OfType<AwaitExpressionSyntax>().Any();
                if (hasAwait && initialize) break;

                var walkers = this.Walkers.Where(x => x.Filter(st1)).ToList();
                foreach (var w in walkers)
                {
                    var f = w.Walk(st1, model);
                    if (string.IsNullOrWhiteSpace(f)) continue;
                    var temp = f.TrimEnd();
                    if (temp.EndsWith("\r\n;"))
                        code.AppendLine(temp.Replace("\r\n;", ";"));
                    else
                        code.AppendLine(temp + ";");
                }
                if (!walkers.Any())
                {
                    Console.WriteLine("!!!!!!");
                    Console.WriteLine("Cannot find statement walker for " + st1.CSharpKind());
                }
                if (hasAwait) break;

            }

            var awaitStatements = statements
                .Where(x => x.DescendantNodes().OfType<AwaitExpressionSyntax>().Any())
                .ToList();

            // now build the promise.then.then.then....
            foreach (var statement in awaitStatements)
            {
                var awaitIndex = awaitStatements.IndexOf(statement);
                var lineNumber = statements.IndexOf(statement);
                var st1 = statement;
                var hasAwait = st1.DescendantNodes().OfType<AwaitExpressionSyntax>().Any();

                var walkers = this.Walkers.Where(x => x.Filter(st1)).ToList();
                var statementCode = "";
                foreach (var w in walkers)
                {
                    var f = w.Walk(st1, model);
                    if (string.IsNullOrWhiteSpace(f)) continue;
                    statementCode = f.TrimEnd();
                    if (!hasAwait)
                        code.AppendLine(statementCode);
                }
                if (!walkers.Any())
                {
                    Console.WriteLine("!!!!!!");
                    Console.WriteLine("Cannot find statement walker for " + st1.CSharpKind());
                }


                var local = statement as LocalDeclarationStatementSyntax;
                // build the first promise for an await with result
                if (null != local && initialize && awaitIndex == 0)
                {

                    if (statementCode.EndsWith(";"))
                        statementCode = statementCode.Substring(0, statementCode.Length - 1);
                    code.AppendLine(statementCode);
                }
                if (null != local)
                {
                    if (initialize)
                    {
                        code.AppendLinf(".then(function(__temp{0}) {{", awaitIndex);
                        code.AppendLinf("       {0} = __temp{1};", local.Declaration.Variables[0].Identifier.Text, awaitIndex);

                        var subsequestStatements = statements.SkipWhile((s, i) => i <= lineNumber).ToList();
                        var subsequentCode = this.BuildAwaitStatementTree(SyntaxFactory.List(subsequestStatements), model);

                        code.AppendLine(subsequentCode);

                    }
                    else
                    {
                        return code.ToString();
                    }
                    code.Append("   })");
                }

                var expression = statement as ExpressionStatementSyntax;
                if (null != expression && initialize && awaitIndex == 0)
                {
                    if (statementCode.EndsWith(";"))
                        statementCode = statementCode.Substring(0, statementCode.Length - 1);
                    code.AppendLine(statementCode);
                }
                if (null != expression)
                {
                    if (initialize)
                    {
                        code.AppendLine(".then(function() {");

                        var subsequestStatements = statements.SkipWhile((s, i) => i <= lineNumber).ToList();
                        var subsequentCode = this.BuildAwaitStatementTree(SyntaxFactory.List(subsequestStatements), model);

                        code.AppendLine(subsequentCode);
                    }
                    else
                    {
                        return code.ToString();
                    }
                    code.Append("   })");
                }

            }
            code.AppendLine(";");



            return code.ToString();
        }

        private static string GetDefaultExpression<T>()
        {
            var type = typeof(T);
            if (type == typeof(bool)) return "false";
            if (type == typeof(string)) return "null";
            if (type == typeof(DateTime)) return "DateTime.MinValue";

            return string.Format("{0}", default(T));
        }

        private static CSharpSyntaxTree BuilExpressionClass<T>(string code, IProjectProvider project, string parameters)
        {
            var file = new StringBuilder();
            var async = code.Contains("await ") || code.Contains("Task");

            file.AppendLine("using System;");
            if (async)
                file.AppendLine("using System.Threading.Tasks;");
            file.AppendLine("using System.Linq;");
            file.AppendLine();
            file.AppendLine("namespace " + project.DefaultNamespace);
            file.AppendLine("{");
            file.AppendLine("   public class ExpressionCompilerSnippet");
            file.AppendLine("   {");
            file.AppendLinf("       public {3}{2} Evaluate({0} item, {1})  ", project.Name, parameters, typeof(T).ToCSharp(), async ? "async " : "");
            file.AppendLine("       {");
            file.AppendLinf("           {0}", code);
            file.AppendLine("       }");
            file.AppendLine("   }");
            file.AppendLine("}");

            return (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(file.ToString());
        }



    }
}