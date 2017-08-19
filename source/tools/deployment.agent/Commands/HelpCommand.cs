using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Reflection;
using Bespoke.Sph.Domain;
using Console = Colorful.Console;

namespace Bespoke.Sph.Mangements.Commands
{
    [Export(typeof(Command))]
    public class HelpCommand : Command
    {
        public override CommandParameter[] GetArgumentList()
        {
            return new[] { new CommandParameter("help", true, "?", "help") };
        }

        public override void Execute(EntityDefinition ed)
        {
            Console.WriteAscii("Deployment Agent", Color.BlueViolet);
            Console.WriteLine(GetHelpText());
        }


        private static string GetHelpText()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string HELP_TEXT = "Bespoke.Sph.Mangements.HelpText.md";

            using (var stream = assembly.GetManifestResourceStream(HELP_TEXT))
            using (var reader = new StreamReader(stream ?? throw new Exception("Cannot read HelpText.md")))
            {
                return reader.ReadToEnd();
            }
        }
    }
}