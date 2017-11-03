using System;
using System.IO;

namespace Bespoke.Sph.RxPs.Domain
{
    public static class ConfigurationManager
    {
        public static string RxApplicationName { get; private set; }
        public static string ApplicationNameToUpper { get; private set; }
        public static string SphSourceDirectory => GetPath("SourceDirectory", "sources");
        public static string Home => GetEnvironmentVariable("HOME");


        public static string SchedulerPath => GetPath("SchedulerPath", "schedulers");
        public static string SubscriberPath => GetPath("SubscriberPath", "subscribers");
        public static string ToolsPath => GetPath("ToolsPath", "tools");
        public static string WebPath => GetPath("WebPath", "web");
        public static string CompilerOutputPath => GetPath("CompilerOutputPath", "output");


        public static string RabbitMqUserName => GetEnvironmentVariable("RabbitMqUserName") ?? "guest";
        public static string RabbitMqPassword => GetEnvironmentVariable("RabbitMqPassword") ?? "guest";
        public static string RabbitMqHost => GetEnvironmentVariable("RabbitMqHost") ?? "localhost";
        public static string RabbitMqManagementScheme => GetEnvironmentVariable("RabbitMqManagementScheme") ?? "http";
        public static int RabbitMqPort => GetEnvironmentVariableInt32("RabbitMqPort", 5672);
        public static int RabbitMqManagementPort => GetEnvironmentVariableInt32("RabbitMqManagementPort", 15672);
        public static string RabbitMqVirtualHost => GetEnvironmentVariable("RabbitMqVirtualHost") ?? RxApplicationName;


        public static void Initialize(string app)
        {
            RxApplicationName = app;
            ApplicationNameToUpper = app.ToUpperInvariant();
        }


        private static string GetPath(string setting, string defaultPath)
        {
            var val = GetEnvironmentVariable(setting);
            if (Path.IsPathRooted(val)) return val;
            return (Home + @"\" + defaultPath).Replace("\\\\", "\\");
        }
        public static int GetEnvironmentVariableInt32(string setting, int defaultValue = 0)
        {
            var val = GetEnvironmentVariable(setting);
            return int.TryParse(val, out var intValue) ? intValue : defaultValue;
        }
        public static bool GetEnvironmentVariableBoolean(string setting, bool defaultValue = false)
        {
            var val = GetEnvironmentVariable(setting);
            return bool.TryParse(val, out var intValue) ? intValue : defaultValue;
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
