using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.Powershells
{
    [Cmdlet(VerbsLifecycle.Invoke, "RxBuilder", DefaultParameterSetName = PARAMETER_SET_NAME)]
    [Alias("rxbuilder")]
    public class RxBuilder : PSCmdlet, IDynamicParameters
    {
        private readonly IDictionary<string, string[]> m_sources = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        public const string PARAMETER_SET_NAME = "RxBuilder";
        public const string PARAMETER_SET_ED = "EntityDefinition";

        [Parameter(HelpMessage = "Asset type trusted connection", ParameterSetName = PARAMETER_SET_NAME)]
        [ValidateSet("EntityDefinition", "Adapter", "OperationEndpoint", "QueryEndpoint", "ReceivePort", "ReceiveLocation", "TransformDefinition", "WorkflowDefinition", "Trigger")]
        public string AssetType { get; set; } = "EntityDefinition";

        [Parameter(HelpMessage = "Trace switch for ConsoleLogger", ParameterSetName = PARAMETER_SET_NAME)]
        [ValidateSet("Debug", "Verbose", "Info", "Warning", "Error")]
        public string TraceSwitch { get; set; } = "Debug";

        [Parameter(HelpMessage = "EntityDefinition from Get-RxEntityDefinition", ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_ED)]
        public EntityDefinition EntityDefinition { get; set; }


        [Parameter(ParameterSetName = PARAMETER_SET_NAME)]
        [Parameter(ParameterSetName = PARAMETER_SET_ED)]
        public string RxApplicationName { get; set; }

        [Parameter(HelpMessage = "Start the process in new window", ParameterSetName = PARAMETER_SET_NAME)]
        public SwitchParameter UseShellExecute { get; set; } = false;


        private string[] GetSources(string type)
        {
            var source = $@"{this.SessionState.Path.CurrentFileSystemLocation}\sources\{type}\";
            var files = new[] { $"Cannot find any {type} in {Path.GetFullPath(source)}" };
            if (Directory.Exists(source))
            {
                files = (from f in Directory.GetFiles(source, "*.json", SearchOption.AllDirectories)
                         select Path.GetFileNameWithoutExtension(f))
                    .ToArray();
            }

            return files;
        }

        protected override void BeginProcessing()
        {
            ValidateParameters();
            WriteVerbose(this.ParameterSetName);
        }

        protected override void ProcessRecord()
        {
            var toolsSphBuilderExe = $@"{this.SessionState.Path.CurrentFileSystemLocation}\tools\sph.builder.exe";
            if (this.ParameterSetName == PARAMETER_SET_ED)
            {
                var jsonFile = $@"{ConfigurationManager.SphSourceDirectory}\{nameof(EntityDefinition)}\{this.EntityDefinition.Id}.json /switch:{TraceSwitch}";

                var info2 = new ProcessStartInfo
                {
                    FileName = toolsSphBuilderExe,
                    Arguments = jsonFile,
                    UseShellExecute = UseShellExecute.IsPresent

                };

                var builder2 = Process.Start(info2);
                builder2?.WaitForExit();

                WriteObject(this.EntityDefinition);
                return;
            }


            var source = ((DynParamQuotedString)MyInvocation.BoundParameters["Source"]).OriginalString;
            var file = $@"{this.SessionState.Path.CurrentFileSystemLocation}\sources\{this.AssetType}\{source}.json /switch:{TraceSwitch}";
            WriteVerbose($"Source = {file}");

            var info = new ProcessStartInfo
            {
                FileName = toolsSphBuilderExe,
                Arguments = file,
                UseShellExecute = UseShellExecute.IsPresent

            };

            var builder = Process.Start(info);
            builder?.WaitForExit();

            WriteObject(source);
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

        public object GetDynamicParameters()
        {
            var parameters = new RuntimeDefinedParameterDictionary();

            m_sources[this.AssetType] = GetSources(this.AssetType);
            var sourceParameter = new RuntimeDefinedParameter(
                "Source",
                typeof(DynParamQuotedString),
                new Collection<Attribute>
                {
                        new ParameterAttribute {
                            ParameterSetName = PARAMETER_SET_NAME,
                            Position = 0,
                            Mandatory = true
                        },
                        new ValidateSetAttribute(DynParamQuotedString.GetQuotedStrings(m_sources[this.AssetType])),
                        new ValidateNotNullOrEmptyAttribute()
                }
                );

            parameters.Add(sourceParameter.Name, sourceParameter);


            return parameters;
        }


    }
}