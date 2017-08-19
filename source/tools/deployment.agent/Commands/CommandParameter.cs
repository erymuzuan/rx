using System;
using System.Collections.Generic;
using System.Linq;

namespace Bespoke.Sph.Mangements.Commands
{
    public class CommandParameter
    {
        public CommandParameter()
        {

        }

        public CommandParameter(int position, bool required)
        {
            Position = position;
            IsRequired = required;
        }

        public CommandParameter(string name, bool required, params string[] switches)
        {
            Name = name;
            IsRequired = required;
            Switches = switches.Concat(new[] { name }).ToArray();
        }
        public string[] Switches { get; }
        public string Name { get; }
        public int Position { get; }
        public bool IsRequired { get; }


        public T GetValue<T>()
        {
            if (this.Position > 0)
            {
                var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => !x.StartsWith("/"))
                    .Where(x => !x.StartsWith("-"))
                    .ToArray();
                if (this.Position >= args.Length)
                    return default(T);
                
                var text = args[Position - 1];
                if (typeof(T) == typeof(string))
                    return (T)((object)text);

                if (typeof(T) == typeof(int?))
                {
                    if (int.TryParse(text, out int nullableIntValue))
                        return (T)(object)nullableIntValue;
                }
                if (typeof(T) == typeof(int))
                    return (T)(int.Parse(text) as object);

                if (typeof(T) == typeof(double?))
                {
                    if (double.TryParse(text, out double nullableDoubleValue))
                        return (T)(object)nullableDoubleValue;
                }
                if (typeof(T) == typeof(double))
                    return (T)(double.Parse(text) as object);

            }

            if (typeof(T) == typeof(string))
                return (T)(object)ParseArg(this.Switches);

            if (typeof(T) == typeof(int))
                return (T)(object)ParseArgInt32(this.Switches);

            if (typeof(T) == typeof(bool))
                return (T)(object)ParseArgExist(this.Switches);

            return default(T);
        }

        private static string ParseArg(params string[] keys)
        {
            IEnumerable<string> GetValue(string name)
            {

                var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
                yield return val?.Replace("/" + name + ":", string.Empty);
            }

            return keys.Select(GetValue).SelectMany(x => x).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
        }

        private static int? ParseArgInt32(params string[] keys)
        {
            IEnumerable<int?> GetValue(string name)
            {

                var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
                var text = val?.Replace("/" + name + ":", string.Empty);
                if (int.TryParse(text, out int number))
                    yield return number;
                yield return default(int?);
            }

            return keys.Select(GetValue).SelectMany(x => x).FirstOrDefault(x => x.HasValue);
        }

        private static bool ParseArgExist(params string[] keys)
        {
            IEnumerable<bool> GetValue(string name)
            {
                var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var val = args.SingleOrDefault(a => a.StartsWith("/" + name));
                yield return null != val;
            }

            return keys.Select(GetValue).SelectMany(x => x).Any(x => x);
        }

    }
}