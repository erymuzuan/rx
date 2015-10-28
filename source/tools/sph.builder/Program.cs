using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Mindscape.Raygun4Net;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.SourceBuilders
{
    public static class Program
    {

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException  ;
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
                "press [ENTER] to continue : to exit Ctrl + c".WriteLine();
                Console.ReadLine();
            }

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            ObjectBuilder.AddCacheList<ILogger>(new ConsoleLogger());
            if (args.Length > 0)
            {
                BuildWithArgsAsync(args).Wait();
                return;
            }

            BuilAllAsyc().Wait();

        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var version = "unknown";
            string file = "..\\version.json";
            if (File.Exists(file))
            {
                var json = JObject.Parse(File.ReadAllText(file));
                var build = json.SelectToken("$.build").Value<int>();
                version = build.ToString();
            }
            var client = new RaygunClient("imHU3x8eZamg84BwYekfMQ==")
            {
                ApplicationVersion = version
            };
            client.SendInBackground(e.ExceptionObject as Exception, new List<string>());
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs e)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                var dll = e.Name.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .First().Trim();
                Console.WriteLine($"Loading {e.Name}");
                Console.WriteLine($"File name :  {dll}");
                var output = $"{ConfigurationManager.CompilerOutputPath}\\{dll}.dll";
                if (File.Exists(output))
                    return Assembly.LoadFile(output);
                var web = $"{ConfigurationManager.WebPath}\\bin\\{dll}.dll";
                if (File.Exists(web))
                    return Assembly.LoadFile(web);
                var sub = $"{ConfigurationManager.SubscriberPath}\\{dll}.dll";
                if (File.Exists(sub))
                    return Assembly.LoadFile(sub);
            }
            finally
            {
                Console.ResetColor();
            }
            throw new Exception("Cannot find any assembly for " + e.Name);
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
                if (null == type)
                {
                    Console.WriteLine($"Unrecognized type {typeName}  in {f}");
                    continue;
                }

                if (type == typeof(EntityDefinition))
                {
                    await BuildAsync<EntityDefinition, EntityDefinitionBuilder>(json);
                }
                if (type.BaseType == typeof(Adapter))
                {
                    await BuildAsync<Adapter, AdapterBuilder>(json);
                }
                if (type == typeof(WorkflowDefinition))
                {
                    await BuildAsync<WorkflowDefinition, WorkflowDefinitionBuilder>(json);
                }
                if (type == typeof(Designation))
                {
                    await BuildAsync<Designation, DesignationBuilder>(json);
                }
                if (type == typeof(TransformDefinition))
                {
                    await BuildAsync<TransformDefinition, TransformDefinitionBuilder>(json);
                }
                if (type == typeof(Trigger))
                {
                    await BuildAsync<Trigger, TriggerBuilder>(json);
                }
            }
        }

        private static async Task BuildAsync<T, TBuilder>(string json) where TBuilder : new()
        {
            var item = json.DeserializeFromJson<T>();

            dynamic builder = new TBuilder();
            builder.Initialize();
            await builder.RestoreAsync(item);

        }


        private static async Task BuilAllAsyc()
        {
            if (!Directory.Exists(ConfigurationManager.CompilerOutputPath))
                Directory.CreateDirectory(ConfigurationManager.CompilerOutputPath);


            // TODO : remove all from output and apps
            //RemoveExistingCompiledBinaries(ConfigurationManager.CompilerOutputPath);
            //RemoveExistingCompiledBinaries(Path.Combine(ConfigurationManager.WebPath, "bin"));
            //RemoveExistingCompiledBinaries(ConfigurationManager.SchedulerPath);
            //RemoveExistingCompiledBinaries(ConfigurationManager.SubscriberPath);
            //RemoveExistingCompiledBinaries(ConfigurationManager.ToolsPath);
            RemoveExistingCompiledBinaries(@"c:\\non-existens");

            var edBuilder = new EntityDefinitionBuilder();
            edBuilder.Initialize();
            await edBuilder.RestoreAllAsync();


            // TODO : we got bugs here, why can't we compile adapters with just *.json file
            //var adapterBuilder = new AdapterBuilder();
            //await adapterBuilder.RestoreAllAsync();

            // NOTE : since map normally depends on adapter, this could fail miserably
            var mapBuilder = new TransformDefinitionBuilder();
            mapBuilder.Initialize();
            await mapBuilder.RestoreAllAsync();

            // NOTE : and WorkflowDefinition may depends on adapter/map, this could fail
            var wdBuilder = new WorkflowDefinitionBuilder();
            wdBuilder.Initialize();
            await wdBuilder.RestoreAllAsync();

            var triggerBuilder = new TriggerBuilder();
            triggerBuilder.Initialize();
            await triggerBuilder.RestoreAllAsync();

            var roleBuilder = new DesignationBuilder();
            roleBuilder.Initialize();
            await roleBuilder.RestoreAllAsync();

            DeployCompiledBinaries(Path.Combine(ConfigurationManager.WebPath, "bin"));
            DeployCompiledBinaries(ConfigurationManager.SchedulerPath);
            DeployCompiledBinaries(ConfigurationManager.SubscriberPath);
            DeployCompiledBinaries(ConfigurationManager.ToolsPath);

        }

        private static void RemoveExistingCompiledBinaries(string folder)
        {
            if (!Directory.Exists(folder)) return;
            Directory.GetFiles(folder, $"{ConfigurationManager.ApplicationName}.*.dll")
                .ToList().ForEach(File.Delete);
            Directory.GetFiles(folder, $"{ConfigurationManager.ApplicationName}.*.pdb")
                .ToList().ForEach(File.Delete);
            Directory.GetFiles(folder, "workflows.*.dll")
                .ToList().ForEach(File.Delete);
            Directory.GetFiles(folder, "workflows.*.pdb")
                .ToList().ForEach(File.Delete);
            Directory.GetFiles(folder, "subscriber.trigger.*.dll")
                .Where(f => Path.GetFileName(f) != "subscriber.trigger.dll")
                .ToList().ForEach(File.Delete);
            Directory.GetFiles(folder, "subscriber.trigger.*.pdb")
                .Where(f => Path.GetFileName(f) != "subscriber.trigger.pdb")
                .ToList().ForEach(File.Delete);
        }
        private static void DeployCompiledBinaries(string folder)
        {
            Directory.GetFiles(ConfigurationManager.CompilerOutputPath, ".*.dll")
                .ToList().ForEach(x => File.Copy(x, $"{folder}\\{Path.GetFileName(x)}", true));
            Directory.GetFiles(ConfigurationManager.CompilerOutputPath, ".*.pdb")
                .ToList().ForEach(x => File.Copy(x, $"{folder}\\{Path.GetFileName(x)}", true));
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
