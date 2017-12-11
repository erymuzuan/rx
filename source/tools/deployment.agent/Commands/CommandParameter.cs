using System;
using System.Collections.Generic;
using System.Drawing;
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
        public CommandParameter(string name, Func<string[]> promptOptions, params string[] switches)
        {
            Name = name;
            PromptOptions = promptOptions;
            Switches = switches.Concat(new[] { name }).ToArray();
        }

        public string[] Switches { get; }
        public string Name { get; }
        public Func<string[]> PromptOptions { get; }
        public int Position { get; }
        public bool IsRequired { get; }


        public T GetValue<T>()
        {
            T ReadSwithces()
            {
                if (typeof(T) == typeof(string))
                    return (T)(object)ParseArg(this.Switches);

                if (typeof(T) == typeof(int))
                    return (T)(object)ParseArgInt32(this.Switches);

                if (typeof(T) == typeof(bool))
                    return (T)(object)ParseArgExist(this.Switches);

                return default;

            }
            T Parse(string text)
            {
                if (typeof(T) == typeof(string))
                    return (T)(object)text;

                if (typeof(T) == typeof(int?))
                {
                    if (int.TryParse(text, out var nullableIntValue))
                        return (T)(object)nullableIntValue;
                }
                if (typeof(T) == typeof(int))
                    return (T)(int.Parse(text) as object);

                if (typeof(T) == typeof(double?))
                {
                    if (double.TryParse(text, out var nullableDoubleValue))
                        return (T)(object)nullableDoubleValue;
                }
                if (typeof(T) == typeof(double))
                    return (T)(double.Parse(text) as object);
                return default;
            }

            if (null != this.PromptOptions)
            {
                var val = ReadSwithces();
                if (!string.IsNullOrWhiteSpace($"{val}"))
                    return val;

                Console.WriteLine(@"-------------------------------------------------");
                Console.WriteLine($@"    Please select your ""{this.Name}"" from these options ");
                Console.WriteLine(@"-------------------------------------------------");
                var options = this.PromptOptions();
                for (var i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($@"{i + 1}. {options[i]}");
                }
                Console.WriteLine();
                Console.Write(@" Key in your options and press [Enter] : ");
                if (int.TryParse(System.Console.ReadLine(), out var choice))
                {
                    if (choice >= options.Length)
                    {
                        Console.WriteLine($@"Please key in number between 1 and {options.Length}");
                    }
                    var text = options[choice - 1];
                    return Parse(text);
                }
                Console.WriteLine();

            }
            if (this.Position <= 0) return ReadSwithces();
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !x.StartsWith("/"))
                .Where(x => !x.StartsWith("-"))
                .ToArray();
            if (this.Position >= args.Length)
                return default;

            return Parse(args[Position]);
        }

        private static string ParseArg(params string[] keys)
        {
            IEnumerable<string> GetValue(string name)
            {
                var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
                yield return val?.Replace("/" + name + ":", string.Empty);
            }

            return keys.Select(GetValue).Where(x => null != x).SelectMany(x => x).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
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
                yield return default;
            }

            return keys.Select(GetValue).Where(x => null != x).SelectMany(x => x).FirstOrDefault(x => x.HasValue);
        }

        private static bool ParseArgExist(params string[] keys)
        {
            IEnumerable<bool> GetValue(string name)
            {
                var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var val = args.SingleOrDefault(a => a.StartsWith("/" + name));
                yield return null != val;
            }

            return keys.Select(GetValue).Where(x => null != x).SelectMany(x => x).Any(x => x);
        }

    }
}