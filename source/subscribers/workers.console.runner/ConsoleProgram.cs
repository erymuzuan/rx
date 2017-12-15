using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;
using Bespoke.Sph.SubscribersInfrastructure;

namespace Bespoke.Sph.MessagingClients
{
    public static class ConsoleProgram
    {
        public static async Task<int> Main()
        {
            if (ParseArgExist("?"))
            {
                PrintHelp();
                return 0;
            }
            Console.WriteLine("Use /? for help");
            Console.WriteLine("(c) 2014 Bespoke Technology Sdn. Bhd.");
            Console.WriteLine();
            var debug = ParseArgExist("debug");
            var workerProcess = Process.GetCurrentProcess();
            var envName = ParseArg("env") ?? "dev";
            var configName = ParseArg("config") ?? "all";
            Console.Title = $"Current process :[{workerProcess.Id}] {workerProcess.ProcessName}/{envName}.{configName}";
            if (debug)
            {
                Console.WriteLine($"Attach your debugger to [{workerProcess.Id}] {workerProcess.ProcessName}");
                Console.WriteLine("Press [ENTER] to continue");
                Console.ReadLine();
            }
            // remove files marked for deletion
            RemoveFilesMarkForDeletion();


            ConfigurationManager.AddConnectionString();

            var sw = new Stopwatch();
            sw.Start();
            Console.WriteLine("Stopwatch started");

            var log = new Logger();
            if (ParseArg("log") == "console")
            {
                log.Loggers.Add(new ConsoleLogger { TraceSwitch = Severity.Debug });
            }
            else
            {
                log.Loggers.Add(new EventLogNotification { TraceSwitch = Severity.Info });
            }
            var fileOut = ParseArg("out");
            var fileOutSize = ParseArg("outSize") ?? "100KB";
            var fileTraceSwitch = ParseArg("outSwitch") ?? "Debug";
            const double BUFFER_SIZE = 100d;
            if (!string.IsNullOrWhiteSpace(fileOut))
            {
                log.Loggers.Add(new FileLogger(fileOut, FileLogger.Interval.Day, fileOutSize, BUFFER_SIZE)
                {
                    TraceSwitch = (Severity)Enum.Parse(typeof(Severity), fileTraceSwitch, true)
                });
            }


            var title = $"[{envName}.{configName}][{workerProcess.Id}] Broker:";
            log.WriteInfo(Console.Title = title);

            var configFile = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(WorkersConfig)}\\{envName}.{configName}.json";
            if (!File.Exists(configFile))
            {
                Console.WriteLine($"Cannot find subscribers config in '{configFile}'");
                return -1;
            }
            var options = configFile.DeserializeFromJsonFile<WorkersConfig>();

            var program = new Program(options.SubscriberConfigs.ToArray())
            {
                NotificationService = log
            };

            SubscriberMetadata[] metadata;
            using (var discoverer = new Isolated<Discoverer>())
            {
                metadata = discoverer.Value.Find();
            }
            foreach (var mt in metadata)
            {
                mt.QueueName = options.SubscriberConfigs.SingleOrDefault(
                    x => x.FullName == mt.FullName
                    && x.Assembly == Path.GetFileNameWithoutExtension(mt.Assembly)
                    )?.QueueName ?? "XXXX";
                Console.WriteLine(mt.FullName);
            }
            metadata.Select(d => d.FullName).ToList().ForEach(Console.WriteLine);

            var stopFlag = new AutoResetEvent(false);
            Console.CancelKeyPress += (s, ce) =>
            {
                Console.WriteLine("The workers is shutting down...");
                program.Stop();
                stopFlag.Set();
            };

            var discoverElapsed = sw.Elapsed;
            await program.StartAsync(metadata);

            var span = sw.Elapsed;
            sw.Stop();
            Console.WriteLine("{0} seconds taken to discover the subscribers", discoverElapsed.TotalSeconds);
            Console.WriteLine("{0} seconds taken to start the console", span.TotalSeconds);
            Console.WriteLine("Welcome to [SPH] Type ctrl + c to quit at any time.");


            stopFlag.WaitOne();
            return 0;
        }

        private static void RemoveFilesMarkForDeletion()
        {
            var mark = $"{ConfigurationManager.SubscriberPath}\\mark.for.delete.txt";
            if (File.Exists(mark))
            {
                var files = File.ReadAllLines(mark).Select(x => $"{ConfigurationManager.SubscriberPath}\\{x}");
                foreach (var file in files.Where(File.Exists))
                {
                    File.Delete(file);
                }
                File.Delete(mark);
            }
        }

        private static string ParseArg(string name)
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

        private static void PrintHelp()
        {
            Console.WriteLine("Starts a RabbitMQ subscribers");
            Console.WriteLine("Use these command line parameter to specify your options");
            Console.WriteLine(
                "     /h:<host name> the name of the RabbitMq host, or the IP address, the default is locahost");
            Console.WriteLine(
                "     /v:<virtual host> the name of the virtual host on your RabbitMq Server, the default DevV1");
            Console.WriteLine("     /u:<user name> username used to connect to RabbitMq server, the default is guest");
            Console.WriteLine("     /p:<password> password used to connect to RabbitMq server, the default is guest");
            Console.WriteLine(
                "     /port:<port number> the port number for connection to RabbitMq, the default is 5672");
            Console.WriteLine(
                "     /i:<instance name> if you need to run multiple instance of console.worker then you need to give them diferrent instance name");
            Console.WriteLine("     /debug a switch to halt the loading so that you can attach a debugger ");
            Console.WriteLine(
                "     /log:<log name> to output the log to console use /log:console, else it will use Windows event logs");
        }
    }
}