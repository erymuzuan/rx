using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;

namespace Bespoke.Sph.Mangements.Commands
{
    [Export(typeof(Command))]
    public class HelpCommand : Command
    {
        public override CommandParameter[] GetArgumentList()
        {
            return new[] { new CommandParameter("help", true, "?", "help") };
        }

        public override void Execute()
        {
            WriteInfo("Deployment Agent");
            WriteInfo(GetHelpText());
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