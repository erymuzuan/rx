using System.IO;
using System.Linq;
using System.Reflection;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using RazorEngine;
using RazorEngine.Templating;

namespace adapter.csproj.gen
{
    public class Project
    {
        public string GetNugetPackagesConfig()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_NAME = "adapter.csproj.gen.packages.xml";

            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME))
            using (var reader = new StreamReader(stream))
            {
                var raw = reader.ReadToEnd();
                return raw;
            }

        }
        public string Generate(Adapter item, string appName)
        {
            var folder = Path.Combine(ConfigurationManager.GeneratedSourceDirectory, item.Name);
            var files = Directory.GetFiles(folder, "*.cs").Select(Path.GetFileName);

            var vm = new
            {
                Adapter = item,
                RootNamespace = $"{ConfigurationManager.ApplicationName}.Adapter.{item.Schema}",
                CsFiles = files,
                AssemblyName = $"{ConfigurationManager.ApplicationName}.{item.Name}"
            };


            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_NAME = "adapter.csproj.gen.ed.xml";

            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME))
            using (var reader = new StreamReader(stream))
            {
                var raw = reader.ReadToEnd();
                var markup = this.TransformTemplate(raw, vm);
                return markup;

            }

        }

        public string TransformTemplate(string template, dynamic model)
        {
            var viewBag = new DynamicViewBag();
            if (string.IsNullOrWhiteSpace(template)) return string.Empty;
            var result = Engine.Razor.RunCompile(template, template, null, (object)model, viewBag);
            return result;
        }
    }
}