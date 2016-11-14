using Humanizer;

namespace mapping.test.runner
{
    public static class StringExtensions
    {
        public static string ToFixString(this object obj, int length)
        {
            var value = $"{obj}";
            var truncate = value.Truncate(length, Truncator.FixedLength, TruncateFrom.Left);
            if (truncate.Length == length)
                return truncate;
            return value + new string(' ', length - value.Length);
        }
    }
}