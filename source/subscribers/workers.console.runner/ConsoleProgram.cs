using System;
using System.Linq;
using Bespoke.Sph.SubscribersInfrastructure;

namespace workers.console.runner
{
    
    public class ConsoleProgram
    {

        public static int Main(string[] args)
        {
            var host = ParseArg("h") ?? "localhost";
            var vhost = ParseArg("v") ?? "sph.0009";
            var username = ParseArg("u") ?? "guest";
            var password = ParseArg("p") ?? "guest";
            var silent = ParseArgExist("quiet");

            var port = ParseArg("port") == null ? 5672 : int.Parse(ParseArg("port"));

            INotificationService log = new ConsoleNotification();
            if (ParseArg("log") != "console")
            {
                log = new EventLogNotification();
            }

            var title = string.Format("Connecting to {2}:{3}@{0}:{1}", host, port, username, password);
            log.Write(Console.Title = title);
            var program = new Program
                {
                    HostName = host,
                    UserName = username,
                    Password = password,
                    Port = port,
                    NotificationService = log,
                    VirtualHost = vhost
                };

            var discoverer = new Discoverer();
            program.SubscribersMetadata = discoverer.Find();
            program.Subscribers = discoverer.FindSubscriber();
            program.SubscribersMetadata.Select(d => d.FullName).ToList().ForEach(Console.WriteLine);

         
            program.Start();
            bool quit = false;
            Console.WriteLine("Welcome to [SPH] Type quit to quit at any time.");
            while (!quit)
            {
                string code = Console.ReadLine();
                quit = string.Format("{0}", code).ToLower() == "quit";
                if (quit)
                {
                    break;
                }
            }
        

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
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name ));
            return null != val;
        }

    }
}
