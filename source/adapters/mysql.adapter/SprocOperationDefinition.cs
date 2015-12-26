using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Integrations.Adapters.Properties;
using MySql.Data.MySqlClient;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SprocOperationDefinition : OperationDefinition
    {

        public string GenerateActionCode(MySqlAdapter adapter, string methodName)
        {
            var name = this.MethodName.ToCsharpIdentitfier();
            var code = new StringBuilder($"       public async Task<{name}Response> {name}Async({name}Request request)");

            var parameterList = this.RequestMemberCollection.Select(p => p.Name).ToList();
            parameterList.AddRange(this.ResponseMemberCollection.OfType<SprocResultMember>()
                .Where(p => p.Type != typeof(Array))
                .Where(p => p.Type != typeof(object))
                .Select(p => p.Name));

            var outParameterSelectList = this.ResponseMemberCollection.OfType<SprocResultMember>()
                .Where(p => p.Type != typeof(Array))
                .Where(p => p.Type != typeof(object))
                .Select(p => p.Type == typeof(string) ? p.Name : $"CAST({p.Name} AS SIGNED)")
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
                code.AppendLine($"               cmd.Parameters.AddWithValue(\"{m.Name}\", request.{m.Name});");
            }
            code.AppendLine("               await conn.OpenAsync();");
            code.AppendLine($"               var response = new {MethodName.ToCsharpIdentitfier()}Response();");
            if (outParameterSelectList.Any())
            {
                code.AppendLine("               using(var reader = await cmd.ExecuteReaderAsync())");
                code.AppendLine("               {");
                code.AppendLine("                   if(await reader.ReadAsync())");
                code.AppendLine("                   {");
                var i = 0;
                foreach (var p in this.ResponseMemberCollection.OfType<SprocResultMember>()
                .Where(p => p.Type != typeof(Array))
                .Where(p => p.Type != typeof(object)))
                {
                    code.AppendLine($"                       response.{p.Name} = ({p.Type})reader[{i}];");
                    i++;
                }

                code.AppendLine("                   }");
                code.AppendLine("               }");

            }
            else
            {
                var hasReader = this.ResponseMemberCollection.OfType<SprocResultMember>().Any(p => p.Type == typeof(Array));
                if (!hasReader)
                    code.AppendLine("               var row = await cmd.ExecuteNonQueryAsync();");
            }

            foreach (var m in this.ResponseMemberCollection.OfType<SprocResultMember>())
            {
                if (m.Type != typeof (Array)) continue;
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
            }

            code.AppendLine("               return response;");
            code.AppendLine("           }");
            code.AppendLine("       }");
            return code.ToString();
        }

        


        public override IEnumerable<Class> GenerateRequestCode()
        {
            var code = new Class {Name = $"{Name.ToCsharpIdentitfier()}Request", BaseClass = nameof(DomainObject), Namespace = CodeNamespace};
            var sources = new ObjectCollection<Class> {code};


            var properties = this.RequestMemberCollection.Select(x => new Property {Code = x.GeneratedCode("   ")});
            code.PropertyCollection.AddRange(properties);

            var otherClasses = this.RequestMemberCollection
                .Select(x => x.GeneratedCustomClass(CodeNamespace, ImportDirectives))
                .SelectMany(x => x.ToArray());
            sources.AddRange(otherClasses);

            return sources;
        }

        public static readonly string[] ImportDirectives =
        {
   typeof(Entity).Namespace,
   typeof(Int32).Namespace,
   typeof(Task<>).Namespace,
   typeof(Enumerable).Namespace,
   typeof(IEnumerable<>).Namespace,
   typeof(MySqlConnection).Namespace,
   typeof(CommandType).Namespace,
   typeof(Encoding).Namespace,
   typeof(XmlAttributeAttribute).Namespace,
   "System.Web.Mvc;",
   "Bespoke.Sph.Web.Helpers"

        };


        public override IEnumerable<Class> GenerateResponseCode()
        {
            var code = new Class
            {
                Name = $"{this.Name.ToCsharpIdentitfier()}Response",
                BaseClass = nameof(DomainObject),
                Namespace = CodeNamespace
            };
            var sources = new ObjectCollection<Class> {code};


            var properties = this.ResponseMemberCollection.OfType<SprocResultMember>().Select(x => x.GeneratedCode())
                .Select(x => new Property {Code = x});
            code.PropertyCollection.AddRange(properties);



            var otherClasses = this.ResponseMemberCollection
              .Select(x => x.GeneratedCustomClass(CodeNamespace, ImportDirectives))
              .SelectMany(x => x.ToArray());
            sources.AddRange(otherClasses);
         

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