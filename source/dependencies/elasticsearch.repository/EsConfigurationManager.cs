using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public static class EsConfigurationManager
    {

        public static long IndexingDelay => GetEnvironmentVariableInt32("ElasticsearchIndexingDelay", 15000);
        public static int IndexingMaxTry => GetEnvironmentVariableInt32("ElasticsearchIndexingMaxTry", 3);
        public static string SystemIndex => GetEnvironmentVariable("ElasticsearchIndex") ?? ConfigurationManager.ApplicationName.ToLowerInvariant() + "_sys";
        public static string Host => GetEnvironmentVariable("ElasticsearchHost") ?? "http://localhost:9200";
        public static string Index => GetEnvironmentVariable("ElasticsearchIndex") ?? ConfigurationManager.ApplicationName.ToLowerInvariant();
        public static string LogHost => GetEnvironmentVariable("ElasticsearchLogHost") ?? Host;
        public static string RequestLogIndexPattern => GetEnvironmentVariable("RequestLogIndexPattern") ?? "yyyyMMdd";

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
            var env = $"RX_{ConfigurationManager.ApplicationNameToUpper}_{setting}";
            var process = Environment.GetEnvironmentVariable(env, EnvironmentVariableTarget.Process);
            if (!string.IsNullOrWhiteSpace(process)) return process;

            var user = Environment.GetEnvironmentVariable(env, EnvironmentVariableTarget.User);
            if (!string.IsNullOrWhiteSpace(user)) return user;

            return Environment.GetEnvironmentVariable(env, EnvironmentVariableTarget.Machine);
        }
    }
}
