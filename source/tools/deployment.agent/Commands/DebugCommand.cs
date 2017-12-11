using System.ComponentModel.Composition;
using System.Diagnostics;

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

        public override void Execute()
        {
            WriteInfo($"Attach your debugger and to {Process.GetCurrentProcess().ProcessName}({Process.GetCurrentProcess().Id}) and press [ENTER] to continue");
            System.Console.ReadLine();

        }
    }

}
