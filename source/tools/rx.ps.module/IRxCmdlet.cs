using System.Management.Automation;

namespace Bespoke.Sph.RxPs
{
    public interface IRxCmdlet
    {
        string RxApplicationName { set; get; }
        SessionState SessionState { get; }
        void ThrowTerminatingError(ErrorRecord error);
    }
}