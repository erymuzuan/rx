using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Extensions;
using Newtonsoft.Json.Linq;
using Console = Colorful.Console;

namespace Bespoke.Sph.SourceBuilders
{
    public static class Program
    {
        private static void DrawProgressBar<T>(int complete, int maxVal = 100, int barSize = 10, char progressCharacter = '-')
        {
            var sourcePath = ConfigurationManager.SphSourceDirectory + $"\\{typeof(T).Name}";
            if (!Directory.Exists(sourcePath))
                Directory.CreateDirectory(sourcePath);
            var current = complete + Directory.GetFiles(sourcePath, "*.json", SearchOption.AllDirectories).Length;
            Console.WriteLine($"Progress ... {progressCharacter}{barSize}{maxVal}{current}", Color.DarkGray);
        }

        public static async Task Main(string[] args)
        {
            Console.WriteAscii("Reactive Developer 1.8", Color.Aqua);
            '-'.WriteFrame();
            " ".WriteMessage(Color.Bisque);
            "Reactive Developer".WriteMessage(Color.Cyan);
            " ".WriteMessage(Color.Cyan);
            "Welcome to Rx Developer platform command line build tools".WriteMessage(Color.Yellow);
            "This tool will truncate all your system data(sph) and rebuild it from your source folder".WriteMessage(Color.Red);

            '-'.WriteFrame();
            Console.ResetColor();

            var debug = ParseArgExist("d", "debug");
            if (debug)
            {
                $"Current Process [{Process.GetCurrentProcess().ProcessName}]({Process.GetCurrentProcess().Id}) ".WriteLine();
                "Press [ENTER] to continue : to exit Ctrl + c ".WriteLine();
                Console.ReadLine();
            }

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var logger = new Logger();
            logger.Loggers.Add(new ConsoleLogger { TraceSwitch = Severity.Info });
            if (TryParseArg("switch", out var tsw))
            {
                var ts = (Severity)Enum.Parse(typeof(Severity), tsw, true);
                logger.Loggers.OfType<ConsoleLogger>().Single().TraceSwitch = ts;
                if (TryParseArg("out", out var outputFile))
                {
                    logger.Loggers.Add(new FileLogger(outputFile, FileLogger.Interval.Hour) { TraceSwitch = ts });
                }
            }
            ObjectBuilder.AddCacheList<ILogger>(logger);

            if (args.Length > 0)
            {
                await BuildWithArgsAsync(args).ConfigureAwait(false);
                return;
            }

            await BuilAllAsyc().ConfigureAwait(false);

        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var entry = new LogEntry(e.ExceptionObject as Exception);
            var logger = ObjectBuilder.GetObject<ILogger>();
            logger.Log(entry);
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs e)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();

            var dll = e.Name.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .First().Trim();
            logger.WriteDebug($"Loading {e.Name}\r\nFile name :  {dll}");

            var output = $"{ConfigurationManager.CompilerOutputPath}\\{dll}.dll";
            if (File.Exists(output))
                return Assembly.LoadFile(output);
            var web = $"{ConfigurationManager.WebPath}\\bin\\{dll}.dll";
            if (File.Exists(web))
                return Assembly.LoadFile(web);
            var sub = $"{ConfigurationManager.SubscriberPath}\\{dll}.dll";
            if (File.Exists(sub))
                return Assembly.LoadFile(sub);

            logger.WriteError($"Cannot find assembly for {e.Name}");
            throw new Exception("Cannot find any assembly for " + e.Name);
        }

        private static async Task BuildWithArgsAsync(IEnumerable<string> args)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            foreach (var f in args)
            {
                if (!File.Exists(f)) continue;
                var json = File.ReadAllText(f);
                var o = JObject.Parse(json);
                var typeName = o.SelectToken("$.$type").Value<string>();
                var type = Strings.GetType(typeName);
                if (null == type)
                {
                    logger.WriteWarning($"Unrecognized type {typeName}  in {f}");
                    continue;
                }

                logger.WriteInfo($"Compiling [{type.Name}]: {Path.GetFileNameWithoutExtension(f)}");
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
                if (type == typeof(OperationEndpoint))
                {
                    await BuildAsync<OperationEndpoint, OperationEndpointBuilder>(json);
                }
                if (type == typeof(QueryEndpoint))
                {
                    await BuildAsync<QueryEndpoint, QueryEndpointBuilder>(json);
                }
                if (type == typeof(ReceiveLocation))
                {
                    await BuildAsync<ReceiveLocation, ReceiveLocationBuilder>(json);
                }
                if (type == typeof(ReceivePort))
                {
                    await BuildAsync<ReceivePort, ReceivePortBuilder>(json);
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

            DrawProgressBar<EntityDefinition>(40);
            var edBuilder = new EntityDefinitionBuilder();
            edBuilder.Initialize();
            await edBuilder.RestoreAllAsync();

            // TODO : we got bugs here, why can't we compile adapters with just *.json file
            DrawProgressBar<Adapter>(50);
            var adapterBuilder = new AdapterBuilder();
            await adapterBuilder.RestoreAllAsync();

            // NOTE : since map normally depends on adapter, this could fail miserably
            DrawProgressBar<TransformDefinition>(60);
            var mapBuilder = new TransformDefinitionBuilder();
            mapBuilder.Initialize();
            await mapBuilder.RestoreAllAsync();

            // NOTE : and WorkflowDefinition may depends on adapter/map, this could fail
            DrawProgressBar<WorkflowDefinition>(70);
            var wdBuilder = new WorkflowDefinitionBuilder();
            wdBuilder.Initialize();
            await wdBuilder.RestoreAllAsync();

            DrawProgressBar<Trigger>(80);
            var triggerBuilder = new TriggerBuilder();
            triggerBuilder.Initialize();
            await triggerBuilder.RestoreAllAsync();

            DrawProgressBar<Designation>(100);
            var roleBuilder = new DesignationBuilder();
            roleBuilder.Initialize();
            await roleBuilder.RestoreAllAsync();

        }


        public static void WriteMessage(this string message, Color color)
        {
            var width = Console.BufferWidth - 1;
            var margin = (width - 2 - message.Length) / 2;

            if (message.Length >= width)
            {
                message.Substring(0, width - 10).WriteMessage(color);
                message.Substring(width - 10).WriteMessage(color);
                return;
            }

            Console.WriteLine(message.Length % 2 == 0 ? "|{0}{1} {0}|" : "|{0}{1}{0}|", new string(' ', margin), message, color);
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


        private static bool TryParseArg(string key, out string value)
        {
            value = ParseArg(key);
            return !string.IsNullOrWhiteSpace(value);

        }

        private static string ParseArg(params string[] keys)
        {
            IEnumerable<string> GetValue(string name)
            {
                var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
                yield return val?.Replace("/" + name + ":", string.Empty);
            }

            return keys.Select(GetValue).Where(x => null != x).SelectMany(x => x).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
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
                yield return default;
            }

            return keys.Select(GetValue).Where(x => null != x).SelectMany(x => x).FirstOrDefault(x => x.HasValue);
        }

        private static bool ParseArgExist(params string[] keys)
        {
            IEnumerable<bool> GetValue(string name)
            {
                var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var val = args.SingleOrDefault(a => a.StartsWith("/" + name));
                yield return null != val;
            }

            return keys.Select(GetValue).Where(x => null != x).SelectMany(x => x).Any(x => x);
        }


    }
}
