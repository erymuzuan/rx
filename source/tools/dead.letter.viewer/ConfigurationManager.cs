﻿using System;
using System.IO;

namespace Bespoke.Station.Windows.RabbitMqDeadLetter
{
    public static class ConfigurationManager
    {
        public static string ApplicationNameToUpper = ApplicationName.ToUpper();
        public static string CompanyName => AppSettings["sph:CompanyName"] ?? (GetEnvironmentVariable("CompanyName") ?? "Bespoke");
        public static string ApplicationName => AppSettings["sph:ApplicationName"] ?? "DevV1";

        public static string TokenSecret => GetEnvironmentVariable("TokenSecret") ?? $"{ApplicationName}{Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Home))}";
        public static string ApplicationFullName => GetEnvironmentVariable("ApplicationFullName") ?? "Reactive Developer platform showcase";
        public static string FromEmailAddress => GetEnvironmentVariable("FromEmailAddress") ?? "admin@rxdeveloper.com";
        public static int SourceDataCache => GetEnvironmentVariableInt32("SourceDataCache", 300);
        public static int StaticFileCache => GetEnvironmentVariableInt32("StaticFileCache", 120);
        public static int WorkflowDebuggerPort => GetEnvironmentVariableInt32("WorkflowDebuggerPort", 50518);
        public static long SqlPersistenceDelay => GetEnvironmentVariableInt32("SqlPersistenceDelay", 15000);
        public static int SqlPersistenceMaxTry => GetEnvironmentVariableInt32("SqlPersistenceMaxTry", 3);
        public static bool EnableOfflineForm => GetEnvironmentVariableBoolean("EnableOfflineForm");
        public static string BaseUrl => GetEnvironmentVariable("BaseUrl") ?? "http://localhost:4436";
        public static string Home => GetEnvironmentVariable("HOME");
        public static string CompilerOutputPath => GetPath("CompilerOutputPath", "output");
        /// <summary>
        /// Ad directory where all the sph and systems source code like the *.json file for each asset definitions
        /// </summary>
        public static string SphSourceDirectory => GetPath("SourceDirectory", "sources");
        /// <summary>
        /// A directory where all the users source codes are
        /// </summary>
        public static string GeneratedSourceDirectory => GetPath("GeneratedSourceDirectory", @"sources\_generated\");
        public static string SqlConnectionString => GetEnvironmentVariable("SqlConnectionString") ?? $"Data Source=(localdb)\\Projects;Initial Catalog={ApplicationName};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";

        public static string RabbitMqUserName => GetEnvironmentVariable("RabbitMqUserName") ?? "guest";
        public static string RabbitMqPassword => GetEnvironmentVariable("RabbitMqPassword") ?? "guest";
        public static string RabbitMqHost => GetEnvironmentVariable("RabbitMqHost") ?? "localhost";
        public static string RabbitMqManagementScheme => GetEnvironmentVariable("RabbitMqManagementScheme") ?? "http";
        public static int RabbitMqPort => GetEnvironmentVariableInt32("RabbitMqPort", 5672);
        public static int RabbitMqManagementPort => GetEnvironmentVariableInt32("RabbitMqManagementPort", 15672);
        public static string RabbitMqVirtualHost => GetEnvironmentVariable("RabbitMqVirtualHost") ?? ApplicationName;

        public static long EsIndexingDelay => GetEnvironmentVariableInt32("EsIndexingDelay", 15000);
        public static int EsIndexingMaxTry => GetEnvironmentVariableInt32("EsIndexingMaxTry", 3);
        public static string ElasticSearchSystemIndex => GetEnvironmentVariable("ElasticsearchIndex") ?? ApplicationName.ToLowerInvariant() + "_sys";
        public static string ElasticSearchHost => GetEnvironmentVariable("ElasticSearchHost") ?? "http://localhost:9200";
        public static string ElasticSearchIndex => GetEnvironmentVariable("ElasticSearchIndex") ?? ApplicationName.ToLowerInvariant();
        public static string ElasticsearchLogHost => GetEnvironmentVariable("ElasticsearchLogHost") ?? ElasticSearchHost;
        public static string RequestLogIndexPattern => GetEnvironmentVariable("RequestLogIndexPattern") ?? "yyyyMMdd";

        public static string ReportDeliveryExecutable => GetPath("ReportDeliveryExecutable", @"schedulers\scheduler.report.delivery.exe");
        public static string ScheduledDataTransferExecutable => GetPath("ScheduledDataTransferExecutable", @"schedulers\scheduler.data.import.exe");
        public static string ScheduledTriggerActivityExecutable => GetPath("ScheduledTriggerActivityExecutable", @"schedulers\scheduler.workflow.trigger.exe");
        public static bool EnableWorkflowGetCacheDependency => GetEnvironmentVariableBoolean("EnableWorkflowGetCacheDependency");


        public static System.Collections.Specialized.NameValueCollection AppSettings => System.Configuration.ConfigurationManager.AppSettings;
        public static int JpegMaxWitdh => GetEnvironmentVariableInt32("jpg.max.width", 400);

        public static string SchedulerPath => GetPath("SchedulerPath", "schedulers");
        public static string SubscriberPath => GetPath("SubscriberPath", "subscribers");
        public static string ToolsPath => GetPath("ToolsPath", "tools");
        public static string WebPath => GetPath("WebPath", "web");
        public static string DelayActivityExecutable => GetPath("DelayActivityExecutable", @"schedulers\scheduler.delayactivity.exe");

        private static string GetPath(string setting, string defaultPath)
        {
            var val = GetEnvironmentVariable(setting);
            if (Path.IsPathRooted(val)) return val;
            return (Home + @"\" + defaultPath).Replace("\\\\", "\\");
        }
        public static int GetEnvironmentVariableInt32(string setting, int defaultValue = 0)
        {
            var val = GetEnvironmentVariable(setting);
            int intValue;
            return int.TryParse(val, out intValue) ? intValue : defaultValue;
        }
        public static bool GetEnvironmentVariableBoolean(string setting, bool defaultValue = false)
        {
            var val = GetEnvironmentVariable(setting);
            bool intValue;
            return bool.TryParse(val, out intValue) ? intValue : defaultValue;
        }
        public static string GetEnvironmentVariable(string setting)
        {
            var process = Environment.GetEnvironmentVariable($"RX_{ApplicationNameToUpper}_{setting}", EnvironmentVariableTarget.Process);
            if (!string.IsNullOrWhiteSpace(process)) return process;

            var user = Environment.GetEnvironmentVariable($"RX_{ApplicationNameToUpper}_{setting}", EnvironmentVariableTarget.User);
            if (!string.IsNullOrWhiteSpace(user)) return user;

            return Environment.GetEnvironmentVariable($"RX_{ApplicationNameToUpper}_{setting}", EnvironmentVariableTarget.Machine);
        }



    }
}
