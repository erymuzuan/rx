using System;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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

            var ws = new CustomWorkspace();
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
    }
}
