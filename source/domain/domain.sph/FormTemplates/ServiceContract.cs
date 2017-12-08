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
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Domain.Compilers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ServiceContract : DomainObject, IProjectDefinition
    {
        public Task<ServiceContractSetting> LoadSettingAsync(string entity)
        {
            if (string.IsNullOrWhiteSpace(entity)) throw new ArgumentNullException(nameof(entity), "Please set the Entity name for the Service contract");

            var cacheManager = ObjectBuilder.GetObject<ICacheManager>();
            var key = $"setting:{entity}";
            var setting = cacheManager.Get<ServiceContractSetting>(key);
            if (null != setting) return Task.FromResult(setting);

            var source = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(ServiceContractSetting)}\\{entity}.service-contract.setting.json";
            setting = File.Exists(source) ? File.ReadAllText(source).DeserializeFromJson<ServiceContractSetting>()
                : new ServiceContractSetting();
            cacheManager.Insert(key, setting, source);

            return Task.FromResult(setting);
        }
        public Task SaveSetttingAsync(ServiceContractSetting setting, string entity)
        {
            if (string.IsNullOrWhiteSpace(entity)) throw new ArgumentNullException(nameof(entity), "Please set the Entity name for the Service contract");
            var cacheManager = ObjectBuilder.GetObject<ICacheManager>();
            var key = $"setting:{entity}";
            var source = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(ServiceContractSetting)}\\{entity}.service-contract.setting.json";
            File.WriteAllText(source, setting.ToJsonString(true));
            cacheManager.Insert(key, setting, source);

            return Task.FromResult(0);
        }

        private readonly string[] m_importDirectives =
       {
            typeof(Entity).Namespace,
            typeof(Int32).Namespace ,
            typeof(Task<>).Namespace,
            typeof(List<>).Namespace,
            typeof(Enumerable).Namespace ,
            typeof(SqlCommand).Namespace,
            typeof(StringBuilder).Namespace,
            "System.Web.Http",
            "Bespoke.Sph.WebApi"
        };

        public async Task<RxCompilerResult> CompileAsync(EntityDefinition entityDefinition)
        {
            m_entityDefinition = entityDefinition;

            var controller = this.GenerateController();
            var source = controller.Save($"ServiceContract.{entityDefinition.Name}");
            var assemblyInfo = await AssemblyInfoClass.GenerateAssemblyInfoAsync(entityDefinition, autoSave: true, folder: $"ServiceContract.{entityDefinition.Name}");

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
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\System.Web.Http.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\webapi.common.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.CompilerOutputPath + $@"\{ConfigurationManager.ApplicationName}.{m_entityDefinition.Name}.dll");

                var result = provider.CompileAssemblyFromFile(parameters, source, assemblyInfo.FileName);
                var cr = new RxCompilerResult
                {
                    Result = true,
                    Output = Path.GetFullPath(parameters.OutputAssembly)
                };
                cr.Result = result.Errors.Count == 0;
                var errors = from CompilerError x in result.Errors
                             select new BuildDiagnostic(this.WebId, x.ErrorText)
                             {
                                 Line = x.Line,
                                 FileName = x.FileName
                             };
                cr.Errors.AddRange(errors);
                return cr;
            }
        }
        private EntityDefinition m_entityDefinition;
        [JsonIgnore]
        public string AssemblyName => $"{ConfigurationManager.ApplicationName}.{nameof(ServiceContract)}.{m_entityDefinition.Name}";
        [JsonIgnore]
        public string CodeNamespace { get; } = $"{ConfigurationManager.CompanyName}.{ConfigurationManager.ApplicationName}.IntegrationApis";
        [JsonIgnore]
        public string Name => $"{m_entityDefinition.Name}ServiceContract";
        [JsonIgnore]
        public string Id
        {
            get => m_entityDefinition.Id; set { }
        }

        private Class GenerateController()
        {
            var controller = new Class
            {
                Name = $"{m_entityDefinition.Name}ServiceContractController",
                IsPartial = true,
                FileName = $"{m_entityDefinition}ServiceContractController",
                BaseClass = "BaseApiController",
                Namespace = CodeNamespace
            };
            controller.ImportCollection.ClearAndAddRange(m_importDirectives);
            controller.ImportCollection.Add("System.Net");
            controller.ImportCollection.Add("System.Net.Http");
            controller.ImportCollection.Add("Newtonsoft.Json.Linq");
            controller.ImportCollection.Add(m_entityDefinition.CodeNamespace);
            controller.AttributeCollection.Add($"[RoutePrefix(\"api/{m_entityDefinition.Plural.ToIdFormat()}\")]");


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