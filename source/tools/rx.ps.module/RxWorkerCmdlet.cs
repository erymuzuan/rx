using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using Bespoke.Sph.Powershells;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.RxPs
{
    public class RxWorkerCmdlet
    {
        public const string PARAMETER_SET_DEFAULT = "default";
    }


    public class WorkerProcessNameCompleter : IArgumentCompleter
    {
        IEnumerable<CompletionResult> IArgumentCompleter.CompleteArgument(string commandName,
            string parameterName,
            string wordToComplete,
            CommandAst commandAst,
            IDictionary fakeBoundParameters)
        {
            return GetAllowedNames().
                Where(new WildcardPattern(wordToComplete + "*", WildcardOptions.IgnoreCase).IsMatch).
                Select(s => new CompletionResult(s));
        }
        private static string[] GetAllowedNames()
        {
            var wps = from p in Process.GetProcessesByName("workers.console.runner")
                      let envs = p.ReadEnvironmentVariables()
                      select envs.ContainsKey("RxWorkerName") ? envs["RxWorkerName"] : "NA";
            return wps.ToArray();
        }
    }

    public class WorkerConfigCompleter : IArgumentCompleter
    {
        IEnumerable<CompletionResult> IArgumentCompleter.CompleteArgument(string commandName,
            string parameterName,
            string wordToComplete,
            CommandAst commandAst,
            IDictionary fakeBoundParameters)
        {
            return GetAllowedNames().
                Where(new WildcardPattern(wordToComplete + "*", WildcardOptions.IgnoreCase).IsMatch).
                Select(s => new CompletionResult(s));
        }
        private static string[] GetAllowedNames()
        {
            var files = Directory.GetFiles($@"{ConfigurationManager.SphSourceDirectory}\WorkersConfig", "*.json");
            var list = files.Select(Path.GetFileNameWithoutExtension)
                .Select(x => x.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToArray();
            return list.ToArray();
        }
    }

    public class WorkerEnvironmentCompleter : IArgumentCompleter
    {
        IEnumerable<CompletionResult> IArgumentCompleter.CompleteArgument(string commandName,
            string parameterName,
            string wordToComplete,
            CommandAst commandAst,
            IDictionary fakeBoundParameters)
        {
            return GetAllowedNames().
                Where(new WildcardPattern(wordToComplete + "*", WildcardOptions.IgnoreCase).IsMatch).
                Select(s => new CompletionResult(s));
        }
        private static string[] GetAllowedNames()
        {
            var files = Directory.GetFiles($@"{ConfigurationManager.SphSourceDirectory}\WorkersConfig", "*.json");
            var list = files.Select(Path.GetFileNameWithoutExtension)
                    .Select(x => x.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .ToArray();
            return list.ToArray();
        }
    }
}
