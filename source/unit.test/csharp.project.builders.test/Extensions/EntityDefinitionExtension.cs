using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit;

namespace Bespoke.Sph.Tests.Extensions
{

    public static class EntityDefinitionExtension
    {
        private static async Task<RxCompilerResult> CompileWithCsharpAsync(this EntityDefinition ed)
        {
            var compiler = new Csharp.CompilersServices.EntityDefinitionCompiler();
            var result = await compiler.BuildAsync(ed, x => new CompilerOptions2());

            return result;
        }

        public static async Task<dynamic> CreateInstanceAsync(this EntityDefinition ed, bool verbose = false)
        {
            byte[] data;
            using (var stream = new MemoryStream())
            {
                var result = await ed.CompileWithCsharpAsync();
                if (!result.Result) return null;

                data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
            }
            var assembly = Assembly.Load(data);
            var edTypeName = $"{ed.CodeNamespace}.{ed.Name}";

            var edType = assembly.GetType(edTypeName);
            Assert.NotNull(edType);

            return Activator.CreateInstance(edType);
        }
    }

}
