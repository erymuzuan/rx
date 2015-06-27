using System;
using System.IO;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json.Linq;

namespace adapter.csproj.gen
{
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please provide the path to adapter json");
                return;
            }
            var file = args[0];
            if (!File.Exists(file))
            {
                Console.WriteLine(@"Cannot find {0}", file);
                return;
            }
            try
            {
                var proj = new Project();
                var json = File.ReadAllText(file);
                var item = json.DeserializeFromJson<Adapter>();

                var o = JObject.Parse(json);
                var an = o.SelectToken("$.Name").Value<string>();

                Console.WriteLine(@"Application name : {0}", an);

                var xml = proj.Generate(item, an);
                var nuget = proj.GetNugetPackagesConfig();
                var folder = Path.Combine(ConfigurationManager.GeneratedSourceDirectory, item.Name);

                var output = Path.Combine(folder, item.Name + ".csproj");
                var packageOutput = Path.Combine(folder, "packages.config");
                File.WriteAllText(output, xml);
                File.WriteAllText(packageOutput, nuget);
                Console.WriteLine(@"Successfully write the project file to " + output);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
            }

        }


    }
}
