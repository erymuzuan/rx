using System.IO;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Domain.Compilers;

namespace Bespoke.Sph.Csharp.CompilersServices.Extensions
{
    public static class ClassExtension
    {
        public static string WriteSource(this Class @class, IProjectDefinition project)
        {
            var file = Path.GetFileNameWithoutExtension(@class.FileName);

            var folder = $@"{ConfigurationManager.GeneratedSourceDirectory}\{project.GetType().Name}.{project.Name}";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fullPath = $@"{ConfigurationManager.GeneratedSourceDirectory}\{project.GetType().Name}.{project.Name}\{file}.cs";
            File.WriteAllText(fullPath, @class.GetCode());
            return fullPath;
        }
    }
}
