using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition
    {
        public async Task<ImmutableArray<Diagnostic>> GetDiagnosticsAsync(CompilerOptions cops)
        {
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithOptimizationLevel(OptimizationLevel.Debug)
            .WithUsings();
            var codes = this.GenerateCode();

            var ws = new AdhocWorkspace();
            var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(this.Id),
                VersionStamp.Create(),
                this.Id, this.Id,
                LanguageNames.CSharp)
                .AddMetadataReference<object>()
                .AddMetadataReference<EnumerableQuery>()
                .AddMetadataReference<Task>()
                .AddMetadataReference<DateTime>()
                .AddMetadataReference<Entity>()
                .AddMetadataReference<Expression>()
                .AddMetadataReference<HttpRequestBase>()
                .AddMetadataReference<RoutePrefixAttribute>()
                .AddMetadataReference<ApiController>()
                .AddMetadataReference(typeof(Binder))
                .AddMetadataReference(typeof(JsonConvert))
                .AddMetadataReference(cops.ReferencedAssembliesLocation.ToArray())
                .WithCompilationOptions(options);
            var project = ws.AddProject(projectInfo);


            foreach (var @class in codes)
            {
                ws.AddDocument(project.Id, @class.FileName, SourceText.From(@class.GetCode()));
            }

            project = ws.CurrentSolution.Projects.First();

            var compiler = await project.GetCompilationAsync().ConfigureAwait(false);
            var diagnostics = compiler.GetDiagnostics();

            return diagnostics;

        }


        public string SanitizeMethodBody(Activity activity)
        {
            var method = new StringBuilder();
            var body = activity.GenerateExecMethodBody(this);
            if (body.Contains("await "))
                method.AppendLine("public async Task<ActivityExecutionResult> " + activity.MethodName + "()");
            else
                method.AppendLine("public Task<ActivityExecutionResult> " + activity.MethodName + "()");
            method.AppendLine("{");
            method.AppendLine(body);


            var file = new StringBuilder();
            file.AppendLine(method.ToString());
            file.AppendLine("}");

            var tree = CSharpSyntaxTree.ParseText(file.ToString());
            var root = (CompilationUnitSyntax)tree.GetRoot();

            var walker = new ActivityThrowSyntaxWalker(activity);
            walker.Visit(root);

            if (walker.HasThrow)
            {
                var resultRemover = new LocalResultDeclarationRemoval();
                var clean = resultRemover.Visit(root);
                return clean.ToFullString();
            }

            method.AppendLine(body.Contains("await ") ? "return result;" : "return Task.FromResult(result);");
            method.AppendLine("}");
            return method.ToString();
        }

        class LocalResultDeclarationRemoval : CSharpSyntaxRewriter
        {
            public override SyntaxNode VisitEmptyStatement(EmptyStatementSyntax node)
            {
                return node.WithSemicolonToken(
                    SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken)
                        .WithLeadingTrivia(node.SemicolonToken.LeadingTrivia)
                        .WithTrailingTrivia(node.SemicolonToken.TrailingTrivia));
            }

            public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
            {
                var resultNode = node.Declaration.Variables.SingleOrDefault(r => r.Identifier.Text == "result");
                if (null != resultNode)
                    return null;
                resultNode = node.Declaration.Variables.SingleOrDefault(r => r.Identifier.Text == "item");
                if (null != resultNode)
                    return null;

                return base.VisitLocalDeclarationStatement(node);
            }

            public override SyntaxNode VisitExpressionStatement(ExpressionStatementSyntax node)
            {
                if (node.ToString().Contains("result")) return null;
                return base.VisitExpressionStatement(node);
            }
        }
        class ActivityThrowSyntaxWalker : CSharpSyntaxWalker
        {
            private readonly Activity m_activity;
            public bool HasThrow { get; private set; }

            public ActivityThrowSyntaxWalker(Activity activity)
            {
                m_activity = activity;
            }

            public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                if (node.Identifier.Text == m_activity.MethodName)
                {
                    this.HasThrow = node.Body.Statements.OfType<ThrowStatementSyntax>().Any();
                    var resultDeclarator = node.Body.Statements.OfType<LocalDeclarationStatementSyntax>().
                        SelectMany(v => v.Declaration.Variables)
                        .SingleOrDefault(r => r.Identifier.Text == "result");

                    //&& var creation = ((ObjectCreationExpressionSyntax)resultDeclarator.Identifier.Value).Type.I)
                    if (null != resultDeclarator)
                        Debug.WriteLine(resultDeclarator.Initializer);
                }
                base.VisitMethodDeclaration(node);
            }
        }

    }

}
