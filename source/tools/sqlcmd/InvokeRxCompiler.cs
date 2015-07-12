using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace Bespoke.Sph.Powershells
{
    [Cmdlet(VerbsLifecycle.Invoke, "RxC")]
    public class InvokeRxCompiler : PSCmdlet, IDynamicParameters
    {
        readonly IDictionary<string, string[]> m_sources = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        public InvokeRxCompiler()
        {
            m_sources.Add("EntityDefinition", new string[] { });
            m_sources.Add("Trigger", new string[] { });
            m_sources.Add("WorkflowDefinition", new string[] { });
            m_sources.Add("TransformDefinition", new string[] { });
            m_sources.Add("Adapter", new string[] { });

        }

        private string[] GetSources(string type)
        {
            var source = $@"{this.SessionState.Path.CurrentFileSystemLocation}\sources\{type}\";
            var files = new[] { $"Cannot find any {type} in {Path.GetFullPath(source)}" };
            if (Directory.Exists(source))
            {

                files = (from f in Directory.GetFiles(source, "*.json")
                         select Path.GetFileNameWithoutExtension(f))
                    .ToArray();
            }

            return files;
        }

        protected override void ProcessRecord()
        {
            WriteObject($"Type = {m_type}");
            var source = ((DynParamQuotedString)MyInvocation.BoundParameters["Source"]).OriginalString;
            string file = $@"{this.SessionState.Path.CurrentFileSystemLocation}\sources\{m_type}\{source}.json";
            WriteObject($"Source = {source}");
            WriteObject($"Source = {file}");

            var toolsSphBuilderExe = $@"{this.SessionState.Path.CurrentFileSystemLocation}\tools\sph.builder.exe";
            var info = new ProcessStartInfo
            {
                FileName = toolsSphBuilderExe,
                Arguments = file,
                CreateNoWindow = true,
                UseShellExecute = true

            };

            var builder = Process.Start(info);
            builder?.WaitForExit();
        }

        // Making this static means it should keep track of the last Type used
        static string m_type;
        public object GetDynamicParameters()
        {
            // Get 'Type' if found, otherwise get first unnamed value
            string author = GetUnboundValue("Type", 0) as string;
            if (!string.IsNullOrEmpty(author))
            {
                m_type = author.Trim('\'').Replace(
                    string.Format("{0}{0}", DynParamQuotedString.DefaultQuoteCharacter),
                    DynParamQuotedString.DefaultQuoteCharacter
                    );
            }

            var parameters = new RuntimeDefinedParameterDictionary();

            bool isTypeParameterMandatory = true;
            if (!string.IsNullOrEmpty(m_type) && m_sources.ContainsKey(m_type))
            {
                m_sources[m_type] = GetSources(m_type);
                isTypeParameterMandatory = false;
                var sourceParameter = new RuntimeDefinedParameter(
                    "Source",
                    typeof(DynParamQuotedString),
                    new Collection<Attribute>
                    {
                        new ParameterAttribute {
                            ParameterSetName = "SourceStuff",
                            Position = 1,
                            Mandatory = true
                        },
                        new ValidateSetAttribute(DynParamQuotedString.GetQuotedStrings(m_sources[m_type])),
                        new ValidateNotNullOrEmptyAttribute()
                    }
                    );

                parameters.Add(sourceParameter.Name, sourceParameter);
            }

            // Create author parameter. Parameter isn't mandatory if _author
            // has a valid author in it
            var authorParameter = new RuntimeDefinedParameter(
                "Type",
                typeof(DynParamQuotedString),
                new Collection<Attribute>
                {
                    new ParameterAttribute {
                        ParameterSetName = "SourceStuff",
                        Position = 0,
                        Mandatory = isTypeParameterMandatory
                    },
                    new ValidateSetAttribute(DynParamQuotedString.GetQuotedStrings(m_sources.Keys.ToArray())),
                    new ValidateNotNullOrEmptyAttribute()
                }
                );
            parameters.Add(authorParameter.Name, authorParameter);

            return parameters;
        }

        /*
            TryGetProperty() and GetUnboundValue() are from here: https://gist.github.com/fearthecowboy/1936f841d3a81710ae87
            Source created a dictionary for all unbound values; I had issues getting ValidateSet on Type parameter to work
            if I used that directly for some reason, but changing it into a function to get a specific parameter seems to work
        */

        object TryGetProperty(object instance, string fieldName)
        {
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

            // any access of a null object returns null. 
            if (instance == null || string.IsNullOrEmpty(fieldName))
            {
                return null;
            }

            var propertyInfo = instance.GetType().GetProperty(fieldName, bindingFlags);

            if (propertyInfo != null)
            {
                try
                {
                    return propertyInfo.GetValue(instance, null);
                }
                catch
                {
                    // ignored
                }
            }

            // maybe it's a field
            var fieldInfo = instance.GetType().GetField(fieldName, bindingFlags);

            if (fieldInfo != null)
            {
                try
                {
                    return fieldInfo.GetValue(instance);
                }
                catch
                {
                    // ignored
                }
            }

            // no match, return null.
            return null;
        }

        object GetUnboundValue(string paramName)
        {
            return GetUnboundValue(paramName, -1);
        }

        object GetUnboundValue(string paramName, int unnamedPosition)
        {

            // If paramName isn't found, value at unnamedPosition will be returned instead
            var context = TryGetProperty(this, "Context");
            var processor = TryGetProperty(context, "CurrentCommandProcessor");
            var parameterBinder = TryGetProperty(processor, "CmdletParameterBinderController");
            var args = TryGetProperty(parameterBinder, "UnboundArguments") as IEnumerable;

            if (args != null)
            {
                var currentParameterName = string.Empty;
                object unnamedValue = null;
                int i = 0;
                foreach (var arg in args)
                {
                    var isParameterName = TryGetProperty(arg, "ParameterNameSpecified");
                    if (isParameterName != null && true.Equals(isParameterName))
                    {
                        string parameterName = TryGetProperty(arg, "ParameterName") as string;
                        currentParameterName = parameterName;

                        continue;
                    }

                    // Treat as a value:
                    var parameterValue = TryGetProperty(arg, "ArgumentValue");

                    if (currentParameterName != string.Empty)
                    {
                        // Found currentParameterName's value. If it matches paramName, return
                        // it
                        if (currentParameterName != null && currentParameterName.Equals(paramName, StringComparison.OrdinalIgnoreCase))
                        {
                            return parameterValue;
                        }
                    }
                    else if (i++ == unnamedPosition)
                    {
                        unnamedValue = parameterValue;  // Save this for later in case paramName isn't found
                    }

                    // Found a value, so currentParameterName needs to be cleared
                    currentParameterName = string.Empty;
                }

                return unnamedValue;
            }

            return null;
        }
    }
}