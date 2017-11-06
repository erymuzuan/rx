using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.Powershells
{
    [Cmdlet(VerbsLifecycle.Invoke, "RxDeploy")]
    [Alias("rxdeploy")]
    public class RxDeploy : PSCmdlet, IDynamicParameters
    {
        public const string PARAMETER_SET_NAME = "RxDeploy";
        public const string PARAMETER_SET_ED = "EntityDefinition";

        [Parameter(HelpMessage = "EntityDefinition from Get-RxEntityDefinition", ValueFromPipeline = true, ParameterSetName = PARAMETER_SET_ED)]
        public EntityDefinition PipeLineEntityDefinition { get; set; }

        [Parameter(ParameterSetName = PARAMETER_SET_NAME)]
        [Parameter(ParameterSetName = PARAMETER_SET_ED)]
        public string RxApplicationName { get; set; }

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
        private string[] GetMigrationPlans(string type = "MigrationPlan")
        {
            var source = $@"{this.SessionState.Path.CurrentFileSystemLocation}\sources\{type}\";
            var files = new[] { "Empty" };
            if (Directory.Exists(source))
            {
                files = (from f in Directory.GetFiles(source, "*.json", SearchOption.AllDirectories)
                         select Path.GetFileNameWithoutExtension(f))
                    .ToArray();
            }

            return files;
        }

        public const string ENTITY_DEFINITION = "EntityDefinition";
        protected override void ProcessRecord()
        {
            var args = "/i";
            object pipeLine = null;

            if (MyInvocation.BoundParameters.ContainsKey(ENTITY_DEFINITION))
            {
                var edparamters = MyInvocation.BoundParameters[ENTITY_DEFINITION];
                var ed = ((DynParamQuotedString)edparamters).OriginalString;
                var plan = "Empty";
                if (MyInvocation.BoundParameters.ContainsKey("MigrationPlan"))
                    plan = ((DynParamQuotedString)MyInvocation.BoundParameters["MigrationPlan"]).OriginalString;

                args = $"/deploy /e:{ed} /plan:{plan}";
                pipeLine = ed;
            }

            if (this.ParameterSetName == PARAMETER_SET_ED)
            {
                pipeLine = this.PipeLineEntityDefinition;
                var plan = "Empty";
                if (MyInvocation.BoundParameters.ContainsKey("MigrationPlan"))
                    plan = ((DynParamQuotedString)MyInvocation.BoundParameters["MigrationPlan"]).OriginalString;
                args = $"/deploy /e:{this.PipeLineEntityDefinition.Name} /plan:{plan}";
            }


            var deployExe = $@"{this.SessionState.Path.CurrentFileSystemLocation}\tools\deployment.agent.exe";
            WriteVerbose($"Executing deployment.agent.exe {args}");

            var info = new ProcessStartInfo
            {
                FileName = deployExe,
                Arguments = args,
                UseShellExecute = UseShellExecute

            };

            var deployer = Process.Start(info);
            deployer?.WaitForExit();

            if (null != pipeLine)
                WriteObject(pipeLine);
        }


        public object GetDynamicParameters()
        {
            var parameters = new RuntimeDefinedParameterDictionary();

            var entityDefinitionParameter = new RuntimeDefinedParameter(
                "EntityDefinition",
                typeof(DynParamQuotedString),
                new Collection<Attribute>
                {
                    new ParameterAttribute {
                        ParameterSetName = PARAMETER_SET_NAME,
                        Position = 0,
                        Mandatory = false,
                        ValueFromPipeline = true
                    },
                    new ValidateSetAttribute(DynParamQuotedString.GetQuotedStrings(GetSources("EntityDefinition"))),
                    new ValidateNotNullOrEmptyAttribute(),
                    new AliasAttribute("e")
                }
            );


            parameters.Add(entityDefinitionParameter.Name, entityDefinitionParameter);
            parameters.Add("MigrationPlan", new RuntimeDefinedParameter(
                    "MigrationPlan",
                    typeof(DynParamQuotedString),
                    new Collection<Attribute>
                    {
                        new ValidateSetAttribute(DynParamQuotedString.GetQuotedStrings(GetMigrationPlans())),
                        new ParameterAttribute
                        {
                            ParameterSetName = PARAMETER_SET_NAME,
                            Position = 1,
                            Mandatory = false
                        },
                        new AliasAttribute("p", "plan")
                    }
                ));

            return parameters;
        }


    }
}