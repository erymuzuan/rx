﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Integrations.Adapters
{
    [EntityType(typeof(ReceiveLocation))]
    [Export("ReceiveLocationDesigner", typeof(ReceiveLocation))]
    [DesignerMetadata(FriendlyName = "Rest API", Route = "receive.location.restapi/:id", FontAwesomeIcon = "gg",
         Name = "restapi")]
    public class RestApiReceiveLocation : ReceiveLocation
    {
        public bool BufferAllRows { get; set; }
        public bool RejectPartial { get; set; }
        public string BaseAddress { get; set; }
        public string ContentType { get; set; }
        public string InboundMapping { get; set; }
        public string InboundType { get; set; }
        public string Method { get; set; }
        public string Route { get; set; }
        public bool InProcess { get; set; }
        public ObjectCollection<HttpHeader> Headers { get; } = new ObjectCollection<HttpHeader>();


        public override async Task<IEnumerable<Class>> GenerateClassesAsync(ReceivePort port)
        {
            var context = new SphDataContext();
            var ed = context.LoadOneFromSources<EntityDefinition>(x => x.Name == port.Entity);

            this.ReferencedAssemblyCollection.AddPackage("Microsoft.AspNet.WebApi.Core", "5.2.3", "net45", "System.Web.Http");
            this.ReferencedAssemblyCollection.AddPackage("Polly", "4.2.4");
            this.ReferencedAssemblyCollection.Add<WebApi.BaseApiController>();
            this.ReferencedAssemblyCollection.Add($"{ConfigurationManager.ApplicationName}.{ed.Name}");

            var list = (await base.GenerateClassesAsync(port)).ToList();
            list.Add(this.GenerateControllerClass(port));
            list.Add(this.GenerateParameterAttributeClass(port));
            list.Add(this.GenerateParameterBindingClass(port));


            return list;
        }


        protected override Class GenerateLoggerClass(ReceivePort port)
        {
            var logger = new Class { Name = "LocationLogger", Namespace = this.CodeNamespace, BaseClass = "ILogger" };
            logger.AddNamespaceImport<DomainObject, DateTime, Task>();

            var code = new StringBuilder();
            code.AppendLine($@"

        public LocationLogger(){{}}
        private ILogger m_logger;
        public System.Collections.Generic.IList<LogEntry> Entries {{get;}} = new System.Collections.Generic.List<LogEntry>();

        public LocationLogger(ILogger logger)
        {{
            m_logger = logger;
        }}

        public Task LogAsync(LogEntry entry)
        {{
            this.Entries.Add(entry);
            if(null != m_logger) m_logger.LogAsync(entry);
            return ObjectBuilder.GetObject<ILogger>().LogAsync(entry);
        }}

        public void Log(LogEntry entry)
        {{
            this.Entries.Add(entry);
            if(null != m_logger) m_logger.Log(entry);
            ObjectBuilder.GetObject<ILogger>().Log(entry);          
        }}

");

            logger.AddMethod(new Method { Code = code.ToString() });


            return logger;
        }


        private Class GenerateParameterBindingClass(ReceivePort port)
        {
            var bind = new Class { Name = $@"{port.Name}{port.Formatter}ParameterBinding", Namespace = CodeNamespace, BaseClass = "HttpParameterBinding" };
            bind.AddNamespaceImport<DateTime, DomainObject>();
            bind.AddNamespaceImport<System.Web.Http.Controllers.HttpParameterBinding, System.Web.Http.ParameterBindingAttribute, System.Web.Http.Metadata.ModelMetadataProvider>();
            bind.AddNamespaceImport<Stream, Task, CancellationToken>();



            var headers = new StringBuilder();
            foreach (var hd in port.FieldMappingCollection.OfType<HeaderFieldMapping>())
            {
                if (hd.IsNullable)
                    headers.AppendLine($@"if(actionContext.Request.Headers.Contains(""{hd.Header}""))");
                headers.AppendLine($@"port.AddHeader(""{hd.Name}"", actionContext.Request.Headers.GetValues(""{hd.Header}"").ToString("";""));");
            }

            var lineCount = this.BufferAllRows ? @"port.AddHeader(""LineCount"", $""{lines.Length}"");" : "";
            var code = new StringBuilder();
            code.AppendLine($@" 
        public {port.Name}{port.Formatter}ParameterBinding(HttpParameterDescriptor parameter) : base(parameter)
        {{
        }}

        public override async Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
            HttpActionContext actionContext, CancellationToken cancellationToken)
        {{
            if(Descriptor.ParameterName == ""logs"")return;
            var request = actionContext.Request;
            var stream = await request.Content.ReadAsStreamAsync();
            var text = GetRequestBody(stream);
            var lines = text.Split(new[] {{ ""\r\n"", ""\n"" }}, StringSplitOptions.RemoveEmptyEntries);

            var logger = new LocationLogger();
            var port = new {port.CodeNamespace}.{port.TypeName}(logger);
            port.AddHeader(""CreationTime"", $""{{DateTime.Now:s}}"");
            port.AddHeader(""DirectoryName"", $""{{actionContext.Request.RequestUri}}"");
            port.AddHeader(""Length"", $""{{actionContext.Request.Content.Headers.ContentLength}}"");
            port.AddHeader(""FullName"", $""{{actionContext.Request.RequestUri}}"");
            port.AddHeader(""Name"", actionContext.Request.Headers.GetValues(""X-Name"").ToString("";""));
            port.AddHeader(""Rx:ApplicationName"", ""{ConfigurationManager.ApplicationName}"");
            port.AddHeader(""Rx:LocationName"", ""{Name}"");
            port.AddHeader(""Rx:Type"", ""{nameof(RestApiReceiveLocation)}"");
            port.AddHeader(""Rx:MachineName"", Environment.GetEnvironmentVariable(""COMPUTERNAME""));
            port.AddHeader(""Rx:UserName"", Environment.GetEnvironmentVariable(""USERNAME""));
            port.Uri = actionContext.Request.RequestUri;
            {headers}
            {lineCount}

            var list = port.Process(lines);
            actionContext.ActionArguments[Descriptor.ParameterName] = list;
            actionContext.ActionArguments[""logs""] = logger.Entries;

        }}


        private static string GetRequestBody(Stream stream)
        {{
            if (stream.CanSeek)
                stream.Position = 0;
            using (var reader = new StreamReader(stream))
            {{
                return reader.ReadToEnd();
            }}
        }}");
            bind.AddMethod(new Method { Code = code.ToString() });
            return bind;
        }

        private Class GenerateParameterAttributeClass(ReceivePort port)
        {
            var attr = new Class { Name = $"{port.Name}{port.Formatter}BindingAttribute", Namespace = CodeNamespace, BaseClass = "ParameterBindingAttribute" };
            attr.AddNamespaceImport<System.Web.Http.Controllers.HttpParameterBinding, System.Web.Http.ParameterBindingAttribute>();
            var code = new StringBuilder();
            code.AppendLine($@" public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {{
            return new {port.Name}{port.Formatter}ParameterBinding(parameter);
        }}");
            attr.AddMethod(new Method { Code = code.ToString() });
            return attr;
        }

        private Class GenerateControllerClass(ReceivePort port)
        {
            var route = this.Route.StartsWith("~/") ? this.Route.Replace("~/", "") : this.Route;
            var controller = new Class { Name = $"{Name}Controller", Namespace = CodeNamespace, BaseClass = "BaseApiController" };
            controller.AddNamespaceImport<DateTime, FileInfo, DomainObject, IEnumerable<object>>();
            controller.AddNamespaceImport<Task, System.Web.Http.ApiController, WebApi.BaseApiController>();
            controller.ImportCollection.AddRange("System.Linq");
            controller.AttributeCollection.Add($@"[RoutePrefix(""{route}"")]");

            var context = new SphDataContext();
            var ed = context.LoadOneFromSources<EntityDefinition>(x => x.Name == port.Entity);

            var reject = "";
            if (this.RejectPartial)
            {
                reject =
                $@"if(errors.Count > 0)
                {{
                    return Invalid((System.Net.HttpStatusCode)422, new {{ success = false, message = $""{{list.Count( x => null != x)}} lines correctly parsed but there are {{errors.Count}} lines with errors"", errors}});
                }}";
            }
            var code = new StringBuilder();
            code.AppendLine($@"   
        [HttpPost]
        [PostRoute("""")]
        public async Task<IHttpActionResult> Create(
            [{port.Name}{port.Formatter}Binding]IEnumerable<{port.CodeNamespace}.{port.Entity}> list,
            [{port.Name}{port.Formatter}Binding]IEnumerable<LogEntry> logs = null)
        {{

            var entities = (from i in list
                           where null != i
                           let json = i.ToJson()
                           select json.DeserializeFromJson<{ed.TypeName}>()).ToList();

            var errors = (from e in (logs ?? Array.Empty<LogEntry>())
                         select new 
                            {{
                                message = e.Exception?.Message.Replace(""Line: 1 "", "" ""),                                
                                details = e.Details.Substring(38, e.Details.IndexOf("" =========================="") - 38).Trim()
                            }}).ToList();
            {reject}

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {{
                foreach (var d in entities)
                {{
                    d.Id = Strings.GenerateId();
                    d.WebId = d.Id;
                    d.CreatedDate = DateTime.Now;
                    session.Attach(d);
                }}
                var headers = new Dictionary<string, object>
                {{
                    {{""CreationTime"", $""{{DateTime.Now:s}}""}},
                    {{""DirectoryName"", $""{{this.Request.RequestUri}}""}},
                    {{""Length"", $""{{this.Request.Content.Headers.ContentLength}}""}},
                    {{""FullName"", $""{{this.Request.RequestUri}}""}},
                    {{""Name"", this.Request.Headers.GetValues(""X-Name"").ToString("";"")}},
                    {{""Rx:ApplicationName"", ""{ConfigurationManager.ApplicationName}""}},
                    {{""Rx:LocationName"", ""{Name}""}},
                    {{""Rx:Type"", ""{nameof(RestApiReceiveLocation)}""}},
                    {{""Rx:MachineName"", Environment.GetEnvironmentVariable(""COMPUTERNAME"")}},
                    {{""Rx:UserName"", Environment.GetEnvironmentVariable(""USERNAME"")}}
                }};
                await session.SubmitChanges(""{SubmitEndpoint}"", headers);

            }}
            return Created($""{{ConfigurationManager.BaseUrl}}/api/rts/{{Strings.GenerateId()}}"", new {{ success = true, message = $""{{entities.Count}} lines successfully imported and {{errors.Count}} lines has errors"" ,rows = entities.Count, errors}});
        }}");
            controller.AddMethod(new Method { Code = code.ToString() });

            return controller;
        }


    }
}