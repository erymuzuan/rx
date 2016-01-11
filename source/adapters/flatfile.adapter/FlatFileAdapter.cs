using System;
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
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Integrations.Adapters
{
    [EntityType(typeof(Adapter))]
    [Export("AdapterDesigner", typeof(Adapter))]
    [DesignerMetadata(Name = "Flat file adapter", FontAwesomeIcon = "file-o", Route = "adapter.flatfile/0", RouteTableProvider = typeof(FlatFileAdapterRouteTableProvider))]
    public partial class FlatFileAdapter : Adapter
    {
        public static readonly string[] ImportDirectives =
        {
   typeof(Entity).Namespace ,
   typeof(int).Namespace ,
   typeof(Task<>).Namespace ,
   typeof(Enumerable).Namespace ,
   typeof(IEnumerable<>).Namespace,
   typeof(Encoding).Namespace ,
   typeof(CookieContainer).Namespace ,
   typeof(Directory).Namespace ,
   typeof(XmlAttributeAttribute).Namespace,
   typeof(FileHelpers.FieldFixedLengthAttribute).Namespace,
   "System.Web.Mvc",
   "Bespoke.Sph.Web.Helpers"

        };

        protected override Task<IEnumerable<Class>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            var code = new Class { Name = Name, Namespace = CodeNamespace, BaseClass = nameof(IDisposable) };
            code.ImportCollection.AddRange(ImportDirectives);
            var sources = new ObjectCollection<Class> { code };

            code.AddProperty("       public string Directory {get; set;}");
            code.AddProperty("       public string Filter {get; set;}");
            code.AddProperty("       private FileSystemWatcher m_watcher;");

            var start = new Method { Name = "Start", AccessModifier = Modifier.Public };
            start.AppendLine("       public void Start()");
            start.AppendLine("       {");
            start.AppendLine("           if(null != m_watcher) m_watcher.Dispose();");
            start.AppendLine("           m_watcher = new FileSystemWatcher(this.Directory, this.Filter) {EnableRaisingEvents = true};");
            start.AppendLine("           m_watcher.Created += (e, a) => { };");
            start.AppendLine("       }");
            code.MethodCollection.Add(start);

            var watcher = new FileSystemWatcher("c:\\temp", "*.csv") { EnableRaisingEvents = true };
            watcher.Created += (e, a) => { };

            code.AddMethod(AddDisposeCode());

            return Task.FromResult(sources.AsEnumerable());
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

      


        protected override Task<Class> GenerateOdataTranslatorSourceCodeAsync()
        {
            return Task.FromResult(default(Class));
        }

        protected override Task<Class> GeneratePagingSourceCodeAsync()
        {
            return Task.FromResult(default(Class));
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
