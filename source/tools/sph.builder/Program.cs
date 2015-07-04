using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.SourceBuilders
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
            
        }
        

        private static async Task BuildAssAsync()
        {

            var edBuilder = new EntityDefinitionBuilder();
            edBuilder.Initialize();
            await edBuilder.RestoreAllAsync();

            var wdBuilder = new WorkflowDefinitionBuilder();
            await wdBuilder.RestoreAllAsync();

            var mapBuilder = new TransformDefinitionBuilder();
            await mapBuilder.RestoreAllAsync();


            var adapterBuilder = new AdapterBuilder();
            await adapterBuilder.RestoreAllAsync();
            
            var triggerBuilder = new TriggerBuilder();
            triggerBuilder.Initialize();
            await triggerBuilder.RestoreAllAsync();
            

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
