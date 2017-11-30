using System;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.Extensions
{

    public static class TestOutputHelperExtension
    {
        public static string OneLine(this string value)
        {
            return value.Replace("\r\n", " ").Replace("  ", " ");
        }
        public static T WriteObject<T>(this T value, ITestOutputHelper console)
        {
            console.WriteLine(value);
            return value;
        }
        public static void WriteLine(this ITestOutputHelper console, object value)
        {
            console.WriteLine($"{value}");
        }
        public static void WriteError(this ITestOutputHelper console, Exception e)
        {
            console.WriteLine($"{new LogEntry(e)}");
        }
        public static JObject WriteJson(this ITestOutputHelper console, object value)
        {
            if (value is string text)
            {
                try
                {
                    var json = JObject.Parse(text);
                    console.WriteLine(json.ToString());
                    return json;
                }
                catch (Exception)
                {
                    console.WriteLine(text);
                }
            }

            if (value is DomainObject dm)
            {
                var text2 = dm.ToJsonString(true);
                console.WriteLine(text2);
                return JObject.Parse(text2);
            }
            var jtext = value.ToJson();
            console.WriteLine(jtext);
            return JObject.Parse(jtext);
        }
    }
}
