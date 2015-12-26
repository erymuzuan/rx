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
using Bespoke.Sph.Integrations.Adapters.Properties;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SprocOperationDefinition : OperationDefinition
    {

        public string GenerateActionCode(SqlServerAdapter adapter, string methodName)
        {
            var code = new StringBuilder();
            code.AppendLine(CreateMethodCode(adapter));


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
            foreach (var m in this.ResponseMemberCollection.OfType<SprocResultMember>())
            {
                if (m.Type == typeof(Array))
                {
                    code.AppendLinf("               using(var reader = await cmd.ExecuteReaderAsync())");
                    code.AppendLine("               {");
                    code.AppendLine("                   while(await reader.ReadAsync())");
                    code.AppendLine("                   {");
                    code.AppendLinf("                       var item = new {0}();", m.Name.Replace("Collection", ""));
                    foreach (var rm in m.MemberCollection.OfType<SprocResultMember>())
                    {

                        code.AppendLinf("                       item.{0} = ({1})reader[\"{0}\"];", rm.Name,
                            rm.Type.ToCSharp());
                    }
                    code.AppendLinf("                       response.{0}.Add(item);", m.Name);
                    code.AppendLine("                   }");
                    code.AppendLine("               }");
                    continue;
                }
                if (m.Name == "@return_value") continue;
                code.AppendLinf("               response.{0} = ({1})cmd.Parameters[\"{0}\"].Value;", m.Name,
                    m.Type.ToCSharp());
            }

            code.AppendLine("               return response;");
            code.AppendLine("           }");
            code.AppendLine("       }");
            return code.ToString();
        }


        private string CreateMethodCode(SqlServerAdapter adapter)
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


            var @class = new Class { Name = this.Name.ToCsharpIdentitfier() + "Response", BaseClass = nameof(DomainObject), Namespace = CodeNamespace };
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
            var @class = new Class { Name = this.Name.ToCsharpIdentitfier() + "Response", BaseClass = nameof(DomainObject), Namespace = CodeNamespace };
            var sources = new ObjectCollection<Class> { @class };

            var properties = from m in this.ResponseMemberCollection.OfType<SprocResultMember>()
                             select new Property { Code = m.GeneratedCode("   ") };
            @class.PropertyCollection.ClearAndAddRange(properties);


            var otherClasses =   this.ResponseMemberCollection
                            .Select(m => m.GeneratedCustomClass(this.CodeNamespace, m_importDirectives))
                            .SelectMany(x => x.ToArray());
            sources.AddRange(otherClasses);
            
            return sources;
        }

    }
}