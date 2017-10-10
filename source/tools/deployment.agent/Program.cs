using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Console = Colorful.Console;

namespace Bespoke.Sph.Mangements
{
    internal class Program
    {
        [ImportMany]
        public Commands.Command[] Commands { set; get; }
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            var program = new Program();
            program.StartAsync().Wait();
        }
        
        private async Task StartAsync()
        {
            if (!this.Compose())
            {
                Console.WriteLine("Error compose", Color.OrangeRed);
                return;
            }
            Console.WriteLine($"We got {this.Commands.Length} commands");


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
                Console.WriteLine($"Running {cmd.GetType().Name} ....", Color.YellowGreen);
                if (cmd.UseAsync)
                    await cmd.ExecuteAsync();
                else
                    cmd.Execute();

                Console.WriteLine($"Done with {cmd.GetType().Name} ....", Color.Green);
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
