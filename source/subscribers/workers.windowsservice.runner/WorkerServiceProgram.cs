using System.ServiceProcess;

namespace workers.windowsservice.runner
{
    static class WorkerServiceProgram
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[] 
                { 
                    new StationMsWorkerService() 
                };
            ServiceBase.Run(servicesToRun);
        }
    }
}
