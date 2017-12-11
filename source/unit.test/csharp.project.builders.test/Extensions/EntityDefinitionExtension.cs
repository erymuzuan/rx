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
        public static async Task<dynamic> CreateInstanceAsync(this OperationEndpoint qe, bool verbose = false)
        {
            var compiler = new Csharp.CompilersServices.OperationEndpointCompiler();
            using (var stream = new MemoryStream())
            {
                var options = new CompilerOptions2(stream);
                var result = await compiler.BuildAsync(qe, x => options);
                if (!result.Result) return null;

                var assembly = Assembly.Load(stream.ToArray());
                var edTypeName = $"{qe.CodeNamespace}.{qe.TypeName}";

                var edType = assembly.GetType(edTypeName);
                return Activator.CreateInstance(edType);
            }
        }
        public static async Task<dynamic> CreateInstanceAsync(this QueryEndpoint qe, bool verbose = false)
        {
            var compiler = new Csharp.CompilersServices.QueryEndpointCompiler();
            using (var stream = new MemoryStream())
            {
                var options = new CompilerOptions2(stream);
                var result = await compiler.BuildAsync(qe, x => options);
                if (!result.Result) return null;

                var assembly = Assembly.Load(stream.ToArray());
                var edTypeName = $"{qe.CodeNamespace}.{qe.Name}";

                var edType = assembly.GetType(edTypeName);
                return Activator.CreateInstance(edType);
            }
        }

        public static async Task<dynamic> CreateInstanceAsync(this EntityDefinition ed, bool verbose = false)
        {
            var compiler = new Csharp.CompilersServices.EntityDefinitionCompiler { BuildDiagnostics = Array.Empty<IBuildDiagnostics>() };
            using (var stream = new MemoryStream())
            {
                var options = new CompilerOptions2(stream);
                var result = await compiler.BuildAsync(ed, x => options);
                if (!result.Result) return null;

                var assembly = Assembly.Load(stream.ToArray());
                var edTypeName = $"{ed.CodeNamespace}.{ed.Name}";

                var edType = assembly.GetType(edTypeName);
                return Activator.CreateInstance(edType);
            }
        }
    }

}
