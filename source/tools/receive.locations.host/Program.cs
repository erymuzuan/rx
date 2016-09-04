using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Bespoke.Sph.Domain;
using Topshelf;

namespace receive.locations.host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var id = ParseArg("id") ?? args.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine(@"You will have to specify the name or id for the receive location to run");
                return;
            }
            var context = new SphDataContext();
            var ld = context.LoadOneFromSources<ReceiveLocation>(x => x.Id == id) ?? context.LoadOneFromSources<ReceiveLocation>(x => x.Name == id);
            if (null == ld)
            {
                Console.WriteLine($@"Cannot find any ReceveiLocation with name or id '{id}'");
                return;
            }
            var assemblyFile = ConfigurationManager.CompilerOutputPath + "\\" + ld.AssemblyName;
            if (!File.Exists(assemblyFile))
            {
                Console.WriteLine($@"You have to compile '{id}'");
                return;

            }
            var type = Assembly.LoadFrom(assemblyFile).GetType($"{ld.CodeNamespace}.{ld.TypeName}");
            dynamic loc = Activator.CreateInstance(type);
            loc.Start();
            QUIT:
            Console.WriteLine(@"To quit press [Enter]");
            Console.ReadLine();
            Console.WriteLine(@"Are you sure [y/n]");
            var answer = Console.ReadLine();
            if (answer == "y")
                loc.Dispose();
            else
                goto QUIT;
            //HostFactory.Run(config =>
            //{
            //    config.Service<IReceiveLocation>(svc =>
            //    {
            //        svc.ConstructUsing(() =>
            //        {
            //            dynamic loc = Activator.CreateInstance(type);
            //            return (IReceiveLocation)loc;
            //        });

            //        svc.WhenStarted(x => x.Start());
            //        svc.WhenStopped(x => x.Stop());
            //        svc.WhenPaused(x => x.Pause());
            //        svc.WhenShutdown(x => x.Stop());
            //        svc.WhenCustomCommandReceived((x, g, i) =>
            //        {
            //            Console.WriteLine(g);
            //            Console.WriteLine(i);
            //        });
            //    });
            //    config.SetServiceName("RxLocation" + ld.Name);
            //    config.SetDisplayName("Rx Receive Location " + ld.Name);
            //    config.SetDescription($"Rx Developer receive location for {ld.GetType().Name}");

            //    config.StartAutomatically();
            //});
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
    }
}
