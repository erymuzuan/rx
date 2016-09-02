using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class FolderReceiveLocation : ReceiveLocation
    {
        public override Task<IEnumerable<Class>> GenerateClassesAsync(ReceivePort port)
        {
            var list = new List<Class>();
            var watcher = new Class { Name = Name.ToPascalCase(), Namespace = port.CodeNamespace, BaseClass = "IDisposable" };
            watcher.AddNamespaceImport<FileSystemWatcher, HttpClient, Uri, DomainObject>();
            watcher.AddNamespaceImport<List<object>, Task>();
            list.Add(watcher);
            watcher.AddProperty("private FileSystemWatcher m_watcher; ");
            watcher.AddProperty("private HttpClient m_client; ");

            var start = new StringBuilder();
            start.AppendLine("public void Start()");
            start.AppendLine("{");
            start.AppendLine($@"       
                        var m_watcher = new FileSystemWatcher({Path.ToVerbatim()}, {Filter.ToVerbatim()});
                        m_watcher.EnableRaisingEvents = true;
                        m_watcher.Created += FswChanged;
                        
                        var m_client = new HttpClient();                        
                        m_client.BaseAddress = new Uri(ConfigurationManager.BaseUrl);
");
            start.AppendLine("}");
            watcher.AddMethod(new Method { Code = start.ToString() });


            var created = GenerateFileCreatedMethod(port);
            watcher.AddMethod(new Method { Code = created });
            watcher.AddMethod(WaitForReadyCode());
            watcher.AddMethod(DisposeCode());

            return Task.FromResult(list.AsEnumerable());
        }

        private static string DisposeCode()
        {
            var code = new StringBuilder();
            code.AppendLine(@"
            public void Dispose()
            {
                m_watcher?.Dispose();
                m_watcher = null;
                m_client?.Dispose();
                m_client = null;
            }
            ");
            return code.ToString();
        }

        private string GenerateFileCreatedMethod(ReceivePort port)
        {
            var context = new SphDataContext();
            var endpoint = context.LoadOneFromSources<OperationEndpoint>(x => x.Id == this.SubmitEndpoint);
            var created = new StringBuilder();
            // TODO : options to archive the file once done
            // TODO : use stream to read those lines, or use ReadLines to read untill the next record, and POST the current record
            // TODO : Polly retry when POST to endpoint
            created.AppendLine($@" 
        private async void FswChanged(object sender, FileSystemEventArgs e)
        {{  
            var file = e.FullPath;
            var wip = file + "".wip"" ;
            await WaitReadyAsync(file);
            File.Move(file, wip );
            var port = new {port.CodeNamespace}.{port.TypeName}();
            // TODO : just read to the next record
            var lines = File.ReadLines(wip);
            var records = port.Process(lines); 
             // now post it to the server            
            foreach(var r in records)
            {{
                // polly policy goes here
                var request = new StringContent(r.ToJson());
                var response = await m_client.PostAsync(""/{endpoint.Route}"", request);
                Console.WriteLine("" "" + response.StatusCode);
            }}
            // TODO : options to archive the file
            File.Delete(wip);
        }}");
            return created.ToString();
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
            using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {{
                if (stream != null)
                {{
                    Console.WriteLine($""Output file {{fileName}} ready."");
                    break;
                }}
            }}
        }}
        catch (FileNotFoundException ex)
        {{
            Console.WriteLine($""Output file {{fileName}} not yet ready ({{ex.Message}})"");
        }}
        catch (IOException ex)
        {{
            Console.WriteLine($""Output file {{fileName}} not yet ready ({{ex.Message}})"");
        }}
        catch (UnauthorizedAccessException ex)
        {{
            Console.WriteLine($""Output file {{fileName}} not yet ready ({{ex.Message}})"");
        }}
        await Task.Delay(500);
    }}
}}"
            };
        }
    }
}