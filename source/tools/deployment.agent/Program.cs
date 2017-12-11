using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Mangements
{
    internal class Program
    {
        [ImportMany]
        public Commands.Command[] Commands { set; get; }
        public static void Main(string[] args)
        {
            var logger = new Logger();
            logger.Loggers.Add(new ConsoleLogger { TraceSwitch = Severity.Info });
            if (TryParseArg("switch", out var tsw))
            {
                var ts = (Severity)Enum.Parse(typeof(Severity), tsw, true);
                logger.Loggers.OfType<ConsoleLogger>().Single().TraceSwitch = ts;
                logger.TraceSwitch = ts;
                if (TryParseArg("out", out var outputFile))
                {
                    logger.Loggers.Add(new FileLogger(outputFile, FileLogger.Interval.Hour) { TraceSwitch = ts });
                }
            }
            ObjectBuilder.AddCacheList<ILogger>(logger);


            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            var program = new Program();
            program.StartAsync().Wait();
        }
        private static bool TryParseArg(string key, out string value)
        {
            value = ParseArg(key);
            return !string.IsNullOrWhiteSpace(value);

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

        private async Task StartAsync()
        {
            if (!this.Compose())
            {
                Console.WriteLine(@"Error compose");
                return;
            }
            Console.WriteLine($@"We got {this.Commands.Length} commands");


            var help = this.Commands.Single(x => x.GetType() == typeof(Commands.HelpCommand));
            var commands = this.Commands.Where(x => x.IsSatisfied()).ToList();
            if (commands.Count == 0)
            {
                help.Execute();
                return;
            }

            if (commands.Count(x => !x.ShouldContinue()) > 1)
            {
                help.Execute();
                return;
            }
            foreach (var cmd in commands.OrderBy(x => !x.ShouldContinue()))
            {
                Console.WriteLine($@"Running {cmd.GetType().Name} ....");
                if (cmd.UseAsync)
                    await cmd.ExecuteAsync();
                else
                    cmd.Execute();

                Console.WriteLine($@"Done with {cmd.GetType().Name} ....");
            }

        }


        private CompositionContainer m_container;

        private bool Compose()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            // TODO : there's conflict with other assemblies
            // catalog.Catalogs.Add(new DirectoryCatalog("."));

            m_container = new CompositionContainer(catalog);
            var batch = new CompositionBatch();
            batch.AddPart(this);
            batch.AddExportedValue(m_container);

            try
            {
                m_container.Compose(batch);
            }
            catch (CompositionException compositionException)
            {
                Debug.WriteLine(compositionException);
                Debugger.Break();

            }
            return true;
        }


        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assembly = args.Name.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .First().Trim();
            var subs = $"{ConfigurationManager.SubscriberPath}\\{assembly}.dll";
            if (File.Exists(subs))
                return Assembly.LoadFile(subs);
            subs = $"{ConfigurationManager.CompilerOutputPath}\\{assembly}.dll";
            if (File.Exists(subs))
                return Assembly.LoadFile(subs);
            throw new Exception("Cannot load " + subs);
        }


    }
}
