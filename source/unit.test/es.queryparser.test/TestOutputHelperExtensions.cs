using Bespoke.Sph.Domain;
using Xunit.Abstractions;

namespace Bespoke.Sph.EsQueryParserTests
{
    public static class TestOutputHelperExtensions
    {
        public static void WriteJson(this ITestOutputHelper console, object value)
        {
            console.WriteLine(value.ToJson());
        }
        public static void WriteLine(this ITestOutputHelper console, object value)
        {
            console.WriteLine($"{value}");
        }
    }
}