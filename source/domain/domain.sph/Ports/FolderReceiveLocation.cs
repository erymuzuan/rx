﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
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
        protected override bool GenerateExecutable()
        {
            return true;
        }

        public override async Task<IEnumerable<Class>> GenerateClassesAsync(ReceivePort port)
        {
            var list = (await base.GenerateClassesAsync(port)).ToList();
            var program = new Class { Name = "Program", Namespace = CodeNamespace };
            program.AddNamespaceImport<DateTime, FileInfo, DomainObject>();
            program.ImportCollection.AddRange("System.Linq", "Topshelf");

            if (!this.ReferencedAssemblyCollection.Any(x => x.Location.EndsWith("Topshelf.dll")))
            {
                this.ReferencedAssemblyCollection.AddPackage("Topshelf", "4.0.2", "net452");
                this.ReferencedAssemblyCollection.AddPackage("NLog", "4.3.9");
                this.ReferencedAssemblyCollection.AddPackage("Topshelf.NLog", "4.0.2", "net452");
                this.ReferencedAssemblyCollection.AddPackage("Polly", "4.2.4");
            }
            list.Add(program);

            var code = new StringBuilder();
            code.AppendLine($@"   
        public static void Main(string[] args)
        {{
            HostFactory.Run(config =>
            {{
                config.UseNLog();
                config.Service<IReceiveLocation>(svc =>
                {{
                    svc.ConstructUsing(() => new {TypeName}());

                    svc.WhenStarted(x => x.Start());
                    svc.WhenStopped(x => x.Stop());
                    svc.WhenPaused(x => x.Pause());
                    svc.WhenContinued(x => x.Resume());
                    svc.WhenShutdown(x => x.Stop());
                    svc.WhenCustomCommandReceived((x, g, i) =>
                    {{
                        Console.WriteLine(g);
                        Console.WriteLine(i);
                    }});
                }});
                config.SetServiceName(""RxFileDropLocation{Name}"");
                config.SetDisplayName(""Rx Receive Location {Name}"");
                config.SetDescription($""Rx Developer receive location for {Name}"");

                config.StartAutomatically();
            }});

        }}");
            program.AddMethod(new Method { Code = code.ToString() });

         

            return list;
        }


        protected override Task InitializeServiceClassAsync(Class watcher, ReceivePort port)
        {
            watcher.AddNamespaceImport<FileSystemWatcher, HttpClient, Uri, DomainObject>();
            watcher.AddNamespaceImport<List<object>, Task, Polly.Policy>();

            watcher.AddProperty("private FileSystemWatcher m_watcher; ");
            watcher.AddProperty("private HttpClient m_client; ");
            watcher.AddProperty("private bool m_paused = false; ");

            var created = GenerateFileCreatedMethod(port);
            watcher.AddMethod(new Method { Code = created });
            watcher.AddMethod(WaitForReadyCode());

            return Task.FromResult(0);

        }

        protected override Task<Method> GenerateStartMethod(ReceivePort port)
        {
            var start = new StringBuilder();
            start.AppendLine("public bool Start()");
            start.AppendLine("{");
            start.AppendLine($@"       
                        var token = ConfigurationManager.GetEnvironmentVariable(""{Name}_JwtToken"") ?? {JwtToken.ToVerbatim()};
                        m_client = new HttpClient();                        
                        m_client.BaseAddress = new Uri(ConfigurationManager.BaseUrl);
                        m_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(""Bearer"", token);

                        var path = ConfigurationManager.GetEnvironmentVariable(""{Name}_Path"") ?? {Path.ToVerbatim()};
                        m_watcher = new FileSystemWatcher(path, {Filter.ToVerbatim()});
                        m_watcher.EnableRaisingEvents = true;
                        m_watcher.Created += FswChanged;

                        return true;
                        
");
            start.AppendLine("}");
            return Task.FromResult(new Method { Code = start.ToString() });

        }

        protected override Task<Method> GenerateStopMethod(ReceivePort port)
        {
            return Task.FromResult(new Method { Code = "public bool Stop(){ this.Dispose(); return true;}" });
        }

        protected override Task<Method> GeneratePauseMethod(ReceivePort port)
        {
            var code = new StringBuilder();
            code.AppendLine("public void Pause(){");
            code.AppendLine("m_paused = true;");
            code.AppendLine("}");
            return Task.FromResult(new Method { Code = code.ToString() });
        }
        protected override Task<Method> GenerateResumeMethod(ReceivePort port)
        {
            var code = new StringBuilder();
            code.AppendLine("public void Resume(){");
            code.AppendLine("m_paused = false;");
            code.AppendLine("}");
            return Task.FromResult(new Method { Code = code.ToString() });
        }

        protected override Task<Method> GenerateDisposeMethod(ReceivePort port)
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
            var method = new Method {Code = code.ToString()};
            return Task.FromResult(method);
        }



        protected override Class GenerateLoggerClass(ReceivePort port)
        {
            var logger = new Class { Name = "LocationLogger", Namespace = this.CodeNamespace , BaseClass = "ILogger"};
            logger.AddNamespaceImport<DomainObject, DateTime, Task>();
            logger.ImportCollection.Add("Topshelf.Logging");

            
            var code = new StringBuilder();
            
            code.AppendLine($@"
        public Task LogAsync(LogEntry entry)
        {{
            this.Log(entry);
            return Task.FromResult(0);
        }}
        public void Log(LogEntry entry)
        {{
            var logger = HostLogger.Get<{this.TypeName}>();
            switch(entry.Severity)
            {{
                case Severity.Critical: logger.Fatal(entry.Message);break;
                case Severity.Error: logger.Error(entry.Details, entry.Exception);break;
                case Severity.Warning: logger.Warn(entry.Message);break;
                case Severity.Log:
                case Severity.Info: logger.Info(entry.Message);break;
                case Severity.Verbose: logger.Debug(entry.Message);break;
                case Severity.Debug: logger.Debug(entry.Message);break;
            }}
            
        }}

");
            
            logger.AddMethod(new Method {Code =  code.ToString()});


            return logger;
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

            var logger = new LocationLogger();
            var port = new {port.CodeNamespace}.{port.TypeName}(logger);
            port.Uri = new System.Uri(file);  

            var fileInfo = new System.IO.FileInfo(file);          
            port.AddHeader(""CreationTime"", $""{{fileInfo.CreationTime:s}}"");
            port.AddHeader(""DirectoryName"", fileInfo.DirectoryName);
            port.AddHeader(""Exists"", $""{{fileInfo.Exists}}"" );
            port.AddHeader(""Length"", $""{{fileInfo.Length}}"" );
            port.AddHeader(""Extension"", fileInfo.Extension);
            port.AddHeader(""Attributes"", $""{{fileInfo.Attributes}}"" );
            port.AddHeader(""FullName"", fileInfo.FullName );
            port.AddHeader(""Name"", fileInfo.Name);
            port.AddHeader(""LastWriteTime"", $""{{fileInfo.LastWriteTime:s}}"");
            port.AddHeader(""LastAccessTime"", $""{{fileInfo.LastAccessTime:s}}"");
            port.AddHeader(""IsReadOnly"", $""{{fileInfo.IsReadOnly}}"");
            port.AddHeader(""Rx:ApplicationName"", ""{ConfigurationManager.ApplicationName}"");
            port.AddHeader(""Rx:LocationName"", ""{Name}"");
            port.AddHeader(""Rx:Type"", ""FolderReceiveLocation"");
            port.AddHeader(""Rx:MachineName"", System.Environment.GetEnvironmentVariable(""COMPUTERNAME""));
            port.AddHeader(""Rx:UserName"", System.Environment.GetEnvironmentVariable(""USERNAME""));



            File.Move(file, wip );
            await WaitReadyAsync(wip);
            await Task.Delay(100);

            // TODO : just read to the next record
            var number = 0;
            var lines = File.ReadLines(wip);
            var records = port.Process(lines); 
             // now post it to the server
            Console.WriteLine($""Reading file {{file}}"");            
            foreach(var r in records)
            {{
                number ++;
                if(null == r) continue; // we got an exception reading the record
                var request = new StringContent(r.ToJson());
                request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(""application/json"");

                // polly policy goes here
                var retry = ConfigurationManager.GetEnvironmentVariableInt32(""{Name}RetryCount"", 3);
                var interval = ConfigurationManager.GetEnvironmentVariableInt32(""{Name}RetryInterval"", 500);

                var pr = await Policy.Handle<Exception>()
                                    .WaitAndRetryAsync(retry, c => TimeSpan.FromMilliseconds(interval * Math.Pow(2, c)))
                                    .ExecuteAndCaptureAsync(async () =>  await m_client.PostAsync(""/api/{endpoint.Resource}/{endpoint.Route}"", request));

                if (null != pr.FinalException)
                {{
                    logger.Log(new LogEntry(pr.FinalException){{Message =$""Line {{number}}""}} );
                    continue;
                }}
                var response = pr.Result;
                Console.Write($""\r{{number}} : {{response.StatusCode}}\t"");
                logger.Log(new LogEntry{{Message = $""Record: {{number}}({{r.GetLineNumber()}}) , StatusCode: {{(int)response.StatusCode}}"" , Severity = Severity.Debug}});

                if (!response.IsSuccessStatusCode)
                {{
                    var warn = new LogEntry{{ Message = $""Non success status code record {{number}}({{r.GetLineNumber()}}), StatusCode: {{(int)response.StatusCode}}"", Severity = Severity.Warning}};
                    warn.Details = $""{{fileInfo.FullName}}:{{number}}({{r.GetLineNumber()}})"";
                    logger.Log(warn);
                    // TODO : we know the use might attempt to resubmit the non 2XX response                
                }}
            }}


            Console.WriteLine();
            logger.Log(new LogEntry{{ Message = $""Done processing {{file}} with {{number}} record(s)"", Severity = Severity.Info }});

            var archivedFolder = ConfigurationManager.GetEnvironmentVariable(""{Name}ArchiveLocation"");
            if (!string.IsNullOrWhiteSpace(archivedFolder) && Directory.Exists(archivedFolder))
                File.Move(wip, Path.Combine(archivedFolder, Path.GetFileName(file)));
            else
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