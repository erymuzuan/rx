using System;
using System.Linq;
using Bespoke.Sph.SubscribersInfrastructure;
using INotificationService = Bespoke.Sph.SubscribersInfrastructure.INotificationService;

namespace workers.console.runner
{

    public class ConsoleProgram
    {

        public static int Main(string[] args)
        {

            var host = ParseArg("h") ?? "localhost";
            var vhost = ParseArg("v") ?? "Dev";
            var userName = ParseArg("u") ?? "guest";
            var password = ParseArg("p") ?? "guest";
            var debug = ParseArgExist("debug");
            if (debug)
            {
                Console.WriteLine("Press [ENTER] to continue");
                Console.ReadLine();
            }

            var port = ParseArg("port") == null ? 5672 : int.Parse(ParseArg("port"));

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

            Console.CancelKeyPress += (s, ce) =>
            {
                Console.WriteLine("Stop the workers [ENTER] +  y");
                while (!Console.KeyAvailable)
                {
                    
                }
                var cki = Console.ReadKey();
                if (cki.Key != ConsoleKey.Y)
                {
                    Console.WriteLine("----");
                    Console.WriteLine(cki.KeyChar);
                    Console.WriteLine(cki.Key);
                    ce.Cancel = true;
                    return;
                }
                Console.WriteLine("The workers is shutting down...");
                program.Stop();
            };

            program.Start(metadata);
            Console.WriteLine("Welcome to [SPH] Type ctrl + c to quit at any time.");
            return 0;
        }



        private static string ParseArg(string name)
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
