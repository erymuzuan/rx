using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Bespoke.Sph.RxPs.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.RxPs
{
    // TODO : create a abstract class RxAssetCmdlet<T> so that we cater for other assets, Trigger, WorkflowDefinition, TransformDefinition etc.
    [Cmdlet(VerbsCommon.Get, "Rx" + nameof(EntityDefinition), DefaultParameterSetName = PARAMETER_SET_NAME_EMPTY)]
    [OutputType(typeof(EntityDefinition))]
    public class RxEntityDefinitionCmdLet : PSCmdlet
    {
        public const string PARAMETER_SET_NAME_EMPTY = "Empty";
        public const string PARAMETER_SET_NAME_NAME = "Name";
        public const string PARAMETER_SET_NAME_ID = "Id";

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_NAME)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_ID)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_EMPTY)]
        public string RxApplicationName { get; set; }

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_NAME), ArgumentCompleter(typeof(AssetNameCompleter<EntityDefinition>))]
        public string Name { set; get; }

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_ID), ArgumentCompleter(typeof(AssetIdCompleter<EntityDefinition>))]
        public string Id { get; set; }


        protected override void BeginProcessing()
        {
            ValidateParameters();
            WriteVerbose(this.ParameterSetName);
        }

        protected override void ProcessRecord()
        {
            if (this.ParameterSetName == PARAMETER_SET_NAME_EMPTY)
            {
                var files = Directory.GetFiles($@"{ConfigurationManager.SphSourceDirectory}\{nameof(EntityDefinition)}", "*.json");
                var list = files.Select(x => JObject.Parse(File.ReadAllText(x))).Select(EntityDefinition.Parse);
                WriteObject(list, true);
            }
            if (this.ParameterSetName == PARAMETER_SET_NAME_NAME)
            {
                var files = Directory.GetFiles($@"{ConfigurationManager.SphSourceDirectory}\{nameof(EntityDefinition)}", "*.json");
                var list = files.Select(x => JObject.Parse(File.ReadAllText(x))).Select(EntityDefinition.Parse)
                    .Where(x => x.Name == this.Name);
                WriteObject(list, true);
            }
            if (this.ParameterSetName == PARAMETER_SET_NAME_ID)
            {
                var files = Directory.GetFiles($@"{ConfigurationManager.SphSourceDirectory}\{nameof(EntityDefinition)}", "*.json");
                var list = files.Select(x => JObject.Parse(File.ReadAllText(x))).Select(EntityDefinition.Parse)
                    .Where(x => x.Id == this.Id);
                WriteObject(list, true);
            }
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

        private void ThrowParameterError(string parameterName)
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
