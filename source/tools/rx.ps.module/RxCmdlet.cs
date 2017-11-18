using System;
using System.Management.Automation;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.RxPs
{
    public class RxCmdlet : PSCmdlet, IRxCmdlet
    {
        [Parameter(ParameterSetName = RxWorkerCmdlet.PARAMETER_SET_DEFAULT)]
        public string RxApplicationName { get; set; }

        protected override void BeginProcessing()
        {
            ValidateParameters();
        }

        private void ValidateParameters()
        {
            const string RX_APPLICATION_NAME = "RxApplicationName";

            if (!string.IsNullOrEmpty(RxApplicationName))
            {
                SessionState.PSVariable.Set(RX_APPLICATION_NAME, RxApplicationName);
            }
            else
            {
                RxApplicationName = SessionState.PSVariable.GetValue(RX_APPLICATION_NAME, string.Empty).ToString();
                if (string.IsNullOrEmpty(RxApplicationName))
                {
                    ThrowParameterError(nameof(RxApplicationName));
                }
            }
            ConfigurationManager.Initialize(RxApplicationName);

        }

        protected void ThrowParameterError(string parameterName)
        {
            ThrowTerminatingError(
                new ErrorRecord(
                    new ArgumentException($"Must specify '{parameterName}'"),
                    Guid.NewGuid().ToString(),
                    ErrorCategory.InvalidArgument,
                    null));
        }
    }
}