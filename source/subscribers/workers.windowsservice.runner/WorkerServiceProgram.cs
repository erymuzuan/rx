using System;
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
            var appName = args.ParseArg("v");
            var serviceName = "RxServer-" + appName;

            if (args.ParseArgExist("install") || args.ParseArgExist("i"))
            {
                var sm = new WindowsServiceManager();
                var install = sm.InstallService(Environment.CurrentDirectory + "\\workers.windowsservice.runner.exe -service",
                    serviceName, "Reactive Developer Server - " + appName);
                if (!install)
                    Console.WriteLine("{0} service install fail....", serviceName);

                return;
            }

            if (args.ParseArgExist("uninstall") || args.ParseArgExist("u"))
            {
                var sm = new WindowsServiceManager();
                if (!sm.UnInstallService(serviceName))
                    Console.WriteLine("{0} service failed to uninstall. ",serviceName);

                return;
            }
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
            if (null == val) return null;
            return val.Replace("/" + name + ":", string.Empty);
        }

        private static bool ParseArgExist(this string[] args, string name)
        {
            args = args.Any() ? args : Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name));
            return null != val;
        }
    }
}
