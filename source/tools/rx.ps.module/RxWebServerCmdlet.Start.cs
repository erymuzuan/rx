using System.Diagnostics;
using System.Management.Automation;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.RxPs
{
    [Cmdlet(VerbsLifecycle.Start, "RxWebServer")]
    public class StartRxWebServerCmdlet : RxCmdlet
    {
        [Parameter]
        public string Trace { get; set; } = "error";
        [Parameter]
        public string IisExpressPath { get; set; } = $@"{ConfigurationManager.Home}\IIS Express\iisexpress.exe";
        [Parameter]
        public string Config { get; set; } = $@"{ConfigurationManager.Home}\config\applicationhost.config";


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
}