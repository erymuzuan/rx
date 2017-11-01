using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace Bespoke.Sph.Powershells
{
    [Cmdlet(VerbsLifecycle.Invoke, "RxBuilder")]
    [Alias("rx-builder")]
    public class RxBuilder : PSCmdlet, IDynamicParameters
    {
        private readonly IDictionary<string, string[]> m_sources = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        public const string PARAMETER_SET_NAME = "RxBuilder";

        [Parameter(HelpMessage = "Asset type trusted connection", ParameterSetName = PARAMETER_SET_NAME)]
        [ValidateSet("EntityDefinition", "Adapter", "OperationEndpoint", "QueryEndpoint", "ReceivePort", "ReceiveLocation", "TransformDefinition", "WorkflowDefinition", "Trigger")]
        public string AssetType { get; set; } = "EntityDefinition";

        [Parameter(HelpMessage = "Trace switch for ConsoleLogger", ParameterSetName = PARAMETER_SET_NAME)]
        [ValidateSet("Debug", "Verbose", "Info", "Warning", "Error")]
        public string TraceSwitch { get; set; } = "Debug";


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

        protected override void ProcessRecord()
        {
            var source = ((DynParamQuotedString)MyInvocation.BoundParameters["Source"]).OriginalString;
            var file = $@"{this.SessionState.Path.CurrentFileSystemLocation}\sources\{this.AssetType}\{source}.json /switch:{TraceSwitch}";
            WriteVerbose($"Source = {file}");

            var toolsSphBuilderExe = $@"{this.SessionState.Path.CurrentFileSystemLocation}\tools\sph.builder.exe";
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