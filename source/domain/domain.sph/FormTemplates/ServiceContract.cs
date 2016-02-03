using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class ServiceContract : DomainObject
    {
        private readonly string[] m_importDirectives =
       {
            typeof(Entity).Namespace,
            typeof(Int32).Namespace ,
            typeof(Task<>).Namespace,
            typeof(List<>).Namespace,
            typeof(Enumerable).Namespace ,
            typeof(SqlCommand).Namespace,
            typeof(StringBuilder).Namespace,
            "System.Web.Mvc",
            "Bespoke.Sph.Web.Api",
            "Bespoke.Sph.Web.Helpers"
        };

        public Task<WorkflowCompilerResult> CompileAsync(EntityDefinition entityDefinition)
        {
            m_entityDefinition = entityDefinition;

            var controller = this.GenerateController();
            var source = controller.Save($"{nameof(EntityDefinition)}.{nameof(ServiceContract)}.{m_entityDefinition.Name}.cs");


            using (var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider())
            {
                var output = $"{ConfigurationManager.CompilerOutputPath}\\{ConfigurationManager.ApplicationName}.{nameof(ServiceContract)}.{m_entityDefinition.Name}.dll";
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
                    typeof(SqlCommand),
                    typeof(HttpResponseBase),
                    typeof(ConfigurationManager));
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\System.Web.Mvc.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\core.sph.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.CompilerOutputPath + $@"\{ConfigurationManager.ApplicationName}.{m_entityDefinition.Name}.dll");

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

        public string CodeNamespace { get; } = $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.IntegrationApis";

        private Class GenerateController()
        {
            var controller = new Class
            {
                Name = $"{m_entityDefinition.Name}ServiceContractController",
                IsPartial = true,
                FileName = $"{m_entityDefinition}ServiceContractController.cs",
                BaseClass = "System.Web.Mvc.Controller",
                Namespace = CodeNamespace
            };
            controller.ImportCollection.ClearAndAddRange(m_importDirectives);
            controller.ImportCollection.Add("System.Net");
            controller.ImportCollection.Add("System.Net.Http");
            controller.ImportCollection.Add("Newtonsoft.Json.Linq");
            controller.ImportCollection.Add(m_entityDefinition.CodeNamespace);
            controller.AttributeCollection.Add($"[RoutePrefix(\"api/{m_entityDefinition.Plural.ToLowerInvariant()}\")]");


            controller.PropertyCollection.Add(new Property { Name = "CacheManager", Type = typeof(ICacheManager) });
            controller.CtorCollection.Add($"public {m_entityDefinition.Name}ServiceContractController() {{ this.CacheManager = ObjectBuilder.GetObject<ICacheManager>(); }}");
            controller.AddProperty($@"private readonly static string EntityDefinitionSource = $""{{ConfigurationManager.SphSourceDirectory}}\\{nameof(EntityDefinition)}\\{m_entityDefinition.Id}.json"";");

            if (this.FullSearchEndpoint.IsAllowed)
                controller.MethodCollection.Add(this.FullSearchEndpoint.GenerateSearchAction(m_entityDefinition));
            if (this.EntityResourceEndpoint.IsAllowed)
                controller.MethodCollection.Add(this.EntityResourceEndpoint.GenerateGetByIdCode(m_entityDefinition));
            if (this.OdataEndpoint.IsAllowed)
            {
                controller.MethodCollection.Add(this.OdataEndpoint.GenerateOdataActionCode(m_entityDefinition));
                controller.MethodCollection.Add(this.OdataEndpoint.GetOtherMethods());
            }

            return controller;


        }
    }
}