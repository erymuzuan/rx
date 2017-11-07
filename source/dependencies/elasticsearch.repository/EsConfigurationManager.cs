using System;
using System.IO;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public static class EsConfigurationManager
    {

        public static long EsIndexingDelay => GetEnvironmentVariableInt32("EsIndexingDelay", 15000);
        public static int EsIndexingMaxTry => GetEnvironmentVariableInt32("EsIndexingMaxTry", 3);
        public static string ElasticSearchSystemIndex => GetEnvironmentVariable("ElasticsearchIndex") ?? ConfigurationManager.ApplicationName.ToLowerInvariant() + "_sys";
        public static string ElasticSearchHost => GetEnvironmentVariable("ElasticSearchHost") ?? "http://localhost:9200";
        public static string ElasticSearchIndex => GetEnvironmentVariable("ElasticSearchIndex") ?? ConfigurationManager.ApplicationName.ToLowerInvariant();
        public static string ElasticsearchLogHost => GetEnvironmentVariable("ElasticsearchLogHost") ?? ElasticSearchHost;
        public static string RequestLogIndexPattern => GetEnvironmentVariable("RequestLogIndexPattern") ?? "yyyyMMdd";

        private static string GetPath(string setting, string defaultPath)
        {
            var val = GetEnvironmentVariable(setting);
            if (Path.IsPathRooted(val)) return val;
            return (ConfigurationManager.Home + @"\" + defaultPath).Replace("\\\\", "\\");
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
            var process = Environment.GetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationNameToUpper}_{setting}", EnvironmentVariableTarget.Process);
            if (!string.IsNullOrWhiteSpace(process)) return process;

            var user = Environment.GetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationNameToUpper}_{setting}", EnvironmentVariableTarget.User);
            if (!string.IsNullOrWhiteSpace(user)) return user;

            return Environment.GetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationNameToUpper}_{setting}", EnvironmentVariableTarget.Machine);
        }
    }
}
