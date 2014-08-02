using System;
using System.IO;
using System.Text.RegularExpressions;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json.Linq;

namespace csproj.gen
{
    class Program
    {
        static string RegexSingleValue(string input, string pattern, string group)
        {
            const RegexOptions REGEX_OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            var matches = Regex.Matches(input, pattern, REGEX_OPTIONS);
            return matches.Count == 1 ? matches[0].Groups[@group].Value.Trim() : null;
        }
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
                Console.WriteLine("Cannot find {0}", file);
                return;
            }
            try
            {
                var proj = new Project();
                var json = File.ReadAllText(file);
                var item = json.DeserializeFromJson<Adapter>();

                var o = JObject.Parse(json);
                var an = o.SelectToken("$.name").Value<string>();

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
