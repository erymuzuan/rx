using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.RxPs
{

    [Cmdlet(VerbsLifecycle.Stop, "RxControlCenter")]
    public class StopRxControlCenterCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }

    [Cmdlet(VerbsLifecycle.Start, "RxControlCenter")]
    public class StartRxControlCenterCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            var exe = @".\control.center\controlcenter.exe";
            if (!File.Exists(exe))
            {
                WriteError(new ErrorRecord(new FileNotFoundException("exe is not found", exe), exe, ErrorCategory.ObjectNotFound, this));
                return;
            }
            var info = new ProcessStartInfo
            {
                FileName = exe,

            };

            Process.Start(info);
            WriteVerbose("control center started");
        }

    }
    [Cmdlet(VerbsCommon.Get, "RxControlCenter")]
    public class GetRxControlCenterCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }
}
