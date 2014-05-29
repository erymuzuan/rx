using System;
using System.IO;
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
            "Enter \"y\" to continue".WriteLine();

            var cont = Console.ReadLine();
            if (cont != "y")
            {
                Console.WriteLine("BYE.");
                return;
            }

            if (args.Length > 0)
            {
                foreach (var f in args)
                {
                    if(!File.Exists(f))continue;
                    var json = File.ReadAllText(f);
                    var o = JObject.Parse(json);
                    var typeName = o.SelectToken("$.$type").Value<string>();
                    var type = Type.GetType(typeName);

                   

                    if (type == typeof (EntityDefinition))
                    {
                        var item = json.DeserializeFromJson<EntityDefinition>();
                        var edb = new EntityDefinitionBuilder();
                        edb.Initialize();
                        edb.RestoreAsync(item).Wait();
                    }
                }
                return;
            }


            var edBuilder = new EntityDefinitionBuilder();
            edBuilder.Initialize();
            edBuilder.RestoreAllAsync().Wait();

            var wdBuilder = new WorkflowDefinitionBuilder();
            wdBuilder.RestoreAllAsync().Wait();


            var triggerBuilder = new TriggerBuilder();
            triggerBuilder.Initialize();
            triggerBuilder.RestoreAllAsync().Wait();


            var chartBuilder = new Builder<EntityChart>();
            chartBuilder.Initialize();
            chartBuilder.RestoreAllAsync().Wait();

            var formBuilder = new EntityFormBuilder();
            formBuilder.Initialize();
            formBuilder.RestoreAllAsync().Wait();

            var viewBuilder = new EntityViewBuilder();
            viewBuilder.Initialize();
            viewBuilder.RestoreAllAsync().Wait();

            var orgBuilder = new Builder<Organization>();
            orgBuilder.Initialize();
            orgBuilder.RestoreAllAsync().Wait();

            var settingBuilder = new Builder<Setting>();
            settingBuilder.Initialize();
            settingBuilder.RestoreAllAsync().Wait();

            var pageBuilder = new Builder<Page>();
            pageBuilder.Initialize();
            pageBuilder.RestoreAllAsync().Wait();

            var designationBuilder = new Builder<Designation>();
            designationBuilder.Initialize();
            designationBuilder.RestoreAllAsync().Wait();


            var rdlBuilder = new Builder<ReportDefinition>();
            rdlBuilder.Initialize();
            rdlBuilder.RestoreAllAsync().Wait();

            var rsBuilder = new Builder<ReportDelivery>();
            rsBuilder.Initialize();
            rsBuilder.RestoreAllAsync().Wait();

            var etBuilder = new Builder<EmailTemplate>();
            etBuilder.Initialize();
            etBuilder.RestoreAllAsync().Wait();

            var dtBuilder = new DocumentTemplateBuilder();
            dtBuilder.Initialize();
            dtBuilder.RestoreAllAsync().Wait();
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

            if (message.Length % 2 == 0)
                Console.WriteLine("|{0}{1} {0}|", new String(' ', margin), message);
            else
                Console.WriteLine("|{0}{1}{0}|", new String(' ', margin), message);
        }
        public static void WriteFrame(this char frame)
        {
            var width = Console.BufferWidth - 1;

            Console.WriteLine("|{0}|", new String(frame, width));
        }
        public static void WriteLine(this string message)
        {
            Console.WriteLine(message);
        }

    }
}
