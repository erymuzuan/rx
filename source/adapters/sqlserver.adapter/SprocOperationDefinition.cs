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
            code.AppendLinf("           using(var cmd = new SqlCommand(\"[{0}].[{1}]\", conn))", adapter.Schema, this.MethodName);
            code.AppendLine("           {");
            code.AppendLine("               cmd.CommandType = CommandType.StoredProcedure;");
            foreach (var m in this.RequestMemberCollection.OfType<SprocParameter>())
            {
                code.AppendLinf("               cmd.Parameters.AddWithValue(\"{0}\", request.{0});", m.Name);
            }
            foreach (var m in this.ResponseMemberCollection.OfType<SprocResultMember>())
            {
                if(m.Type == typeof(Array))continue;
                if(m.Type == typeof(object))continue;
                if (m.Name == "@return_value") continue;
                code.AppendLinf("               cmd.Parameters.Add(\"{0}\", SqlDbType.{1}).Direction = ParameterDirection.Output;", m.Name, m.SqlDbType);
            }
            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine("               var row = await cmd.ExecuteNonQueryAsync();");
            code.AppendLinf("               var response = new {0}Response();", this.MethodName.ToCsharpIdentitfier());
            foreach (var m in this.ResponseMemberCollection.OfType<SprocResultMember>())
            {
                if (m.Type == typeof (Array))
                {
                    code.AppendLinf("               using(var reader = await cmd.ExecuteReaderAsync())");
                    code.AppendLine("               {");
                    code.AppendLine("                   while(await reader.ReadAsync())");
                    code.AppendLine("                   {");
                    code.AppendLinf("                       var item = new {0}();",m.Name.Replace("Collection",""));
                    foreach (var rm in m.MemberCollection.OfType<SprocResultMember>())
                    {

                        code.AppendLinf("                       item.{0} = ({1})reader[\"{0}\"];", rm.Name, rm.Type.ToCSharp());
                    }
                    code.AppendLinf("                       response.{0}.Add(item);", m.Name);
                    code.AppendLine("                   }");
                    code.AppendLine("               }");
                    continue;
                }
                if (m.Name == "@return_value") continue;
                code.AppendLinf("               response.{0} = ({1})cmd.Parameters[\"{0}\"].Value;", m.Name, m.Type.ToCSharp());
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


        public override Dictionary<string, string> GenerateRequestCode()
        {
            var sources = new Dictionary<string, string>();
            var header = this.GetCodeHeader();

            var typeName = this.Name.ToCsharpIdentitfier() + "Request";
            var code = new StringBuilder();
            code.AppendLine(header);
            code.AppendLine("   public class " + typeName + " : DomainObject");
            code.AppendLine("   {");


            // properties for each members
            foreach (var member in this.RequestMemberCollection)
            {
                code.AppendLinf("       //member:{0}", member.Name);
                code.AppendLine(member.GeneratedCode());
            }


            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace

            sources.Add(typeName + ".cs", code.ToString());
            // classes for members
            foreach (var member in this.RequestMemberCollection.Where(m => m.Type == typeof(object) || m.Type == typeof(Array)))
            {
                var mc = header + member.GeneratedCustomClass() + "\r\n}";
                var fileName = member.Name + ".cs";
                if (sources.ContainsKey(fileName))
                {
                    Console.WriteLine(Resources.DuplicateContentSource, fileName, mc == sources[fileName] ? "same" : "different");
                    continue;
                }

                sources.Add(member.Name + ".cs", mc);
            }


            return sources;
        }

        private string GetCodeHeader(params string[] namespaces)
        {
            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(Int32).Namespace + ";");
            header.AppendLine("using " + typeof(Task<>).Namespace + ";");
            header.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            header.AppendLine("using " + typeof(IEnumerable<>).Namespace + ";");
            header.AppendLine("using " + typeof(SqlConnection).Namespace + ";");
            header.AppendLine("using " + typeof(CommandType).Namespace + ";");
            header.AppendLine("using " + typeof(Encoding).Namespace + ";");
            header.AppendLine("using " + typeof(XmlAttributeAttribute).Namespace + ";");
            header.AppendLine("using System.Web.Mvc;");
            header.AppendLine("using Bespoke.Sph.Web.Helpers;");
            foreach (var ns in namespaces)
            {
                header.AppendLinf("using {0};", ns);
            }
            header.AppendLine();

            header.AppendLine("namespace " + this.CodeNamespace);
            header.AppendLine("{");
            return header.ToString();

        }


        public override Dictionary<string, string> GenerateResponseCode()
        {
            var sources = new Dictionary<string, string>();
            var header = this.GetCodeHeader();

            var responseTypeName = this.Name.ToCsharpIdentitfier() + "Response";
            var code = new StringBuilder();
            code.AppendLine(header);
            code.AppendLine("   public class " + responseTypeName + " : DomainObject");
            code.AppendLine("   {");


            // properties for each members
            foreach (var member in this.ResponseMemberCollection.OfType<SprocResultMember>())
            {
                code.AppendLinf("       //member:{0}", member.Name);
                code.AppendLine(member.GeneratedCode());
            }


            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace

            sources.Add(responseTypeName + ".cs", code.ToString());


            // classes for members
            foreach (var member in this.ResponseMemberCollection.Where(m => m.Type == typeof(object) || m.Type == typeof(Array)))
            {
                var mc = header + member.GeneratedCustomClass() + "\r\n}";
                var fileName = member.Name + ".cs";
                if (sources.ContainsKey(fileName))
                {
                    Console.WriteLine("There is already file {0} with the {1} content", fileName, mc == sources[fileName] ? "same" : "different");
                    continue;
                }
                sources.Add(member.Name + ".cs", mc);
            }

            return sources;
        }

    }
}