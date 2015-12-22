using System;
using Bespoke.Sph.Domain;
using System.Linq;
using System.ServiceProcess;
using Westwind.Windows.Services;

namespace workers.windowsservice.runner
{
    static class WorkerServiceProgram
    {
        [STAThread]
        static void Main(string[] args)
        {
            var help = args.ParseArgExist("?");
            if (help)
            {
                Console.WriteLine("To install use /i /v:<app-name>");
                Console.WriteLine("To uninstall use /u /v:<app-name>");
                return;
            }
            var appName = args.ParseArg("v");
            var serviceName = "RxServer-" + appName;


            if (args.ParseArgExist("install") || args.ParseArgExist("i"))
            {
                var sm = new WindowsServiceManager();
                if (string.IsNullOrWhiteSpace(appName))
                {
                    Console.WriteLine("To install use /i /v:<app-name>");
                    return;
                }
                var install = sm.InstallService(Environment.CurrentDirectory + "\\workers.windowsservice.runner.exe -service",
                    serviceName, "Reactive Developer Server - " + appName);
                Console.WriteLine(install ? "{0} service successfully installed...." : "{0} service install fail....", serviceName);

                return;
            }

            if (args.ParseArgExist("uninstall") || args.ParseArgExist("u"))
            {
                var sm = new WindowsServiceManager();
                Console.WriteLine(
                    !sm.UnInstallService(serviceName)
                        ? "{0} service successfully uninstalled. "
                        : "{0} service failed to uninstall. ", serviceName);

                return;
            }

            ConfigurationManager.AddConnectionString();
            var servicesToRun = new ServiceBase[]
                {
                    new SphWorkerService()
                };
            ServiceBase.Run(servicesToRun);
        }


        private static string ParseArg(this string[] args, string name)
        {
            args = args.Any() ? args : Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
            return val?.Replace("/" + name + ":", string.Empty);
        }

        private static bool ParseArgExist(this string[] args, string name)
        {
            args = args.Any() ? args : Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name));
            return null != val;
        }
    }
}
