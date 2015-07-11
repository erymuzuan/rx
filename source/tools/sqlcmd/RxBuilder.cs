using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace sqlcmd
{

    [Cmdlet(VerbsLifecycle.Invoke, "RxCompiler", DefaultParameterSetName = "rx")]
    [Alias("rxc")]
    public class RxBuilder : Cmdlet, IDynamicParameters
    {
        public string[] Entities { get; set; }

        public object GetDynamicParameters()
        {
            var source = @".\sources\EntityDefinition\";
            var files = new[] { "Cannot find any EntityDefinition in " + Path.GetFullPath(source) };
            if (Directory.Exists(source))
            {

                files = (from f in Directory.GetFiles(source, "*.json")
                         select Path.GetFileNameWithoutExtension(f))
                    .ToArray();
            }

            var parameters = new RuntimeDefinedParameterDictionary
            {
                {
                    "Entities",
                    new RuntimeDefinedParameter("Entities", typeof (string[]),
                        new Collection<Attribute>()
                        {
                            new ParameterAttribute {Position = 0, ParameterSetName = "rx", Mandatory = true},
                            new ValidateNotNullOrEmptyAttribute(),
                            new ValidateSetAttribute(files.ToArray())
                        })
                }
            };

            return parameters;
        }
        protected override void ProcessRecord()
        {
            if (null == this.Entities)
            {
                WriteObject("NULLLL");
                return;
            }
            foreach (var ent in Entities)
            {

                WriteObject("test " + ent);
            }
            base.ProcessRecord();
        }
        

    }
}