using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Messaging.RabbitMqMessagings
{
    public static class RabbitMqConfigurationManager
    {

        public static string UserName => GetEnvironmentVariable("RabbitMqUserName") ?? "guest";
        public static string Password => GetEnvironmentVariable("RabbitMqPassword") ?? "guest";
        public static string Host => GetEnvironmentVariable("RabbitMqHost") ?? "localhost";
        public static string ManagementScheme => GetEnvironmentVariable("RabbitMqManagementScheme") ?? "http";
        public static int Port => GetEnvironmentVariableInt32("RabbitMqPort", 5672);
        public static int ManagementPort => GetEnvironmentVariableInt32("RabbitMqManagementPort", 15672);
        public static string VirtualHost => GetEnvironmentVariable("RabbitMqVirtualHost") ?? ConfigurationManager.ApplicationName;

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
            var applicationNameToUpper = ConfigurationManager.ApplicationNameToUpper;
            var process = Environment.GetEnvironmentVariable($"RX_{applicationNameToUpper}_{setting}", EnvironmentVariableTarget.Process);
            if (!string.IsNullOrWhiteSpace(process)) return process;

            var user = Environment.GetEnvironmentVariable($"RX_{applicationNameToUpper}_{setting}", EnvironmentVariableTarget.User);
            if (!string.IsNullOrWhiteSpace(user)) return user;

            return Environment.GetEnvironmentVariable($"RX_{applicationNameToUpper}_{setting}", EnvironmentVariableTarget.Machine);
        }

    }
}
