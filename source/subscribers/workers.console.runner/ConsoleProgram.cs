using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RabbitMqPublisher;
using Bespoke.Sph.SubscribersInfrastructure;
using INotificationService = Bespoke.Sph.SubscribersInfrastructure.INotificationService;

namespace workers.console.runner
{

    public class ConsoleProgram
    {

        public static int Main(string[] args)
        {
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

            INotificationService log = new ConsoleNotification(ObjectBuilder.GetObject<IBrokerConnection>());
            if (ParseArg("log") != "console")
            {
                log = new EventLogNotification();
            }

            // start web.console
            var webConsole = new ConsoleNotificationSubscriber
            {
                HostName = host,
                UserName = userName,
                Password = password,
                Port = port,
                VirtualHost = vhost
            };
            webConsole.Run();

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
            var fsw = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory) { EnableRaisingEvents = true };
            fsw.Changed += (o, e) =>
            {
                if (e.Name != "q.txt") return;
                Console.WriteLine("The workers is shutting down...!!!");
                program.Stop();
                stopFlag.Set();
                fsw.Dispose();
            };
            var discoverElapsed = sw.Elapsed;
            program.Start(metadata);
            Console.WriteLine("********* Wathching " + AppDomain.CurrentDomain.BaseDirectory);

            var span = sw.Elapsed;
            sw.Stop();
            Console.WriteLine("{0} seconds taken to start discover the subscribers", discoverElapsed.TotalSeconds);
            Console.WriteLine("{0} seconds taken to start the console", span.TotalSeconds);
            Console.WriteLine("Welcome to [SPH] Type ctrl + c to quit at any time.");

            stopFlag.WaitOne();
            return 0;
        }


        public static string ParseArg(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
            if (null == val) return null;
            return val.Replace("/" + name + ":", string.Empty);
        }

        private static bool ParseArgExist(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name));
            return null != val;
        }

    }
}
