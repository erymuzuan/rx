using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SprocOperationDefinition : OperationDefinition
    {
        /// <summary>
        /// When the sproc is safe and idempotent , we could use GET
        /// </summary>
        public bool UseHttpGet { get; set; }

        public string GenerateApiCode(SqlServerAdapter adapter)
        {
            if (this.UseHttpGet)
            {
                return GenerateGetApiCode(adapter);
            }
            var code = new StringBuilder();
            var action = this.MethodName.ToCsharpIdentitfier();
            code.AppendLine("[HttpPost]");
            code.AppendLine($@"[Route(""{this.MethodName.ToIdFormat()}"")]");
            code.AppendLine($"public async Task<IHttpActionResult> {action}([FromBody]{action}Request  request)");
            code.AppendLine("{");

            code.AppendLine($@"
                                var adapter = new {adapter.Name}();
                                var response = await adapter.{action}Async(request);
                                return Ok(response);");
            code.AppendLine("}");
            return code.ToString();
        }

        private string GenerateGetApiCode(SqlServerAdapter adapter)
        {
            var code = new StringBuilder();
            var action = this.MethodName.ToCsharpIdentitfier();
            var parameters = this.RequestMemberCollection.ToString(", ", x => $@"[FromUri(Name=""{x.Name}"")]" + x.GenerateParameterCode());
            var routesParamters = this.RequestMemberCollection.ToString("/", x => "{" + x.Name + "}");

            code.AppendLine("[HttpGet]");
            code.AppendLine($@"[Route(""{this.MethodName.ToIdFormat()}/{routesParamters}"")]");
            code.AppendLine($"public async Task<IHttpActionResult> {action}({parameters})");
            code.AppendLine("{");

            code.AppendLine($"   var request = new {action}Request();");
            var values = this.RequestMemberCollection.ToString("\r\n", x => $"request.{x.Name} = {x.Name.ToCamelCase()};");
            code.AppendLine(values);

            code.AppendLine($@"
                                var adapter = new {adapter.Name}();
                                var response = await adapter.{action}Async(request);
                                return Ok(response);");
            code.AppendLine("}");
            return code.ToString();
        }

        public string GenerateActionCode(SqlServerAdapter adapter, string methodName)
        {
            var code = new StringBuilder();
            code.AppendLine(CreateMethodCode());


            code.AppendLine("           using(var conn = new SqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new SqlCommand(\"[{0}].[{1}]\", conn))", adapter.Schema,
                this.MethodName);
            code.AppendLine("           {");
            code.AppendLine("               cmd.CommandType = CommandType.StoredProcedure;");
            foreach (var m in this.RequestMemberCollection.OfType<SprocParameter>())
            {
                code.AppendLinf("               cmd.Parameters.AddWithValue(\"{0}\", request.{0});", m.Name);
            }
            foreach (var m in this.ResponseMemberCollection.OfType<SprocResultMember>())
            {
                if (m.Type == typeof(Array)) continue;
                if (m.Type == typeof(object)) continue;
                if (m.Name == "@return_value") continue;
                code.AppendLinf(
                    "               cmd.Parameters.Add(\"{0}\", SqlDbType.{1}).Direction = ParameterDirection.Output;",
                    m.Name, m.SqlDbType);
            }
            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               var row = await cmd.ExecuteNonQueryAsync();");
            code.AppendLinf("               var response = new {0}Response();", this.MethodName.ToCsharpIdentitfier());
            foreach (var m in this.ResponseMemberCollection)
            {
                var cm = m as ComplexMember;
                if (null != cm && cm.AllowMultiple)
                {
                    code.AppendLinf("               using(var reader = await cmd.ExecuteReaderAsync())");
                    code.AppendLine("               {");
                    code.AppendLine("                   while(await reader.ReadAsync())");
                    code.AppendLine("                   {");
                    code.AppendLine($"                       var item = new {cm.TypeName}();");
                    var readerCodes = m.MemberCollection.OfType<SprocResultMember>()
                                    .Select(x => x.GenerateReaderCode())
                                    .Where(x => !string.IsNullOrWhiteSpace(x))
                                    .ToString("\r\n");
                    code.AppendLine(readerCodes);
                    code.AppendLinf("                       response.{0}.Add(item);", m.Name);
                    code.AppendLine("                   }");
                    code.AppendLine("               }");
                    continue;
                }
                if (m.Name == "@return_value") continue;
                var srm = m as SprocResultMember;
                if (null != srm)
                    code.AppendLinf("               response.{0} = ({1})cmd.Parameters[\"{0}\"].Value;", m.Name, srm.Type.ToCSharp());
            }

            code.AppendLine("               return response;");
            code.AppendLine("           }");
            code.AppendLine("       }");
            return code.ToString();
        }


        private string CreateMethodCode()
        {
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<{0}Response> {0}Async({0}Request request)",
                this.MethodName.ToCsharpIdentitfier());
            code.AppendLine("       {");
            code.AppendLinf("           const string SPROC = \"{0}\";", this.MethodName);

            return code.ToString();
        }


        public override IEnumerable<Class> GenerateRequestCode()
        {
            var @class = new Class { Name = this.Name.ToCsharpIdentitfier() + "Request", BaseClass = nameof(DomainObject), Namespace = CodeNamespace };
            @class.AddNamespaceImport<DateTime, DomainObject>();
            var sources = new ObjectCollection<Class> { @class };

            var properties = from m in this.RequestMemberCollection
                             select new Property { Code = m.GeneratedCode("   ") };
            @class.PropertyCollection.ClearAndAddRange(properties);


            var otherClasses = this.RequestMemberCollection
                            .Select(m => m.GeneratedCustomClass(this.CodeNamespace, m_importDirectives))
                            .SelectMany(x => x.ToArray());
            sources.AddRange(otherClasses);

            return sources;

        }

        private readonly string[] m_importDirectives =
        {
            typeof (Entity).Namespace ,
            typeof (Int32).Namespace ,
            typeof (Task<>).Namespace,
            typeof (Enumerable).Namespace ,
            typeof (IEnumerable<>).Namespace ,
            typeof (SqlConnection).Namespace ,
            typeof (CommandType).Namespace ,
            typeof (Encoding).Namespace ,
            typeof (XmlAttributeAttribute).Namespace ,
            "System.Web.Mvc",
            "Bespoke.Sph.Web.Helpers"

        };


        public override IEnumerable<Class> GenerateResponseCode()
        {
            var @class = new Class { Name = this.Name.ToCsharpIdentitfier() + "Response", Namespace = CodeNamespace };
            @class.AddNamespaceImport<DateTime, DomainObject>();
            var sources = new ObjectCollection<Class> { @class };

            var properties = from m in this.ResponseMemberCollection
                             select new Property { Code = m.GeneratedCode("   ") };
            @class.PropertyCollection.ClearAndAddRange(properties);


            var otherClasses = this.ResponseMemberCollection
                            .Select(m => m.GeneratedCustomClass(this.CodeNamespace, m_importDirectives))
                            .SelectMany(x => x.ToArray());
            sources.AddRange(otherClasses);

            return sources;
        }

    }
}