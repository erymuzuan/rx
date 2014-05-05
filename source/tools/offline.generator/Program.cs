using System;
using System.Linq;
using System.Threading.Tasks;

namespace offline.generator
{
    class Program
    {
        static void Main()
        {
            var width = Console.BufferWidth;
           
            Console.WriteLine(new string('*', width));
            Console.WriteLine("|{0}|", new String(' ', width-2));
            Console.WriteLine("|{0}|", new String(' ', width-2));

            WriteMessage("This tool is use to create a offline assets");
            WriteMessage("this tool is used without support");

            Console.WriteLine("|{0}|", new String(' ', width-2));
            Console.WriteLine("|{0}|", new String(' ', width-2));
            Console.WriteLine(new string('*', width));
            var entity = ParseArg("e");
            Run(entity).Wait();
            Console.WriteLine("Press [ENTER] to exit");
            Console.ReadLine();
        }

        private static void WriteMessage(string message)
        {
            var width = Console.BufferWidth;
            var margin = (width - 2 - message.Length)/2;
            Console.WriteLine("|{0}{1}{0} |", new String(' ', margin), message);
            
        }

        private static async Task Run(string entity)
        {
            var output = ParseArg("o");
            var builder = new PageBuilder(entity);
            Console.WriteLine("Loading {0}",entity);
            await builder.LoadAsync();
            await builder.BuildDasboard(output);
        }
        private static string ParseArg(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
            if (null == val) return null;
            return val.Replace("/" + name + ":", string.Empty);
        }
        private static bool ParseArgExist(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name));
            return null != val;
        }
    }
}
