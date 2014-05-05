using System;
using System.Linq;
using System.Threading.Tasks;

namespace offline.generator
{
    class Program
    {
        static void Main()
        {
            var width = Console.BufferWidth-1;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new string('*', width));
            Console.WriteLine("|{0}|", new String(' ', width - 2));

            WriteMessage("Reactive Developer");
            WriteMessage("This tool is use to create a offline assets");
            WriteMessage("(c) Bespoke Technology 2014");
            WriteMessage("Command line switches");
            WriteMessage("/o:<output folder> where the output will be generated");
            WriteMessage("/e:<EntityDefinition> the name of the EntityDefinition");
            WriteMessage("/q Quiet mode - non interactive");
            Console.WriteLine("|{0}|", new String(' ', width - 2));
            Console.WriteLine(new string('*', width));
            var entity = ParseArg("e");

            Console.ResetColor();

            Run(entity).Wait();
            Console.ResetColor();
            if (ParseArgExist("q"))return;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Press [ENTER] to exit : ");
            Console.ReadLine();
            Console.ResetColor();
        }

        private static void WriteMessage(string message)
        {
            var width = Console.BufferWidth-1;
            var margin = (width - 2 - message.Length) / 2;
            Console.WriteLine("|{0}{1}{0}|", new String(' ', margin), message);
        }

        private static async Task Run(string entity)
        {
            var output = ParseArg("o");
            var builder = new PageBuilder(entity);
            Console.WriteLine("Loading {0}", entity);
            await builder.LoadAsync();
            await builder.BuildDasboard(output);
            await builder.BuildForms(output);
        }
        private static string ParseArg(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
            return null == val ? null : val.Replace("/" + name + ":", string.Empty);
        }
        private static bool ParseArgExist(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name));
            return null != val;
        }
    }
}
