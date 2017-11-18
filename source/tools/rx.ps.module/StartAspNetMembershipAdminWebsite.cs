using System.Management.Automation;

namespace Bespoke.Sph.RxPs
{
    [Cmdlet(VerbsCommon.Get, "RxAspNetMembershipAdmin")]
    public class GetAspNetMembershipAdminWebsite : Cmdlet
    {
        /*start "C:\Program Files\Internet Explorer\iexplore.exe" "http://localhost:4437/asp.netwebadminfiles/default.aspx?applicationPhysicalPath=c:\project\work\sph\source\web\web.sph&applicationUrl=/"
"C:\Program Files (x86)\IIS Express\iisexpress.exe" /path:"C:\Windows\Microsoft.NET\Framework\v4.0.30319\ASP.NETWebAdminFiles" /vpath:"/ASP.NETWebAdminFiles" /port:4437 /clr:"4.0" /ntlm
@rem � using the following values for the [param]:
@rem [port] � any port you have free in IISExpress (I use 8082 in the example below)
@rem This should launch an IISExpress instance of the Configuration Manager Site<br/>clip_image002
@rem Open your browser
@rem In the URL enter the following �http://localhost:8082/asp.netwebadminfiles/default.aspx?applicationPhysicalPath=[appPath]&applicationUrl=/� substituting the [appPath] with the absolute path to the Visual Studio Project folder with the solution file in it.
*/

        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }
    }
    [Cmdlet(VerbsLifecycle.Start, "RxAspNetMembershipAdmin")]
    public class StartAspNetMembershipAdminWebsite : Cmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }
    }

    [Cmdlet(VerbsLifecycle.Stop, "RxAspNetMembershipAdmin")]
    public class StopAspNetMembershipAdminWebsite : Cmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }
    }
}
