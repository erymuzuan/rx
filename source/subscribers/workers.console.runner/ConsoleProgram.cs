using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Bespoke.Sph.SubscribersInfrastructure;
using INotificationService = Bespoke.Sph.SubscribersInfrastructure.INotificationService;

namespace workers.console.runner
{

    public class ConsoleProgram
    {

        public static int Main(string[] args)
        {
            if (ParseArgExist("?"))
            {
                PrintHelp();
                return 0;
            }
            Console.WriteLine("Use /? for help");
            Console.WriteLine("(c) 2014 Bespoke Technology Sdn. Bhd.");
            Console.WriteLine();
            var host = ParseArg("h") ?? "localhost";
            var vhost = ParseArg("v") ?? "DevV1";
            var userName = ParseArg("u") ?? "guest";
            var password = ParseArg("p") ?? "guest";
            var debug = ParseArgExist("debug");
            if (debug)
            {
                Console.WriteLine("Press [ENTER] to continue");
                Console.ReadLine();
            }

            var port = ParseArg("port") == null ? 5672 : int.Parse(ParseArg("port"));

            var sw = new Stopwatch();
            sw.Start();
            Console.WriteLine("Stopwatch started");

            INotificationService log = new ConsoleNotification();
            if (ParseArg("log") != "console")
            {
                log = new EventLogNotification();
            }


            var title = string.Format("Connecting to {2}:{3}@{0}:{1}", host, port, userName, password);
            log.Write(Console.Title = title);
            var program = new Program
            {
                HostName = host,
                UserName = userName,
                Password = password,
                Port = port,
                NotificationService = log,
                VirtualHost = vhost
            };

            SubscriberMetadata[] metadata;
            using (var discoverer = new Isolated<Discoverer>())
            {
                metadata = discoverer.Value.Find();
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
            program.Start(metadata);

            var span = sw.Elapsed;
            sw.Stop();
            Console.WriteLine("{0} seconds taken to discover the subscribers", discoverElapsed.TotalSeconds);
            Console.WriteLine("{0} seconds taken to start the console", span.TotalSeconds);
            Console.WriteLine("Welcome to [SPH] Type ctrl + c to quit at any time.");

        
            stopFlag.WaitOne();
            return 0;
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

        private static void PrintHelp()
        {
            Console.WriteLine("Starts a RabbitMQ subscribers");
            Console.WriteLine("Use these command line parameter to specify your options");
            Console.WriteLine("     /h:<host name> the name of the RabbitMq host, or the IP address, the default is locahost");
            Console.WriteLine("     /v:<virtual host> the name of the virtual host on your RabbitMq Server, the default DevV1");
            Console.WriteLine("     /u:<user name> username used to connect to RabbitMq server, the default is guest");
            Console.WriteLine("     /p:<password> password used to connect to RabbitMq server, the default is guest");
            Console.WriteLine("     /port:<port number> the port number for connection to RabbitMq, the default is 5672");
            Console.WriteLine("     /i:<instance name> if you need to run multiple instance of console.worker then you need to give them diferrent instance name");
            Console.WriteLine("     /debug a switch to halt the loading so that you can attach a debugger ");
            Console.WriteLine("     /log:<log name> to output the log to console use /log:console, else it will use Windows event logs");
        }

    }



}
