using System.Diagnostics;
using System.Management.Automation;

namespace Bespoke.Sph.RxPs
{

    [Cmdlet(VerbsLifecycle.Stop, "RxWebServer")]
    public class StopRxWebServerCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }

    [Cmdlet(VerbsLifecycle.Start, "RxWebServer")]
    public class StartRxWebServerCmdlet : RxCmdlet
    {
        [Parameter]
        public string Trace { get; set; } = "error";
        [Parameter]
        public string IisExpressPath { get; set; } = @".\IIS Express\iisexpress.exe";
        [Parameter]
        public string Config { get; set; } = @".\config\applicationhost.config";


        [Parameter(HelpMessage = "Start the process in new window")]
        public SwitchParameter NoNewWindow { get; set; } 

        protected override void ProcessRecord()
        {
            var args = $@"/config:""{Config}"" /site:""web.{this.RxApplicationName}"" /trace:""error""";
            WriteVerbose($@"Start-Process -FilePath {IisExpressPath} {args}");

            var info = new ProcessStartInfo(IisExpressPath, args)
            {
                UseShellExecute = !NoNewWindow.IsPresent
            };
            Process.Start(info);
        }

    }
    [Cmdlet(VerbsCommon.Get, "RxWebServer")]
    public class GetRxWebServerCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }
}
