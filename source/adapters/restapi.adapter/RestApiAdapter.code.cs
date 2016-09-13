using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class RestApiAdapter
    {
        protected override Task<IEnumerable<Class>> GenerateSourceCodeAsync(CompilerOptions options,
            params string[] namespaces)
        {
            options.AddReference(typeof(Microsoft.CSharp.RuntimeBinder.Binder));
            var sources = new ObjectCollection<Class>();
            var adapterClass = new Class { Name = Name, Namespace = CodeNamespace };
            adapterClass.AddNamespaceImport<DateTime, DomainObject, SqlConnection, CommandType>();
            adapterClass.AddNamespaceImport<Task, HttpClient, JsonSerializerSettings>();
            var ope = this.OperationDefinitionCollection.OfType<RestApiOperationDefinition>().First();
            var uri = new Uri(ope.BaseAddress);
            var baseAddress = $"{uri.Scheme}://{uri.Host}:{uri.Port}";
            adapterClass.AddProperty($@"public string BaseAddress => ConfigurationManager.GetEnvironmentVariable(""{Name}_BaseAddress"") ?? ""{baseAddress}"";");
            adapterClass.AddProperty(@"private HttpClient m_client;");
            
            adapterClass.CtorCollection.Add($@"
            public {Name}()
            {{
                m_client = new HttpClient {{BaseAddress = new System.Uri(this.BaseAddress)}};
            }}

");

            var addedActions = new List<string>();
            foreach (var op in this.OperationDefinitionCollection)
            {
                op.CodeNamespace = this.CodeNamespace;
                var methodName = op.MethodName;

                if (addedActions.Contains(methodName)) continue;
                addedActions.Add(methodName);
                //
                adapterClass.AddMethod(op.GenerateActionCode(this));

                var requestSources = op.GenerateRequestCode();
                sources.AddRange(requestSources);

                var responseSources = op.GenerateResponseCode();
                sources.AddRange(responseSources);
            }
            sources.Add(adapterClass);

            return Task.FromResult(sources.AsEnumerable());
        }

        protected override Task<Class> GeneratePagingSourceCodeAsync()
        {
            var tcs = new TaskCompletionSource<Class>();
            tcs.SetResult(null);
            return tcs.Task;
        }

        protected override Task<TableDefinition> GetSchemaDefinitionAsync(string table)
        {
            var tcs = new TaskCompletionSource<TableDefinition>();
            tcs.SetResult(null);
            return tcs.Task;
        }
    }
}