using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace Westwind.Windows.Services
{
    /// <summary>
    /// This class handles installation of a Windows Service as well as providing
    /// the ability to start stop and detect the state of a Windows Service.
    /// Utilizes P/Invoke calls to install the service.
    /// </summary>
    public class WindowsServiceManager
    {

        #region DLLImport

        // ReSharper disable InconsistentNaming
        [DllImport("advapi32.dll")]
        public static extern IntPtr OpenSCManager(string lpMachineName, string lpSCDB, int scParameter);
        [DllImport("Advapi32.dll")]
        public static extern IntPtr CreateService(IntPtr SC_HANDLE, string lpSvcName, string lpDisplayName,
            int dwDesiredAccess, int dwServiceType, int dwStartType, int dwErrorControl, string lpPathName,
            string lpLoadOrderGroup, int lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);
        [DllImport("advapi32.dll")]
        public static extern void CloseServiceHandle(IntPtr SCHANDLE);
        [DllImport("advapi32.dll")]
        public static extern int StartService(IntPtr SVHANDLE, int dwNumServiceArgs, string lpServiceArgVectors);


        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern IntPtr OpenService(IntPtr SCHANDLE, string lpSvcName, int dwNumServiceArgs);
        [DllImport("advapi32.dll")]
        public static extern int DeleteService(IntPtr SVHANDLE);

        [DllImport("kernel32.dll")]
        public static extern int GetLastError();
        // ReSharper restore InconsistentNaming
        #endregion DLLImport

        /// <summary>
        /// This method installs and runs the service in the service conrol manager.
        /// </summary>
        /// <param name="svcPath">The complete path of the service.</param>
        /// <param name="svcName">Name of the service.</param>
        /// <param name="svcDispName">Display name of the service.</param>
        /// <returns>True if the process went thro successfully. False if there was any error.</returns>
        public bool InstallService(string svcPath, string svcName, string svcDispName)
        {
            #region Constants declaration.
            // ReSharper disable InconsistentNaming
            const int SC_MANAGER_CREATE_SERVICE = 0x0002;
            const int SERVICE_WIN32_OWN_PROCESS = 0x00000010;
            //int SERVICE_DEMAND_START = 0x00000003;
            const int SERVICE_ERROR_NORMAL = 0x00000001;

            const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
            const int SERVICE_QUERY_CONFIG = 0x0001;
            const int SERVICE_CHANGE_CONFIG = 0x0002;
            const int SERVICE_QUERY_STATUS = 0x0004;
            const int SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
            const int SERVICE_START = 0x0010;
            const int SERVICE_STOP = 0x0020;
            const int SERVICE_PAUSE_CONTINUE = 0x0040;
            const int SERVICE_INTERROGATE = 0x0080;
            const int SERVICE_USER_DEFINED_CONTROL = 0x0100;

            const int SERVICE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED |
                                            SERVICE_QUERY_CONFIG |
                                            SERVICE_CHANGE_CONFIG |
                                            SERVICE_QUERY_STATUS |
                                            SERVICE_ENUMERATE_DEPENDENTS |
                                            SERVICE_START |
                                            SERVICE_STOP |
                                            SERVICE_PAUSE_CONTINUE |
                                            SERVICE_INTERROGATE |
                                            SERVICE_USER_DEFINED_CONTROL);

            const int SERVICE_AUTO_START = 0x00000002;
            // ReSharper restore InconsistentNaming
            #endregion Constants declaration.

            try
            {
                var scHandle = OpenSCManager(null, null, SC_MANAGER_CREATE_SERVICE);

                if (scHandle.ToInt32() != 0)
                {
                    IntPtr svHandle = CreateService(scHandle, svcName, svcDispName, SERVICE_ALL_ACCESS,
                                                     SERVICE_WIN32_OWN_PROCESS, SERVICE_AUTO_START,
                                                     SERVICE_ERROR_NORMAL, svcPath, null, 0, null, null, null);

                    if (svHandle.ToInt32() == 0)
                    {

                        CloseServiceHandle(scHandle);
                        return false;
                    }
                    //now trying to start the service
                    int i = StartService(svHandle, 0, null);
                    // If the value i is zero, then there was an error starting the service.
                    // note: error may arise if the service is already running or some other problem.
                    if (i == 0)
                    {
                        //Console.WriteLine("Couldnt start service");
                        return false;
                    }
                    //Console.WriteLine("Success");
                    CloseServiceHandle(scHandle);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        /// <summary>
        /// This method uninstalls the service from the service conrol manager.
        /// </summary>
        /// <param name="serviceName">Name of the service to uninstall.</param>
        public bool UnInstallService(string serviceName)
        {
            const int GENERIC_WRITE = 0x40000000;
            var scHndl = OpenSCManager(null, null, GENERIC_WRITE);

            if (scHndl.ToInt32() == 0) return false;
            const int DELETE = 0x10000;
            var svcHndl = OpenService(scHndl, serviceName, DELETE);
            //Console.WriteLine(svc_hndl.ToInt32());
            if (svcHndl.ToInt32() == 0) return false;
            var i = DeleteService(svcHndl);
            if (i != 0)
            {
                CloseServiceHandle(scHndl);
                return true;
            }
            CloseServiceHandle(scHndl);
            return false;
        }

        /// <summary>
        /// Determines whether a service exisits. Pass in the Service Name
        /// either by the ServiceName or the descriptive name
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public bool IsServiceInstalled(string serviceName)
        {
            serviceName = serviceName.ToLowerInvariant();
            var services = ServiceController.GetServices();
            return services.Any(GetServiceByName(serviceName));

        }

        public bool IsServiceRunning(string serviceName)
        {
            serviceName = serviceName.ToLowerInvariant();
            var services = ServiceController.GetServices();
            return services
                .Where(s => s.Status == ServiceControllerStatus.Running)
                .Any(GetServiceByName(serviceName));

        }

        private static Func<ServiceController, bool> GetServiceByName(string serviceName)
        {
            return s => s.ServiceName.ToLowerInvariant() == serviceName
                        || s.DisplayName.ToLowerInvariant() == serviceName;
        }

        /// <summary>
        /// Starts a service by name or descriptive name
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public bool StartService(string serviceName)
        {
            serviceName = serviceName.ToLower();

            var services = ServiceController.GetServices();
            foreach (var svc in services)
            {

                if (svc.ServiceName.ToLower() == serviceName ||
                    svc.DisplayName.ToLower() == serviceName)
                {
                    if (svc.Status == ServiceControllerStatus.Running)
                        return true;

                    svc.Start();
                    svc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                    return svc.Status == ServiceControllerStatus.Running;
                }
            }
            return false;
        }

        /// <summary>
        /// Stops a service by name or descriptive name
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public bool StopService(string serviceName)
        {
            serviceName = serviceName.ToLower();

            var services = ServiceController.GetServices();
            foreach (var svc in services)
            {
                if (svc.ServiceName.ToLower() == serviceName ||
                    svc.DisplayName.ToLower() == serviceName)
                {
                    if (svc.Status == ServiceControllerStatus.Stopped ||
                        svc.Status == ServiceControllerStatus.Paused)
                        return true;

                    svc.Stop();
                    svc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));
                    return svc.Status == ServiceControllerStatus.Stopped;
                }
            }
            return false;
        }

        /// <summary>
        /// Pauses a service by name or descriptive name
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public bool PauseService(string serviceName)
        {
            serviceName = serviceName.ToLower();

            var services = ServiceController.GetServices();
            foreach (var svc in services)
            {
                if (svc.ServiceName.ToLower() == serviceName ||
                    svc.DisplayName.ToLower() == serviceName)
                {
                    if (svc.Status == ServiceControllerStatus.Stopped ||
                        svc.Status == ServiceControllerStatus.Paused)
                        return true;

                    svc.Pause();

                    svc.WaitForStatus(ServiceControllerStatus.Paused, TimeSpan.FromSeconds(15));
                    return svc.Status == ServiceControllerStatus.Paused;
                }
            }
            return false;
        }

        /// <summary>
        /// Pauses a service by name or descriptive name
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public bool ContinueService(string serviceName)
        {
            serviceName = serviceName.ToLower();

            var services = ServiceController.GetServices();
            foreach (var svc in services)
            {
                if (svc.ServiceName.ToLower() == serviceName ||
                    svc.DisplayName.ToLower() == serviceName)
                {
                    if (svc.Status == ServiceControllerStatus.Paused)
                        svc.Continue();
                    else if (svc.Status == ServiceControllerStatus.Stopped)
                        svc.Start();

                    svc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));

                    return svc.Status == ServiceControllerStatus.Running;
                }
            }

            return false;
        }


    }
}
