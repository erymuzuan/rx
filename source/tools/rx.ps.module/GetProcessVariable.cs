using System;
using System.Diagnostics;
using System.Management.Automation;

namespace Bespoke.Sph.Powershells
{
    [Cmdlet(VerbsCommon.Get, "ProcessVariable")]
    public class GetProcessVariable : PSCmdlet
    {
        [Parameter(Position = 0,ValueFromPipeline = true,ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string[] Name { set; get; }
        
        [Parameter(HelpMessage = "Proces name", Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Variable { get; set; }

        protected override void ProcessRecord()
        {
            if (this.Name.Length != 1)
            {
                WriteError(new ErrorRecord(new Exception("You could only supply one process"), "0", ErrorCategory.FromStdErr, this.Name));
                return;
            }
            var processes = Process.GetProcessesByName(this.Name[0]);

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