using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Console = Colorful.Console;

namespace Bespoke.Sph.Mangements
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.FirstOrDefault() == "?" || ParseArgExist("?") || args.Length == 0)
            {
                Console.WriteAscii("Deployment Agent", Color.BlueViolet);
                Console.WriteLine(Program.GetHelpText());
                return;
            }

            if (ParseArgExist("gui") || ParseArgExist("ui") || ParseArgExist("i"))
            {
                var gui = new System.Diagnostics.ProcessStartInfo($"{ConfigurationManager.ToolsPath}\\deployment.gui.exe")
                {
                    WorkingDirectory = ConfigurationManager.ToolsPath
                };
                System.Diagnostics.Process.Start(gui);
                return;
            }

            var id = ParseArg("e");
            var file = args.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(id))
                file = $@"{ConfigurationManager.SphSourceDirectory}\EntityDefinition\{id}.json";
            if (!string.IsNullOrWhiteSpace(id) && !File.Exists(file))// try with ED Name
                file = $@"{ConfigurationManager.SphSourceDirectory}\EntityDefinition\{GetEntityDefinitionId(id)}.json";

            if (!File.Exists(file))
            {
                Console.WriteLine(@"Specify your EntityDefinition json source as your first argument or with /e:<EntityDefinition id/name>", Color.Yellow);
                return;
            }

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            var program = new Program();
            program.StartAsync(file).Wait();
        }

        private async Task StartAsync(string file)
        {
            var ed = file.DeserializeFromJsonFile<EntityDefinition>();

            var nes = ParseArgExist("nes");
            var truncate = ParseArgExist("truncate");

            await DeploymentMetadata.InitializeAsync();
            var deployment = new DeploymentMetadata(ed);


            if (ParseArgExist("q"))
            {
                var istory = await deployment.QueryAsync();
                var table = new ConsoleTable(istory);
                table.PrintTable();
                return;
            }

            await deployment.BuildAsync(truncate, nes);

        }


        private static string GetEntityDefinitionId(string name)
        {
            var files = Directory.GetFiles($@"{ConfigurationManager.SphSourceDirectory}\EntityDefinition\", "*.json");
            foreach (var file in files)
            {
                var ed = file.DeserializeFromJsonFile<EntityDefinition>();
                if (ed.Name == name) return ed.Id;
            }

            return null;
        }


        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assembly = args.Name.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .First().Trim();
            var subs = $"{ConfigurationManager.SubscriberPath}\\{assembly}.dll";
            if (File.Exists(subs))
                return Assembly.LoadFile(subs);
            subs = $"{ConfigurationManager.CompilerOutputPath}\\{assembly}.dll";
            if (File.Exists(subs))
                return Assembly.LoadFile(subs);
            throw new Exception("Cannot load " + subs);
        }


        public static string ParseArg(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
            return val?.Replace("/" + name + ":", string.Empty);
        }
        private static bool ParseArgExist(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name));
            return null != val;
        }

        private static string GetHelpText()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string HELP_TEXT = "Bespoke.Sph.Mangements.HelpText.md";

            using (var stream = assembly.GetManifestResourceStream(HELP_TEXT))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
