using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Extensions;

namespace domain.test.Extensions
{
    public static class EntityDefinitionExtension
    {
        public static async Task<RxCompilerResult> CompileWithCsharpAsync(this IProjectDefinition project)
        {
            var compiler = new Bespoke.Sph.Csharp.CompilersServices.EntityDefinitionCompiler();
            var logger = ObjectBuilder.GetObject<ILogger>();


            var tempFileName = Path.GetTempFileName();
            var temp = new FileInfo(tempFileName + ".dll");
            var temp2 = new FileInfo(tempFileName + ".pdb");

            var options = new CompilerOptions2(temp.FullName, temp2.FullName) { IsDebug = true, IsVerbose = true };
            var cr = await compiler.BuildAsync(project, x => options);


            logger.WriteInfo($"{compiler.GetType().Name} has {(cr.Result ? "successfully building" : "failed to build")} {project.Name}");
            logger.WriteInfo(cr.ToString());

            var dll = $"{ConfigurationManager.CompilerOutputPath}\\{project.AssemblyName}.dll";
            var pdb = $"{ConfigurationManager.CompilerOutputPath}\\{project.AssemblyName}.pdb";
            if (!cr.IsEmpty && temp.Exists && temp.Length > 0)
                temp.CopyTo(dll, true);
            if (!cr.IsEmpty && temp2.Exists && temp2.Length > 0)
                temp2.CopyTo(pdb, true);


            return cr;
        }
    }
}
