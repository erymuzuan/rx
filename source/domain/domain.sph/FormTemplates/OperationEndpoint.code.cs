using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class OperationEndpoint : ICompilationUnit
    {
        private readonly string[] m_importDirectives =
       {
            typeof(Entity).Namespace,
            typeof(Int32).Namespace ,
            typeof(Task<>).Namespace,
            typeof(List<>).Namespace,
            typeof(Enumerable).Namespace ,
            typeof(XmlAttributeAttribute).Namespace,
            "System.Web.Http",
            "Bespoke.Sph.WebApi"
        };

        public Task<WorkflowCompilerResult> CompileAsync(EntityDefinition entityDefinition)
        {
            m_entityDefinition = entityDefinition;

            var controller = this.GenerateController();
            var source = controller.Save($"{nameof(OperationEndpoint)}.{Entity}.{Name}");


            using (var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider())
            {
                var output = $"{ConfigurationManager.CompilerOutputPath}\\{AssemblyName}";
                var parameters = new CompilerParameters
                {
                    OutputAssembly = output,
                    GenerateExecutable = false,
                    IncludeDebugInformation = true

                };

                parameters.AddReference(typeof(Entity),
                    typeof(int),
                    typeof(INotifyPropertyChanged),
                    typeof(Expression<>),
                    typeof(XmlAttributeAttribute),
                    typeof(SmtpClient),
                    typeof(HttpClient),
                    typeof(XElement),
                    typeof(HttpResponseBase),
                    typeof(ConfigurationManager));
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\System.Web.Http.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\webapi.common.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.CompilerOutputPath + $@"\{ConfigurationManager.ApplicationName}.{Entity}.dll");

                var result = provider.CompileAssemblyFromFile(parameters, source);
                var cr = new WorkflowCompilerResult
                {
                    Result = true,
                    Output = Path.GetFullPath(parameters.OutputAssembly)
                };
                cr.Result = result.Errors.Count == 0;
                var errors = from CompilerError x in result.Errors
                             select new BuildError(this.WebId, x.ErrorText)
                             {
                                 Line = x.Line,
                                 FileName = x.FileName
                             };
                cr.Errors.AddRange(errors);
                return Task.FromResult(cr);
            }
        }
        private EntityDefinition m_entityDefinition;
        [JsonIgnore]
        public string CodeNamespace => $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.IntegrationApis";

        [JsonIgnore]
        public string AssemblyName => $"{ConfigurationManager.ApplicationName}.OperationEndpoint.{Entity}.{Name}.dll";
        [JsonIgnore]
        public string PdbName => $"{ConfigurationManager.ApplicationName}.OperationEndpoint.{Entity}.{Name}.pdb";
        [JsonIgnore]
        public string TypeName => $"{Entity}{Name}OperationEndpointController";
        [JsonIgnore]
        public string TypeFullName => $"{CodeNamespace}.{TypeName}, {AssemblyName.Replace(".dll", "")}";

        private Class GenerateController()
        {
            var controller = new Class
            {
                Name = TypeName,
                IsPartial = true,
                FileName = TypeName,
                BaseClass = "BaseApiController",
                Namespace = CodeNamespace
            };
            controller.ImportCollection.ClearAndAddRange(m_importDirectives);
            controller.ImportCollection.Add("System.Net");
            controller.ImportCollection.Add("System.Net.Http");
            controller.ImportCollection.Add("Newtonsoft.Json.Linq");
            controller.ImportCollection.Add(m_entityDefinition.CodeNamespace);
            controller.AttributeCollection.Add($"[RoutePrefix(\"api/{Resource}\")]");


            controller.PropertyCollection.Add(new Property { Name = "CacheManager", Type = typeof(ICacheManager) });
            controller.CtorCollection.Add($"public {TypeName}() {{ this.CacheManager = ObjectBuilder.GetObject<ICacheManager>(); }}");
            controller.AddProperty($@"private readonly static string EntityDefinitionSource = $""{{ConfigurationManager.SphSourceDirectory}}\\{nameof(EntityDefinition)}\\{m_entityDefinition.Id}.json"";");
            controller.AddProperty($@"private readonly static string EndpointSource = $""{{ConfigurationManager.SphSourceDirectory}}\\{nameof(OperationEndpoint)}\\{Id}.json"";");

            if (this.IsHttpPost)
                controller.MethodCollection.Add(this.GeneratePostAction(m_entityDefinition));
            if (this.IsHttpPatch)
                controller.MethodCollection.Add(this.GeneratePatchAction(m_entityDefinition));

            if (this.IsHttpPut)
                controller.MethodCollection.Add(this.GeneratePutAction(m_entityDefinition));

            if (this.IsHttpDelete)
                controller.MethodCollection.Add(this.GenerateDeleteAction(m_entityDefinition));


            return controller;


        }

    }
}
