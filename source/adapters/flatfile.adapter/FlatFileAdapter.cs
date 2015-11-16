﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    [EntityType(typeof(Adapter))]
    [Export("AdapterDesigner", typeof(Adapter))]
    [DesignerMetadata(Name = "Flat file adapter", FontAwesomeIcon = "file-o", Route = "adapter.flatfile/0", RouteTableProvider = typeof(FlatFileAdapterRouteTableProvider))]
    public partial class FlatFileAdapter : Adapter
    {
        private string GetCodeHeader(params string[] namespaces)
        {
            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(int).Namespace + ";");
            header.AppendLine("using " + typeof(Task<>).Namespace + ";");
            header.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            header.AppendLine("using " + typeof(IEnumerable<>).Namespace + ";");
            header.AppendLine("using " + typeof(Encoding).Namespace + ";");
            header.AppendLine("using " + typeof(CookieContainer).Namespace + ";");
            header.AppendLine("using " + typeof(Directory).Namespace + ";");
            header.AppendLine("using " + typeof(XmlAttributeAttribute).Namespace + ";");
            header.AppendLine("using " + typeof(FileHelpers.FieldFixedLengthAttribute).Namespace + ";");
            header.AppendLine("using System.Web.Mvc;");
            header.AppendLine("using Bespoke.Sph.Web.Helpers;");
            foreach (var ns in namespaces)
            {
                header.AppendLinf("using {0};", ns);
            }
            header.AppendLine();

            header.AppendLine("namespace " + this.CodeNamespace);
            header.AppendLine("{");
            return header.ToString();

        }

        protected override Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            var sources = new Dictionary<string, string>();

            var header = this.GetCodeHeader(namespaces);
            var code = new StringBuilder(header);

            code.AppendLine("   public class " + this.Name + " : IDisposable");
            code.AppendLine("   {");

            code.AppendLine("       public string Directory {get; set;}");
            code.AppendLine("       public string Filter {get; set;}");
            code.AppendLine("       private FileSystemWatcher m_watcher;");
            code.AppendLine("       public void Start()");
            code.AppendLine("       {");
            code.AppendLine("           if(null != m_watcher) m_watcher.Dispose();");
            code.AppendLine("           m_watcher = new FileSystemWatcher(this.Directory, this.Filter) {EnableRaisingEvents = true};");
            code.AppendLine("           m_watcher.Created += (e, a) => { };");
            code.AppendLine("       }");

            var watcher = new FileSystemWatcher("c:\\temp", "*.csv") {EnableRaisingEvents = true};
            watcher.Created += (e, a) => { };

            code.AppendLine(AddDisposeCode());
            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace
            sources.Add(this.Name + ".cs", code.ToString());

            return Task.FromResult(sources);
        }

        private string AddDisposeCode()
        {

            var code = new StringBuilder();
            code.AppendLinf("       public {0}()", this.Name);
            code.AppendLine("       {");
            

            code.AppendLine("       }");
            code.AppendLine("       public void Dispose()");
            code.AppendLine("       {");

            code.AppendLine("       }");

            return code.ToString();
        }



        private static void AddSources(Dictionary<string, string> classes, Dictionary<string, string> sources)
        {
            foreach (var cs in classes.Keys)
            {
                if (!sources.ContainsKey(cs))
                {
                    sources.Add(cs, classes[cs]);
                    continue;
                }
                if (sources[cs] != classes[cs])
                    throw new InvalidOperationException("You are generating 2 different sources for " + cs);
            }
        }


        protected override Task<Tuple<string, string>> GenerateOdataTranslatorSourceCodeAsync()
        {
            return Task.FromResult(default(Tuple<string, string>));
        }

        protected override Task<Tuple<string, string>> GeneratePagingSourceCodeAsync()
        {
            return Task.FromResult(default(Tuple<string, string>));
        }

        protected override Task<TableDefinition> GetSchemaDefinitionAsync(string table)
        {
            var td = TableDefinitionCollection.SingleOrDefault(x => x.Name == table);
            return Task.FromResult(td);
        }

        public override string OdataTranslator => null;


        public Task OpenAsync()
        {
            return Task.FromResult(0);
        }
    }
}