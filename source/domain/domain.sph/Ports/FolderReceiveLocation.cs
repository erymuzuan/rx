using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;
using Humanizer;

namespace Bespoke.Sph.Domain
{
    public partial class FolderReceiveLocation : ReceiveLocation
    {
        public override Task<IEnumerable<Class>> GenerateClassesAsync(ReceivePort port)
        {
            var list = new List<Class>();
            var watcher = new Class { Name = Name.ToPascalCase(), Namespace = port.CodeNamespace };
            watcher.AddNamespaceImport<FileSystemWatcher, HttpClient, Uri, DomainObject>();
            watcher.AddNamespaceImport<List<object>, Task>();
            list.Add(watcher);

            var start = new StringBuilder();
            start.AppendLine("public void Start()");
            start.AppendLine("{");
            start.AppendLine($"       var watcher = new FileSystemWatcher({Path.ToVerbatim()}, {Filter.ToVerbatim()});");
            start.AppendLine("       watcher.EnableRaisingEvents = true;");
            start.AppendLine("       watcher.Created += FswChanged;");
            start.AppendLine("}");
            watcher.AddMethod(new Method { Code = start.ToString() });


            var created = GenerateFileCreatedMethod(port);
            watcher.AddMethod(new Method { Code = created.ToString() });
            watcher.AddMethod(WaitForReadyCode());

            return Task.FromResult(list.AsEnumerable());
        }

        private static StringBuilder GenerateFileCreatedMethod(ReceivePort port)
        {
            var created = new StringBuilder();
            created.AppendLine($@" 
        private async void FswChanged(object sender, FileSystemEventArgs e)
        {{  
            var file = e.FullPath;
            var wip = file + "".wip"" ;
            await WaitReadyAsync(file);
            System.IO.File.Move(file, wip );
            var port = new {port.CodeNamespace}.{port.TypeName}();
            var lines = File.ReadAllLines(wip);
            var records = await port.ProcessAsync(lines);
             // now post it to the server
            using(var client  = new HttpClient())
            {{
                client.BaseAddress = new Uri(ConfigurationManager.BaseUrl);
                foreach(var r in records)
                {{
                    var request = new StringContent(r.ToJson());
                    var response = await client.PostAsync(""api/{port.Entity.Pluralize().ToIdFormat()}"", request);
                    Console.WriteLine("" "" + response.StatusCode);
                }}
            }}
            System.IO.File.Delete(wip);
        }}");
            return created;
        }

        private Method WaitForReadyCode()
        {
            return new Method()
            {
                Code = $@"
public async Task WaitReadyAsync(string fileName)
{{
    while (true)
    {{
        try
        {{
            using (Stream stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {{
                if (stream != null)
                {{
                    Console.WriteLine(string.Format(""Output file {{0}} ready."", fileName));
                    break;
                }}
            }}
        }}
        catch (FileNotFoundException ex)
        {{
            Console.WriteLine(string.Format(""Output file {{0}} not yet ready ({{1}})"", fileName, ex.Message));
        }}
        catch (IOException ex)
        {{
            Console.WriteLine(string.Format(""Output file {{0}} not yet ready ({{1}})"", fileName, ex.Message));
        }}
        catch (UnauthorizedAccessException ex)
        {{
            Console.WriteLine(string.Format(""Output file {{0}} not yet ready ({{1}})"", fileName, ex.Message));
        }}
        await Task.Delay(500);
    }}
}}"
            };
        }
    }
}