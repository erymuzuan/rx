using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Bespoke.Sph.RxPs.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Powershells
{
    [Cmdlet(VerbsLifecycle.Invoke, "RxBuilder", DefaultParameterSetName = PARAMETER_SET_NAME)]
    [Alias("rxbuilder")]
    public class RxBuilder : PSCmdlet, IDynamicParameters
    {
        private readonly IDictionary<string, string[]> m_idList = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        public const string PARAMETER_SET_NAME = "Name";
        public const string PARAMETER_SET_ED = "EntityDefinition";
        public const string PARAMETER_SET_ID = "Id";

        [Parameter(HelpMessage = "Asset type trusted connection", ParameterSetName = PARAMETER_SET_NAME)]
        [ValidateSet("EntityDefinition", "Adapter", "OperationEndpoint", "QueryEndpoint", "ReceivePort", "ReceiveLocation", "TransformDefinition", "WorkflowDefinition", "Trigger")]
        public string AssetType { get; set; } = nameof(EntityDefinition);

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
            void ExecuteSphBuilder(string src)
            {
                var arg = $@"{ConfigurationManager.SphSourceDirectory}\{this.AssetType}\{src}.json /switch:{TraceSwitch}";
                var info = new ProcessStartInfo
                {
                    FileName = toolsSphBuilderExe,
                    Arguments = arg,
                    UseShellExecute = UseShellExecute.IsPresent

                };

                var builder = Process.Start(info);
                builder?.WaitForExit();

                WriteObject(src);
            }

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
            if (this.ParameterSetName == PARAMETER_SET_ID)
            {
                var source = ((DynParamQuotedString)MyInvocation.BoundParameters["Id"]).OriginalString;
                ExecuteSphBuilder(source);
                return;
            }

            if (this.ParameterSetName == PARAMETER_SET_NAME)
            {
                var nameValue = ((DynParamQuotedString)MyInvocation.BoundParameters["Name"]).OriginalString;
                var list = from o in Directory.GetFiles($@"{ConfigurationManager.SphSourceDirectory}\{this.AssetType}\",
                        "*.json")
                           let text = File.ReadAllText(o)
                           let json = JObject.Parse(text)
                           let nameField = json.SelectToken("$.Name")
                           where null != nameField
                           let name = nameField.Value<string>()
                           where name == nameValue
                           select json.SelectToken("$.Id").Value<string>();
                ExecuteSphBuilder(list.FirstOrDefault());
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

        public object GetDynamicParameters()
        {
            var parameters = new RuntimeDefinedParameterDictionary();
            m_idList[this.AssetType] = GetSources(this.AssetType);


            parameters.Add("Id", new RuntimeDefinedParameter(
                "Id",
                typeof(DynParamQuotedString),
                new Collection<Attribute>
                {
                    new ParameterAttribute {
                        ParameterSetName = PARAMETER_SET_ID,
                        Position = 0,
                        Mandatory = true,
                        ValueFromPipeline = true,
                        ValueFromPipelineByPropertyName = true
                    },
                    new ValidateSetAttribute(DynParamQuotedString.GetQuotedStrings(m_idList[this.AssetType])),
                    new ValidateNotNullOrEmptyAttribute()
                }
            ));

            parameters.Add("Name", new RuntimeDefinedParameter(
                "Name",
                typeof(DynParamQuotedString),
                new Collection<Attribute>
                {
                    new ParameterAttribute {
                        ParameterSetName = PARAMETER_SET_NAME,
                        Position = 0,
                        Mandatory = true,
                        ValueFromPipeline = true,
                        ValueFromPipelineByPropertyName = true
                    },
                    new ValidateSetAttribute(DynParamQuotedString.GetQuotedStrings(GetAssetNames(this.AssetType))),
                    new ValidateNotNullOrEmptyAttribute()
                }
            ));


            return parameters;
        }

        private static IEnumerable<string> GetAssetNames(string assetType)
        {
            var list = from o in Directory.GetFiles($@"{ConfigurationManager.SphSourceDirectory}\{assetType}\",
                    "*.json")
                let text = File.ReadAllText(o)
                let json = JObject.Parse(text)
                let nameField = json.SelectToken("$.Name")
                where null != nameField
                select nameField.Value<string>();

            return list.ToArray();
        }
    }
}