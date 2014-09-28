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

        public static long SqlPersistenceDelay
        {
            get
            {
                var pn = System.Configuration.ConfigurationManager.AppSettings["sph:SqlPersistenceDelay"] ?? "1000";
                return long.Parse(pn);
            }
        }

        public static int SqlPersistenceMaxTry
        {
            get
            {
                var pn = System.Configuration.ConfigurationManager.AppSettings["sph:SqlPersistenceMaxTry"] ?? "3";
                return int.Parse(pn);
            }
        }

        public static bool EnableOfflineForm
        {
            get
            {
                var pn = System.Configuration.ConfigurationManager.AppSettings["sph:EnableOfflineForm"] ?? "false";
                return bool.Parse(pn);
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
                var val = System.Configuration.ConfigurationManager.AppSettings["sph:WorkflowCompilerOutputPath"];
                if (Path.IsPathRooted(val)) return val;
                return BaseDirectory + BinPath + @"\output";
            }
        }

        /// <summary>
        /// Ad directory where all the sph and systems source code like the *.json file for each asset definitions
        /// </summary>
        public static string SphSourceDirectory
        {
            get
            {
                var val = System.Configuration.ConfigurationManager.AppSettings["sph:SphSourceDirectory"];
                if (Path.IsPathRooted(val)) return val;
                return BaseDirectory + BinPath + @"\sources\";
            }
        }
        /// <summary>
        /// A directory where all the users source codes are
        /// </summary>
        public static string UserSourceDirectory
        {
            get
            {
                var val = System.Configuration.ConfigurationManager.AppSettings["sph:UserSourceDirectory"];
                if (Path.IsPathRooted(val)) return val;
                return BaseDirectory + BinPath + @"\sources\";
            }
        }

        public static string ElasticSearchHost
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["sph:ElasticSearchHost"] ?? "http://localhost:9200";
            }
        }

        public static string ElasticSearchIndex
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["sph:ElasticSearchIndex"]
                    ?? ApplicationName.ToLowerInvariant() + "_sys";
            }
        }

        public static string BinPath
        {
            get
            {
                if (ApplicationName == "Dev")
                    return @"\bin";
                return string.Empty;
            }
        }

        public static string ReportDeliveryExecutable
        {
            get
            {
                var val = System.Configuration.ConfigurationManager.AppSettings["sph:ReportDeliveryExecutable"];
                if (Path.IsPathRooted(val)) return val;
                return BaseDirectory + BinPath + @"\schedulers\scheduler.report.delivery.exe";
            }
        }

        public static string ScheduledTriggerActivityExecutable
        {
            get
            {
                var val =
                    System.Configuration.ConfigurationManager.AppSettings["sph:ScheduledTriggerActivityExecutable"];
                if (Path.IsPathRooted(val)) return val;
                return BaseDirectory + BinPath + @"\schedulers\scheduler.workflow.trigger.exe";
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

                var val = System.Configuration.ConfigurationManager.AppSettings["sph:SchedulerPath"];
                if (Path.IsPathRooted(val)) return val;
                return BaseDirectory + BinPath + @"\schedulers";

            }

        }
        public static string SubscriberPath
        {
            get
            {
                var val = System.Configuration.ConfigurationManager.AppSettings["sph:SubscriberPath"];
                if (Path.IsPathRooted(val)) return val;
                return BaseDirectory + BinPath + @"\subscribers";
            }
        }
        public static string ToolsPath
        {
            get
            {
                var val = System.Configuration.ConfigurationManager.AppSettings["sph:ToolsPath"];
                if (Path.IsPathRooted(val)) return val;
                return BaseDirectory + BinPath + @"\tools";
            }
        }
        public static string WebPath
        {
            get
            {

                var val = System.Configuration.ConfigurationManager.AppSettings["sph:WebPath"];
                if (Path.IsPathRooted(val)) return val;

                if (ApplicationName == "Dev")
                    return BaseDirectory  + @"\source\web\web.sph";

                return BaseDirectory + BinPath + @"\web";
            }
        }

        public static string DelayActivityExecutable
        {
            get
            {
                var val = System.Configuration.ConfigurationManager.AppSettings["sph:DelayActivityExecutable"];
                if (Path.IsPathRooted(val)) return val;
                return BaseDirectory + BinPath + @"\schedulers\scheduler.delayactivity.exe";

            }
        }

        public static string ApplicationName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["sph:ApplicationName"] ?? "YOUR_APP";
            }
        }

        public static string ApplicationFullName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["sph:ApplicationFullName"] ?? "Reactive Developer platform showcase";
            }
        }
    }
}
