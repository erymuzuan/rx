using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Extensions;

namespace Bespoke.Sph.Domain.Extensions
{
    public static class ProjectDefinitionExtension
    {
        public static string WriteSource(this Class @class, IProjectDefinition project)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            var file = Path.GetFileNameWithoutExtension(@class.FileName);
            if (@class.TrackedInSourceControl)
            {
                var sourceFolder = $@"{ConfigurationManager.SphSourceDirectory}\{project.GetType().Name}";
                if (!Directory.Exists(sourceFolder))
                    Directory.CreateDirectory(sourceFolder);

                var src = $@"{sourceFolder}\{@class.FileName}.";
                File.WriteAllText(src, @class.GetCode());
                logger.WriteInfo("Source file written to " + src);
                return src;
            }

            var tempFolder = $@"{ConfigurationManager.GeneratedSourceDirectory}\{project.GetType().Name}.{project.Name}";
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            var generated = $@"{ConfigurationManager.GeneratedSourceDirectory}\{project.GetType().Name}.{project.Name}\{file}.cs";
            File.WriteAllText(generated, @class.GetCode());
            return generated;
        }


        public static async Task<RxCompilerResult> CompileDesignModeAsync(this IProjectDefinition project)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            var results = new List<RxCompilerResult>();
            var allCompilers = ObjectBuilder.GetObject<IDeveloperService>().ProjectBuilders;
            logger.WriteDebug($"Found {allCompilers.Length} IProjectBuilders");
            var compilers = allCompilers
                .Where(x => x.IsAvailableInDesignMode)
                .ToArray();// TODO: .AsQueryable().WhereIf(x => this.Compilers.Contains(x.Name), Compilers.Length > 0).ToArray();
            logger.WriteDebug($"Found {compilers.Length} compilers matching the name");
            foreach (var compiler in compilers)
            {
                logger.WriteInfo($"Building with {compiler.Name}...");


                logger.WriteInfo($"Writing source code with {compiler.Name}...");
                var sources = await compiler.GenerateCodeAsync(project);
                sources.ToList().ForEach(x => x.WriteSource(project));

                var options2 = new CompilerOptions2 { IsDebug = true, IsVerbose = true };
                var cr = await compiler.BuildAsync(project, x => options2);
                results.Add(cr);

                logger.WriteInfo($"{compiler.GetType().Name} has {(cr.Result ? "successfully building" : "failed to build")} {project.Name}");
                logger.WriteInfo(cr.ToString());


            }

            var final = new RxCompilerResult { Result = results.All(x => x.Result) };
            final.Errors.AddRange(results.SelectMany(x => x.Errors));

            return final;
        }
        public static async Task<RxCompilerResult> CompileAsync(this IProjectDefinition project)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            var results = new List<RxCompilerResult>();
            var allCompilers = ObjectBuilder.GetObject<IDeveloperService>().ProjectBuilders;
            logger.WriteDebug($"Found {allCompilers.Length} IProjectBuilders");
            var compilers = allCompilers
                .Where(x => x.IsAvailableInBuildMode)
                .ToArray();// TODO: .AsQueryable().WhereIf(x => this.Compilers.Contains(x.Name), Compilers.Length > 0).ToArray();
            logger.WriteDebug($"Found {compilers.Length} compilers matching the name");
            foreach (var compiler in compilers)
            {
                logger.WriteInfo($"Building with {compiler.Name}...");
                var temp = new FileInfo(Path.GetTempFileName() + ".dll");
                var temp2 = new FileInfo(Path.GetTempFileName() + ".pdb");

                //TODO : read Debug and Verbose from the command line option
                var options2 = new CompilerOptions2(temp.FullName, temp2.FullName) { IsDebug = true, IsVerbose = true };
                var cr = await compiler.BuildAsync(project, x => options2);
                results.Add(cr);

                logger.WriteInfo($"{compiler.GetType().Name} has {(cr.Result ? "successfully building" : "failed to build")} {project.Name}");
                logger.WriteInfo(cr.ToString());

                var dll = $"{ConfigurationManager.CompilerOutputPath}\\{project.AssemblyName}.dll";
                var pdb = $"{ConfigurationManager.CompilerOutputPath}\\{project.AssemblyName}.pdb";
                if (!cr.IsEmpty && temp.Exists && temp.Length > 0)
                    temp.CopyTo(dll, true);
                if (!cr.IsEmpty && temp2.Exists && temp2.Length > 0)
                    temp2.CopyTo(pdb, true);
            }

            var final = new RxCompilerResult { Result = results.All(x => x.Result) };
            final.Errors.AddRange(results.SelectMany(x => x.Errors));

            return final;
        }

    }
}
