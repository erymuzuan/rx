using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{

   
    [EntityType(typeof(ReceiveLocation))]
    [Export("ReceiveLocationDesigner", typeof(ReceiveLocation))]
    [DesignerMetadata(FriendlyName = "File drop", FontAwesomeIcon = "folder-open-o", Route = "receive.location.folder/:id", Name = "folder")]
    public partial class FolderReceiveLocation : ReceiveLocation
    {
        protected override Task InitializeServiceClassAsync(Class watcher, ReceivePort port)
        {
            watcher.AddNamespaceImport<FileSystemWatcher, HttpClient, Uri, DomainObject>();
            watcher.AddNamespaceImport<List<object>, Task>();
            watcher.AddProperty("private FileSystemWatcher m_watcher; ");
            watcher.AddProperty("private HttpClient m_client; ");
            watcher.AddProperty("private bool m_paused = false; ");

            var created = GenerateFileCreatedMethod(port);
            watcher.AddMethod(new Method { Code = created });
            watcher.AddMethod(WaitForReadyCode());
            watcher.AddMethod(DisposeCode());

            return Task.FromResult(0);

        }

        protected override Task<Method> GenerateStartMethod(ReceivePort port)
        {
            var start = new StringBuilder();
            start.AppendLine("public bool Start()");
            start.AppendLine("{");
            start.AppendLine($@"       
                        m_client = new HttpClient();                        
                        m_client.BaseAddress = new Uri(ConfigurationManager.BaseUrl);
                        m_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(""Bearer"", ""{JwtToken}"");

                        var m_watcher = new FileSystemWatcher({Path.ToVerbatim()}, {Filter.ToVerbatim()});
                        m_watcher.EnableRaisingEvents = true;
                        m_watcher.Created += FswChanged;

                        return true;
                        
");
            start.AppendLine("}");
            return Task.FromResult(new Method { Code = start.ToString() });

        }

        protected override Task<Method> GenerateStopMethod(ReceivePort port)
        {
            return Task.FromResult(new Method {Code = "public bool Stop(){ this.Dispose(); return true;}"});
        }

        protected override Task<Method> GeneratePauseMethod(ReceivePort port)
        {
            var code = new StringBuilder();
            code.AppendLine("public void Pause(){");
            code.AppendLine("m_paused = true;");
            code.AppendLine("}");
            return Task.FromResult(new Method {Code = code.ToString()});
        }
        protected override Task<Method> GenerateResumeMethod(ReceivePort port)
        {
            var code = new StringBuilder();
            code.AppendLine("public void Resume(){");
            code.AppendLine("m_paused = false;");
            code.AppendLine("}");
            return Task.FromResult(new Method {Code = code.ToString()});
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
            var number = 0;
            var lines = File.ReadLines(wip);
            var records = port.Process(lines); 
             // now post it to the server
            Console.WriteLine($""Reading file {{file}}"");            
            foreach(var r in records)
            {{
                number ++;
                // polly policy goes here
                var request = new StringContent(r.ToJson());
                request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(""application/json"");
                var response = await m_client.PostAsync(""/api/{endpoint.Resource}/{endpoint.Route}"", request);
                Console.Write($""\r{{number}} : "" + response.StatusCode);
            }}
            // TODO : options to archive the file
            Console.WriteLine();
            Console.WriteLine($""Done processing {{file}}""); 
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