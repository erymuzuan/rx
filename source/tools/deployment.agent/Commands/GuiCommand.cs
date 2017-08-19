using System.ComponentModel.Composition;
using System.Diagnostics;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Mangements.Commands
{
    [Export(typeof(Command))]
    public class GuiCommand : Command
    {
        public override CommandParameter[] GetArgumentList()
        {
            return new[] { new CommandParameter("gui", true, "i", "ui") };
        }

        public override void Execute(EntityDefinition ed)
        {
            var start = new ProcessStartInfo($"{ConfigurationManager.ToolsPath}\\deployment.gui.exe")
            {
                WorkingDirectory = ConfigurationManager.ToolsPath
            };
            var gui = Process.Start(start);
            gui?.WaitForExit();
        }
    }
}