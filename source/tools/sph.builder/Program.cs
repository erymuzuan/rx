using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace sph.builder
{
    public static class Program
    {

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            '-'.WriteFrame();
            " ".WriteMessage();
            "Reactive Developer".WriteMessage();
            " ".WriteMessage();
            "Welcome to Rx Developer platform command line build tools".WriteMessage();
            "This tool will truncate all your system data(sph) and rebuild it from your source folder".WriteMessage();

            '-'.WriteFrame();
            Console.ResetColor();

            var quiet = args.Contains("/s") || args.Contains("/q") || args.Contains("/silent") || args.Contains("/quiet");
            if (!quiet)
            {

                "Enter \"y\" to continue".WriteLine();
                var cont = Console.ReadLine();
                if (cont != "y")
                {
                    Console.WriteLine(@"BYE.");
                    return;
                }
            }

            if (args.Length > 0)
            {
                BuildWithArgsAsync(args).Wait();
                return;
            }

            BuildAssAsync().Wait();

        }

        private static async Task BuildWithArgsAsync(IEnumerable<string> args)
        {

            foreach (var f in args)
            {
                if (!File.Exists(f)) continue;
                var json = File.ReadAllText(f);
                var o = JObject.Parse(json);
                var typeName = o.SelectToken("$.$type").Value<string>();
                var type = Type.GetType(typeName);


                if (type == typeof(EntityDefinition))
                {
                    await BuildEntityDefinitionAsync(json);
                }
                if (type == typeof(WorkflowDefinition))
                {
                    await BuildWorkflowAsync(json);
                }
            }
        }

        private static async Task BuildWorkflowAsync(string json)
        {
            var wd = json.DeserializeFromJson<WorkflowDefinition>();
            var builder = new WorkflowDefinitionBuilder();
            builder.Initialize();
            await builder.RestoreAsync(wd);
        }

        private static async Task BuildEntityDefinitionAsync(string json)
        {
            var item = json.DeserializeFromJson<EntityDefinition>();

            var edb = new EntityDefinitionBuilder();
            edb.Initialize();
            await edb.RestoreAsync(item);

            // now build the EntityForm
            var formBuilder = new EntityFormBuilder();
            formBuilder.Initialize();
            var formTasks = from ff in GetJsonFiles(typeof(EntityForm))
                            let fjson = File.ReadAllText(ff)
                            let fo = JObject.Parse(fjson)
                            let edid = fo.SelectToken("$.EntityDefinitionId").Value<string>()
                            where edid == item.Id
                            select formBuilder.RestoreAsync(fjson.DeserializeFromJson<EntityForm>());
            await Task.WhenAll(formTasks);

            // then build the EntityView
            var viewBuilder = new EntityViewBuilder();
            viewBuilder.Initialize();
            var viewTasks = from ff in GetJsonFiles(typeof(EntityView))
                            let fjson = File.ReadAllText(ff)
                            let fo = JObject.Parse(fjson)
                            let edidToken = fo.SelectToken("$.EntityDefinitionId")
                            where null != edidToken
                            let edid = edidToken.Value<string>()
                            where edid == item.Id
                            select viewBuilder.RestoreAsync(fjson.DeserializeFromJson<EntityView>());
            await Task.WhenAll(viewTasks);

            // then build the charts
            var chartBuilder = new Builder<EntityChart>();
            chartBuilder.Initialize();
            var chartTasks = from ff in GetJsonFiles(typeof(EntityChart))
                             let fjson = File.ReadAllText(ff)
                             let fo = JObject.Parse(fjson)
                             let edid = fo.SelectToken("$.EntityDefinitionId").Value<string>()
                             where edid == item.Id
                             select chartBuilder.RestoreAsync(fjson.DeserializeFromJson<EntityChart>());
            await Task.WhenAll(chartTasks);

            // then the triggers
            var triggerBuilder = new TriggerBuilder();
            triggerBuilder.Initialize();
            var triggerTasks = from ff in GetJsonFiles(typeof(Trigger))
                               let fjson = File.ReadAllText(ff)
                               let fo = JObject.Parse(fjson)
                               let ent = fo.SelectToken("$.Entity").Value<string>()
                               where ent == item.Name
                               select triggerBuilder.RestoreAsync(fjson.DeserializeFromJson<Trigger>());
            await Task.WhenAll(triggerTasks);
        }

        private static IEnumerable<string> GetJsonFiles(Type type)
        {
            var triggerfolder = Path.Combine(ConfigurationManager.SphSourceDirectory, type.Name);
            return Directory.GetFiles(triggerfolder, "*.json");
        }

        private static async Task BuildAssAsync()
        {

            var edBuilder = new EntityDefinitionBuilder();
            edBuilder.Initialize();
            await edBuilder.RestoreAllAsync();

            var wdBuilder = new WorkflowDefinitionBuilder();
            await wdBuilder.RestoreAllAsync();


            var triggerBuilder = new TriggerBuilder();
            triggerBuilder.Initialize();
            await triggerBuilder.RestoreAllAsync();


            var chartBuilder = new Builder<EntityChart>();
            chartBuilder.Initialize();
            await chartBuilder.RestoreAllAsync();

            var formBuilder = new EntityFormBuilder();
            formBuilder.Initialize();
            await formBuilder.RestoreAllAsync();

            var viewBuilder = new EntityViewBuilder();
            viewBuilder.Initialize();
            await viewBuilder.RestoreAllAsync();

            var orgBuilder = new Builder<Organization>();
            orgBuilder.Initialize();
            await orgBuilder.RestoreAllAsync();

            var settingBuilder = new Builder<Setting>();
            settingBuilder.Initialize();
            await settingBuilder.RestoreAllAsync();

            var pageBuilder = new Builder<Page>();
            pageBuilder.Initialize();
            await pageBuilder.RestoreAllAsync();

            var designationBuilder = new Builder<Designation>();
            designationBuilder.Initialize();
            await designationBuilder.RestoreAllAsync();


            var rdlBuilder = new Builder<ReportDefinition>();
            rdlBuilder.Initialize();
            await rdlBuilder.RestoreAllAsync();

            var rsBuilder = new Builder<ReportDelivery>();
            rsBuilder.Initialize();
            await rsBuilder.RestoreAllAsync();

            var etBuilder = new Builder<EmailTemplate>();
            etBuilder.Initialize();
            await etBuilder.RestoreAllAsync();

            var dtBuilder = new DocumentTemplateBuilder();
            dtBuilder.Initialize();
            await dtBuilder.RestoreAllAsync();
        }

        public static void WriteMessage(this string message)
        {
            var width = Console.BufferWidth - 1;
            var margin = (width - 2 - message.Length) / 2;

            if (message.Length >= width)
            {
                message.Substring(0, width - 10).WriteMessage();
                message.Substring(width - 10).WriteMessage();
                return;
            }

            Console.WriteLine(message.Length % 2 == 0 ? "|{0}{1} {0}|" : "|{0}{1}{0}|", new string(' ', margin), message);
        }
        public static void WriteFrame(this char frame)
        {
            var width = Console.BufferWidth - 1;
            Console.WriteLine("|{0}|", new string(frame, width));
        }
        public static void WriteLine(this string message)
        {
            Console.WriteLine(message);
        }

    }
}
