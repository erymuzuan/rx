using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Messaging.AzureMessaging
{
    public static class AzureServiceBusConfigurationManager
    {

        public static string PrimaryAccessKey => GetEnvironmentVariable("AzureServiceBusPrimaryAccessKey");
        public static string SecondaryAccessKey => GetEnvironmentVariable("AzureServiceBusSecondaryAccessKey");
        public static string PrimaryConnectionString => GetEnvironmentVariable("AzureServiceBusPrimaryConnectionString");
        public static string SecondaryConnectionString => GetEnvironmentVariable("AzureServiceBusSecondaryConnectionString");
        public static string DefaultTopicPath => GetEnvironmentVariable("AzureServiceBusDefaultTopicPath") ?? "rx.topics";

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
