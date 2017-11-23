using Xunit.Abstractions;

namespace domain.test.Extensions
{
    public static class TestOutputHelperExtension
    {
        public static T WriteLine<T>(this ITestOutputHelper console, T value)
        {
            console.WriteLine($"{value}");
            return value;
        }
    }
}
