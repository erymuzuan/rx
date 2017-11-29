using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace domain.test.Extensions
{
    public static class EntityDefinitionExtension
    {
        public static async Task<RxCompilerResult> CompileWithCsharpAsync(this EntityDefinition ed)
        {
            var compiler = new Bespoke.Sph.Csharp.CompilersServices.EntityDefinitionCompiler();
            var classes = await compiler.GenerateCodeAsync(ed);
            var sources = (from cs in classes.Keys
                let name = Path.GetFileName(cs)
                let tem = Path.GetTempPath() + "\\" + name
                select new
                {
                    FileName = tem,
                    Code = classes[cs]
                }).ToList();
            sources.ForEach(x => File.WriteAllText(x.FileName, x.Code));
            var result = await compiler.BuildAsync(ed, sources.Select(x => x.FileName).ToArray());

            return result;
        }
    }
}
