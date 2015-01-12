using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters.Properties;
using MySql.Data.MySqlClient;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SprocOperationDefinition : OperationDefinition
    {

        public string GenerateActionCode(MySqlAdapter adapter, string methodName)
        {
            var code = new StringBuilder();
            code.AppendLine(CreateMethodCode(adapter));

            var parameterList = this.RequestMemberCollection.Select(p => p.Name).ToList();
            parameterList.AddRange(this.ResponseMemberCollection.OfType<SprocResultMember>()
                .Where(p => p.Type != typeof(Array))
                .Where(p => p.Type != typeof(object))
                .Select(p => p.Name));

            var outParameterSelectList = this.ResponseMemberCollection.OfType<SprocResultMember>()
                .Where(p => p.Type != typeof(Array))
                .Where(p => p.Type != typeof(object))
                .Select(p => p.Type == typeof(string) ? p.Name : string.Format("CAST({0} AS SIGNED)", p.Name))
                .ToList();

            code.AppendFormat("           var sql =\"CALL `{0}`.`{1}`({2});\";", adapter.Schema, this.MethodName,
                string.Join(",", parameterList));
            code.AppendLine();
            if (outParameterSelectList.Any())
                code.AppendLine("           sql +=\"SELECT " + string.Join(",", outParameterSelectList) + ";\";");

            code.AppendLine();
            code.AppendLine("           using(var conn = new MySqlConnection(this.ConnectionString))");
            code.AppendLinf("           using(var cmd = new MySqlCommand(sql, conn))");
            code.AppendLine("           {");



            foreach (var m in this.RequestMemberCollection.OfType<SprocParameter>())
            {
                code.AppendLinf("               cmd.Parameters.AddWithValue(\"{0}\", request.{0});", m.Name);
            }
            foreach (var m in this.ResponseMemberCollection.OfType<SprocResultMember>())
            {
                if (m.Type == typeof(Array)) continue;
                if (m.Type == typeof(object)) continue;
                if (m.Name == "@return_value") continue;
            }
            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLinf("               var response = new {0}Response();", this.MethodName.ToCsharpIdentitfier());
            if (outParameterSelectList.Any())
            {
                code.AppendLinf("               using(var reader = await cmd.ExecuteReaderAsync())");
                code.AppendLine("               {");
                code.AppendLine("                   if(await reader.ReadAsync())");
                code.AppendLine("                   {");
                var i = 0;
                foreach (var p in this.ResponseMemberCollection.OfType<SprocResultMember>()
                .Where(p => p.Type != typeof(Array))
                .Where(p => p.Type != typeof(object)))
                {
                    code.AppendLinf("                       response.{0} = ({1})reader[{2}];", p.Name, p.Type.ToCSharp(), i);
                    i++;
                }

                code.AppendLine("                   }");
                code.AppendLine("               }");

            }
            else
            {
                var hasReader = this.ResponseMemberCollection.OfType<SprocResultMember>().Any(p => p.Type == typeof (Array));
                if (!hasReader)
                    code.AppendLine("               var row = await cmd.ExecuteNonQueryAsync();");
            }

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

                        code.AppendLinf("                       item.{0} = ({1})reader[\"{0}\"];", rm.Name, rm.Type.ToCSharp());
                    }
                    code.AppendLinf("                       response.{0}.Add(item);", m.Name);
                    code.AppendLine("                   }");
                    code.AppendLine("               }");
                    continue;
                }
                if (m.Name == "@return_value") continue;
            }

            code.AppendLine("               return response;");
            code.AppendLine("           }");
            code.AppendLine("       }");
            return code.ToString();
        }


        private string CreateMethodCode(MySqlAdapter adapter)
        {
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<{0}Response> {0}Async({0}Request request)",
                this.MethodName.ToCsharpIdentitfier());
            code.AppendLine("       {");
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
                    Console.WriteLine(Resources.DuplicationFileContent, fileName, mc == sources[fileName] ? "same" : "different");
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
            header.AppendLine("using " + typeof(MySqlConnection).Namespace + ";");
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
                    Console.WriteLine(Resources.DuplicationFileContent, fileName, mc == sources[fileName] ? "same" : "different");
                    continue;
                }
                sources.Add(member.Name + ".cs", mc);
            }

            return sources;
        }

        public string Text { get; set; }

        public void ParseParameters(string text)
        {
            const string PATTERN = @"`\((?<param>.*?)\)\n";
            var paramsText = Strings.RegexSingleValue(text, PATTERN, "param");
            if (string.IsNullOrWhiteSpace(paramsText))
            {
                Console.WriteLine(Resources.MessageNoSprocParameters);
                Console.WriteLine(text);
                return;

            }
            this.RequestMemberCollection.Clear();
            this.ResponseMemberCollection.Clear();
            foreach (var s in paramsText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var lines = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (lines[0] == "IN")
                {
                    this.RequestMemberCollection.Add(new SprocParameter { Name = "@" + lines[1], Type = lines[2].GetClrDataType() });
                }
                if (lines[0] == "OUT")
                {
                    this.ResponseMemberCollection.Add(new SprocResultMember { Name = "@" + lines[1], Type = lines[2].GetClrDataType() });
                }

            }
        }
    }
}