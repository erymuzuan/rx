using System;
using System.Diagnostics;
using System.Management.Automation;

namespace Bespoke.Sph.Powershells
{
    [Cmdlet(VerbsCommon.Get, "ProcessVariable")]
    public class GetProcessVariable : PSCmdlet
    {
        [Parameter(HelpMessage = "Process name", Mandatory = false)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; } = "controlcenter";



        [Parameter(HelpMessage = "Proces name", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Variable { get; set; }

        protected override void ProcessRecord()
        {
            var processes = Process.GetProcessesByName(this.Name);

            if (processes.Length == 0)
                WriteError(new ErrorRecord(new Exception("Process with a given name not found. Please modify the Name"), "0", ErrorCategory.FromStdErr, this.Name));

            foreach (var process in processes)
            {
                var env = process.ReadEnvironmentVariables();
                var value = env[this.Variable];
                WriteObject(value);
            }

        }


    }
}