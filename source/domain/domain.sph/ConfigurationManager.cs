using System;

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

        public static string WorkflowCompilerOutputPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["sph:WorkflowCompilerOutputPath"] ?? AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static string WorkflowSourceDirectory
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["sph:WorkflowSourceDirectory"] ??
                       string.Empty;
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
                return System.Configuration.ConfigurationManager.AppSettings["sph:ReportDeliveryExecutable"];
            }
        }

        public static string ScheduledTriggerActivityExecutable
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["sph:ScheduledTriggerActivityExecutable"];
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

        public static int JpegMaxWitdh
        {
            get
            {
                var width = System.Configuration.ConfigurationManager.AppSettings["jpg.max.width"] ?? "400";
                return int.Parse(width);
            }
        }

        public static string GetSchedulerPath(string bin)
        {

            return System.Configuration.ConfigurationManager.AppSettings["sph:SchedulerPath"] ?? System.IO.Path.Combine(bin, @"../../../../bin/schedulers");


        }
        public static string GetSubscriberPath(string bin)
        {

            return System.Configuration.ConfigurationManager.AppSettings["sph:SubscriberPath"] ?? System.IO.Path.Combine(bin, @"../../../../bin/subscribers");


        }
    }
}
