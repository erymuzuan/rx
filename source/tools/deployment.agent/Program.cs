using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
                Console.WriteLine(GetHelpText());
                return;
            }

            if (ParseArgExist("debug"))
            {
                Console.WriteLine($"Attach your debugger and to {Process.GetCurrentProcess().ProcessName} and press [ENTER] to continue");
                System.Console.ReadLine();
            }
            if (ParseArgExist("gui", "ui", "i"))
            {
                var gui = new ProcessStartInfo($"{ConfigurationManager.ToolsPath}\\deployment.gui.exe")
                {
                    WorkingDirectory = ConfigurationManager.ToolsPath
                };
                Process.Start(gui);
                return;
            }

            var id = ParseArg("e", "entity", "entity-id", "entity-name");
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

            var nes = ParseArgExist("nes", "NoElasticsearch");
            var truncate = ParseArgExist("truncate", "t");

            await DeploymentMetadata.InitializeAsync();
            var deployment = new DeploymentMetadata(ed);

            if (ParseArgExist("change", "changes", "diff"))
            {
                var plan = await deployment.GetChangesAsync();
                foreach (var change in plan.ChangeCollection.OrderBy(x => x.OldPath).Where(x => !x.IsEmpty))
                {
                    Console.WriteLine("______________________________________________________");
                    Console.WriteLine(change);
                }
                var migrationPlanFile = $"{ed.Name}-{plan.PreviousCommitId}-{plan.CurrentCommitId}";
                Console.WriteLine($"MigrationPlan is saved to {migrationPlanFile}", Color.Yellow);
                File.WriteAllText($@"{ConfigurationManager.SphSourceDirectory}\MigrationPlan\{migrationPlanFile}.json", plan.ToJson());
                return;
            }
            if (ParseArgExist("migrate") && ParseArgExist("whatif"))
            {
                // TODO : delete all the migration dll in output
                // ls bin/output/migration.* | remove-item
                var outputFolder = ParseArg("output");
                await deployment.TestMigrationAsync(ParseArg("plan"), outputFolder);
                return;
            }

            if (ParseArgExist("logs", "l"))
            {
                var istory = await deployment.QueryAsync();
                var table = new ConsoleTable(istory);
                table.PrintTable();
                return;
            }
            var batchSize = ParseArgInt32("batch-size", "size", "batch") ?? 1000;
            await deployment.BuildAsync(truncate, nes, batchSize);


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


        public static string ParseArg(params string[] keys)
        {
            IEnumerable<string> GetValue(string name)
            {

                var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
                yield return val?.Replace("/" + name + ":", string.Empty);
            }

            return keys.Select(GetValue).SelectMany(x => x).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
        }

        public static int? ParseArgInt32(params string[] keys)
        {
            IEnumerable<int?> GetValue(string name)
            {

                var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
                var text = val?.Replace("/" + name + ":", string.Empty);
                if (int.TryParse(text, out int number))
                    yield return number;
                yield return default(int?);
            }

            return keys.Select(GetValue).SelectMany(x => x).FirstOrDefault(x => x.HasValue);
        }


        private static bool ParseArgExist(params string[] keys)
        {
            IEnumerable<bool> GetValue(string name)
            {
                var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var val = args.SingleOrDefault(a => a.StartsWith("/" + name));
                yield return null != val;
            }

            return keys.Select(GetValue).SelectMany(x => x).Any(x => x);
        }

        private static string GetHelpText()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string HELP_TEXT = "Bespoke.Sph.Mangements.HelpText.md";

            using (var stream = assembly.GetManifestResourceStream(HELP_TEXT))
            using (var reader = new StreamReader(stream ?? throw new Exception("Cannot read HelpText.md")))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
