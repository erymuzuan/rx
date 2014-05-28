using System.IO;
using System.Reflection;
using Bespoke.Sph.Domain;
using RazorEngine;
using RazorEngine.Templating;

namespace csproj.gen
{
    public class Project
    {
        public string GetNugetPackagesConfig()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "csproj.gen.packages.xml";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var raw = reader.ReadToEnd();
                return raw;
            }

        }
        public string Generate(EntityDefinition item, string appName)
        {
            var vm = new
            {
                EntityDefinition = item,
                ApplicationName = appName,
                CsFiles = new string[] { }
            };

            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "csproj.gen.ed.xml";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var raw = reader.ReadToEnd();
                var markup = this.TransformTemplate(raw, vm);
                return markup;

            }

        }

        public string TransformTemplate(string template, dynamic model)
        {
            dynamic viewBag = new DynamicViewBag();
            //viewBag.BaseUrl = ConfigurationManager.BaseUrl;
            //viewBag.ApplicationName = ConfigurationManager.ApplicationName;
            //viewBag.ApplicationFullName = ConfigurationManager.ApplicationFullName;

            if (string.IsNullOrWhiteSpace(template)) return string.Empty;
            var body = Razor.Parse(template, model, viewBag, null);
            return body;
        }
    }
}