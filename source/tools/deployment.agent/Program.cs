using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SqlRepository;
using Bespokse.Sph.ElasticsearchRepository;
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
                Console.WriteLine(@"

Usage :
deployment.agent <path-to-entity-definition-source>|/e:<entity-definition-name>|/e:entity-definition-id>|/nes

/nes No elasticsearch migration, specify this switch when you want to skip Elasticsearch

For EntityDefinition with Treat data as source:
/truncate   will truncate the existing table if exist, and load the data from source files
the default option is to migrate the existing data and append any new source from your source files


");
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
            var ed = file.DeserializeFromJsonFile<EntityDefinition>();

            var tableBuilder = new TableSchemaBuilder(WriteMessage);
            tableBuilder.BuildAsync(ed)
                .Wait();

            // TODO : for DataAsSource, truncate or append ? the default should be append, what if the row changed from the source??
            if (ed.TreatDataAsSource)
            {
                var truncate = ParseArgExist("truncate");
                var sourceMigrator = new SourceTableBuilder(WriteMessage, WriteWarning, WriteError);
                if (truncate)
                    sourceMigrator.CleanAndBuildAsync(ed).Wait();
                else
                    sourceMigrator.BuildAsync(ed).Wait();

            }

            using (var mappingBuilder = new MappingBuilder(WriteMessage, WriteWarning, WriteError))
            {
                var nes = ParseArgExist("nes");
                if (!nes)
                    mappingBuilder.BuildAllAsync(ed).Wait();

            }


            Console.WriteLine($@"{ed.Name} was succesfully deployed ");
        }

        private static void WriteMessage(string m)
        {
            Console.WriteLine($@"{DateTime.Now:T} : {m}", Color.Cyan);
        }

        private static void WriteWarning(string m)
        {
            Console.WriteLine($@"{DateTime.Now:T} : {m}", Color.Yellow);
        }

        private static void WriteError(Exception m)
        {
            Console.WriteLine($@"{DateTime.Now:T} : {m}", Color.OrangeRed);
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
    }
}
