using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace Bespoke.Sph.Powershells
{
    [Cmdlet(VerbsLifecycle.Invoke, "RxDeploy")]
    [Alias("rx-deploy")]
    public class RxDeploy : PSCmdlet, IDynamicParameters
    {
        public const string PARAMETER_SET_NAME = "RxDeploy";
        [Parameter(HelpMessage = "Deploy", ParameterSetName = PARAMETER_SET_NAME)]
        [Alias("deploy")]
        public SwitchParameter IsDeploy { get; set; } = false;

        [Parameter(HelpMessage = "Diff", ParameterSetName = PARAMETER_SET_NAME)]
        [Alias("diff")]
        public SwitchParameter IsDiff { get; set; } = false;

        [Parameter(HelpMessage = "Gui", ParameterSetName = PARAMETER_SET_NAME)]
        [Alias("i", "ui", "gui")]
        public SwitchParameter IsGui { get; set; } = SwitchParameter.Present;

        [Parameter(HelpMessage = "Trace switch for ConsoleLogger", ParameterSetName = PARAMETER_SET_NAME)]
        [ValidateSet("Debug", "Verbose", "Info", "Warning", "Error")]
        public string TraceSwitch { get; set; } = "Debug";

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

        protected override void ProcessRecord()
        {
            WriteObject($"TraceSwitch = {TraceSwitch}");
            var args = "";
            if (IsDeploy.IsPresent)
            {
                var ed = ((DynParamQuotedString)MyInvocation.BoundParameters["EntityDefinition"]).OriginalString;
                var plan = ((DynParamQuotedString)MyInvocation.BoundParameters["MigrationPlan"]).OriginalString;
                args = $"/deploy /e:{ed} /plan:{plan}";
            }
            if (IsGui.IsPresent)
                args = "/i";

            var deployExe = $@"{this.SessionState.Path.CurrentFileSystemLocation}\tools\deployment.agent.exe";
            var info = new ProcessStartInfo
            {
                FileName = deployExe,
                Arguments = args,
                CreateNoWindow = true,
                UseShellExecute = true

            };

            var deployer = Process.Start(info);
            deployer?.WaitForExit();
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
                        Mandatory = IsDeploy.IsPresent
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
                            Position = 2,
                            Mandatory = IsDeploy.IsPresent
                        },
                        new AliasAttribute("p", "plan")
                    }
                ));


            return parameters;
        }


    }
}