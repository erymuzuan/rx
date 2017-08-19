using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using Bespoke.Sph.Domain;
using Colorful;

namespace Bespoke.Sph.Mangements.Commands
{
    [Export(typeof(Command))]
    public class DebugCommand : Command
    {
        public override CommandParameter[] GetArgumentList()
        {
            return new[] { new CommandParameter("debug", true) };
        }

        public override bool ShouldContinue()
        {
            return true;
        }

        public override void Execute(EntityDefinition ed)
        {
            Console.WriteLine($"Attach your debugger and to {Process.GetCurrentProcess().ProcessName} and press [ENTER] to continue");
            System.Console.ReadLine();

        }
    }

}
