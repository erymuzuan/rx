using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using Bespoke.Sph.RxPs.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.RxPs
{
    public class AssetNameCompleter<T> : IArgumentCompleter
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
            var files = Directory.GetFiles($@"{ConfigurationManager.SphSourceDirectory}\{typeof(T).Name}", "*.json");
            var list = files.Select(x => JObject.Parse(File.ReadAllText(x))).Select(x => x.SelectToken("$.Name").Value<string>());
            return list.ToArray();
        }
    }
}