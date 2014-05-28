using System;
using System.IO;
using System.Text.RegularExpressions;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace csproj.gen
{
    class Program
    {
        static string RegexSingleValue(string input, string pattern, string group)
        {
            const RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            var matches = Regex.Matches(input, pattern, option);
            return matches.Count == 1 ? matches[0].Groups[@group].Value.Trim() : null;
        }
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please provide the path to entity definition json");
                return;
            }
            var file = args[0];
            if (!File.Exists(file))
            {
                Console.WriteLine("Cannot find {0}", file);
                return;
            }
            try
            {
                var proj = new Project();
                var json = File.ReadAllText(file);
                var item = json.DeserializeFromJson<EntityDefinition>();

                var o = JObject.Parse(json);
                var an = RegexSingleValue(o.SelectToken("$.CodeNamespace").Value<string>(), @"Bespoke\.(?<an>.*?)_[0-9]{1,}\.Domain", "an");

                Console.WriteLine("Application name : {0}", an);

                var xml = proj.Generate(item,an);
                var nuget = proj.GetNugetPackagesConfig();

                var output = string.Format(@".\sources\{0}\{0}.csproj", item.Name);
                var packageOutput = string.Format(@".\sources\{0}\packages.config", item.Name);
                File.WriteAllText(output, xml);
                File.WriteAllText(packageOutput, nuget);
                Console.WriteLine("Successfully write the project file to " + output);
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
