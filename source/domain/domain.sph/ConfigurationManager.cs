using System.IO;

namespace Bespoke.Sph.Domain
{
    public static class ConfigurationManager
    {
        public static int WorkflowDebuggerPort
        {
            get
            {
                var pn = System.Configuration.ConfigurationManager.AppSettings["sph:WorkflowDebuggerPort"] ?? "50518";
                return int.Parse(pn);
            }
        }

        public static string BaseUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["sph:BaseUrl"] ?? "http://localhost:4436";

            }
        }
        public static string BaseDirectory
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["sph:BaseDirectory"] ?? @"c:\project\work\sph";

            }
        }

        public static string WorkflowCompilerOutputPath
        {
            get
            {
                var val = System.Configuration.ConfigurationManager.AppSettings["sph:WorkflowCompilerOutputPath"]
                    ?? @"\bin\output";
                return BaseDirectory + val;
            }
        }

        public static string WorkflowSourceDirectory
        {
            get
            {
                var val = System.Configuration.ConfigurationManager.AppSettings["sph:WorkflowSourceDirectory"] ??
                       @"\bin\sources\";
                return BaseDirectory + val;
            }
        }

        public static string ElasticSearchHost
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["sph:ElasticSearchHost"] ?? "http://localhost:9200/sph/";
            }
        }

        public static string ReportDeliveryExecutable
        {
            get
            {
                var val = System.Configuration.ConfigurationManager.AppSettings["sph:ReportDeliveryExecutable"]
                    ?? @"\bin\schedulers\scheduler.report.delivery.exe";
                return Path.Combine(BaseDirectory, val);
            }
        }

        public static string ScheduledTriggerActivityExecutable
        {
            get
            {
                var val = System.Configuration.ConfigurationManager.AppSettings["sph:ScheduledTriggerActivityExecutable"]
                    ?? @"\bin\schedulers\scheduler.workflow.trigger.exe";
                return Path.Combine(BaseDirectory, val);
            }
        }

        public static bool EnableWorkflowGetCacheDependency
        {
            get
            {
                var enabled = System.Configuration.ConfigurationManager.AppSettings["sph:EnableWorkflowGetCacheDependency"] ?? "false";
                return bool.Parse(enabled);
            }
        }

        public static System.Configuration.ConnectionStringSettingsCollection ConnectionStrings
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings;
            }
        }

        public static System.Collections.Specialized.NameValueCollection AppSettings
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings;
            }
        }

        public static int JpegMaxWitdh
        {
            get
            {
                var width = System.Configuration.ConfigurationManager.AppSettings["jpg.max.width"] ?? "400";
                return int.Parse(width);
            }
        }

        public static string SchedulerPath
        {
            get
            {

                var val = System.Configuration.ConfigurationManager.AppSettings["sph:SchedulerPath"] ?? @"\bin\schedulers";
                return BaseDirectory + val;
            }

        }
        public static string SubscriberPath
        {
            get
            {

                var val = System.Configuration.ConfigurationManager.AppSettings["sph:SubscriberPath"] ?? @"\bin\subscribers";
                return BaseDirectory + val;
            }
        }
        public static string WebPath
        {
            get
            {

                var val = System.Configuration.ConfigurationManager.AppSettings["sph:SubscriberPath"] ?? @"\source\web\web.sph";
                return BaseDirectory + val;
            }
        }

        public static string DelayActivityExecutable
        {
            get
            {
                var val = System.Configuration.ConfigurationManager.AppSettings["sph:DelayActivityExecutable"] ?? @"\bin\schedulers\scheduler.delayactivity.exe";
                return BaseDirectory + val;


            }
        }
    }
}
