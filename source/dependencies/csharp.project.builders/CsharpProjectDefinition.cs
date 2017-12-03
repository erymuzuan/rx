using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Csharp.CompilersServices.Extensions;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;

namespace Bespoke.Sph.Csharp.CompilersServices
{

    public static class ProjectDefinitionExtension
    {

        public static MetadataReference[] GetMetadataReferences(this IProjectDefinition project)
        {
            var cvs = ObjectBuilder.GetObject<ISourceRepository>();
            var references = new List<MetadataReference>
                {
                    project.CreateMetadataReference<object>(),
                    project.CreateMetadataReference<WorkflowDefinition>(),
                    project.CreateMetadataReference<Task>(),
                    project.CreateMetadataReference<Newtonsoft.Json.Linq.JObject>(),
                    project.CreateMetadataReference<EnumerableQuery>()
                };
            switch (project)
            {
                case QueryEndpoint qe:
                    var ed2 = cvs.LoadOneAsync<EntityDefinition>(x => x.Name == qe.Entity).Result;
                    references.Add(MetadataReference.CreateFromFile($"{ConfigurationManager.WebPath}\\bin\\webapi.common.dll"));
                    references.Add(MetadataReference.CreateFromFile($"{ConfigurationManager.WebPath}\\bin\\System.Web.Http.dll"));
                    references.Add(MetadataReference.CreateFromFile($"{ConfigurationManager.CompilerOutputPath}\\{ed2.AssemblyName}.dll"));
                    references.AddMetadataReference(typeof(HttpStatusCode));
                    break;
                case OperationEndpoint oe:
                    //TODO : get the parents from IProjectDefinition interface
                    var ed1 = cvs.LoadOneAsync<EntityDefinition>(x => x.Name == oe.Entity).Result;
                    references.Add(MetadataReference.CreateFromFile($"{ConfigurationManager.WebPath}\\bin\\webapi.common.dll"));
                    references.Add(MetadataReference.CreateFromFile($"{ConfigurationManager.WebPath}\\bin\\System.Web.Http.dll"));
                    references.Add(MetadataReference.CreateFromFile($"{ConfigurationManager.CompilerOutputPath}\\{ed1.AssemblyName}.dll"));
                    references.AddMetadataReference(typeof(HttpStatusCode));
                    break;
            }

            return references.ToArray();

        }

        private static async Task<IEnumerable<Class>> GenerateProjectCodesAsync(this IProjectDefinition project)
        {
            var sourceRepository = ObjectBuilder.GetObject<ISourceRepository>();
            switch (project)
            {
                case EntityDefinition ed:
                    return await ed.GenerateCodeAsync();
                case OperationEndpoint oe:
                    var ed1 = await sourceRepository.LoadOneAsync<EntityDefinition>(x => x.Name == oe.Entity);
                    return await oe.GenerateSourceAsync(ed1);
                case QueryEndpoint qe:
                    var sources = new List<Class>();
                    if (!(project is QueryEndpoint endpoint)) return sources;
                    var ed2 = await sourceRepository.LoadOneAsync<EntityDefinition>(x => x.Name == endpoint.Entity);

                    var wrapper = new QueryEndpointCsharp(qe, ed2);

                    var controller = wrapper.GenerateCode();
                    var info = await wrapper.GetAssemblyInfoCodeAsync();

                    sources.AddRange(controller, info);

                    return sources;
                    //case Trigger tg:
                    //    return tg.GenerateCodeAsync();
                    //case WorkflowDefinition wd:
                    //    return wd.GenerateCodeAsync();
                    //case ReceivePort rp:
                    //    return rp.GenerateCodeAsync();
                    //case ReceiveLocation rl:
                    //    return rl.GenerateCodeAsync();
                    //case TransformDefinition td:
                    //    return td.GenerateCodeAsync();
                    //case Adapter ad:
                    //    return ad.GenerateCodeAsync();
            }
            throw new Exception();
        }

        public static async Task<RxCompilerResult> CompileAsyncWithRoslynAsync(this IProjectDefinition project, CompilerOptions2 options)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            var parseOptions = new CSharpParseOptions(LanguageVersion.CSharp7_1);
            var classes = (await project.GenerateProjectCodesAsync()).ToList();

            List<SyntaxTree> trees;

            if (null != options.PdbStream || null != options.PdbPath)
            {
                // just to get the PDB
                trees = (from c in classes
                         let file = c.WriteSource(project)
                         let root = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(c.GetCode(), parseOptions, file, Encoding.UTF8)
                         select CSharpSyntaxTree.Create(root.GetRoot(), path: file, encoding: Encoding.UTF8)).ToList();
            }
            else
            {
                trees = (from c in classes
                         let x = c.GetCode()
                         let root = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(x, parseOptions)
                         select CSharpSyntaxTree.Create(root.GetRoot(), path: c.FileName)).ToList();
            }



            var co = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(project.AssemblyName)
                .WithAssemblyName(project.AssemblyName)
                .WithOptions(co)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(project.GetMetadataReferences())
                .AddSyntaxTrees(trees);

            var errors = compilation.GetDiagnostics()
                .Where(d => d.Id != "CS8019")
                .Select(d => d.ToBuildError());

            var result = new RxCompilerResult { Result = true };
            result.Errors.AddRange(errors);
            result.Result = result.Errors.All(x => x.IsWarning);

            result.Errors.ForEach(x => logger.WriteError(x.ToString()));

            if (!result.Result || !options.Emit)
                return result;

            if (null == options.PeStream && null == options.PePath)
                throw new ArgumentException(@"To emit please provide a PeStream or PePath in your options", nameof(options));

            //TODO : wrong Encoding , no pdb,
            // if pdb is presents then we have to save the source files
            EmitResult emitResult = null;
            if (null != options.PeStream)
            {
                emitResult = null == options.PdbStream ?
                  compilation.Emit(options.PeStream) :
                  compilation.Emit(options.PeStream, options.PdbStream);
            }

            if (null != options.PePath)
            {
                emitResult = null == options.PdbPath ?
                  compilation.Emit(options.PePath) :
                  compilation.Emit(options.PePath, options.PdbPath);
            }


            //var emitResult = compilation.Emit(options.PeStream);

            result.Result = emitResult.Success;
            var errors2 = emitResult.Diagnostics.Select(v => v.ToBuildError());
            result.Errors.AddRange(errors2);

            return result;
        }
    }
}

