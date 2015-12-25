using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Domain.Api
{
    public partial class TableDefinition
    {
        public TableDefinition()
        {

        }

        public TableDefinition(AdapterTable table)
        {
            this.Name = table.Name;
            var tables = from a in table.ChildRelationCollection
                select new TableDefinition
                {
                    Name = a.Table,
                    CodeNamespace = this.CodeNamespace,
                    Schema = this.Schema
                };
            this.ChildTableCollection.ClearAndAddRange(tables);

        }

        private string[] GetUsingNamespaces()
        {

            return new[] {
            typeof(Entity).Namespace ,
            typeof(int).Namespace ,
            typeof(Task<>).Namespace ,
            typeof(Enumerable).Namespace ,
            typeof(JsonConvert).Namespace ,
            typeof(CamelCasePropertyNamesContractResolver).Namespace ,
            typeof(StringEnumConverter).Namespace ,
            typeof(XmlAttributeAttribute).Namespace ,
            typeof(MediaTypeFormatter).Namespace,
            "using System.Web.Http;",
            "using System.Net;",
            "using System.Net.Http;"};

        }
        private string GetCodeHeader()
        {

            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(int).Namespace + ";");
            header.AppendLine("using " + typeof(Task<>).Namespace + ";");
            header.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            header.AppendLine("using " + typeof(JsonConvert).Namespace + ";");
            header.AppendLine("using " + typeof(CamelCasePropertyNamesContractResolver).Namespace + ";");
            header.AppendLine("using " + typeof(StringEnumConverter).Namespace + ";");
            header.AppendLine("using " + typeof(XmlAttributeAttribute).Namespace + ";");
            header.AppendLine("using " + typeof(MediaTypeFormatter).Namespace + ";");
            header.AppendLine("using System.Web.Http;");
            header.AppendLine("using System.Net;");
            header.AppendLine("using System.Net.Http;");
            header.AppendLine();

            header.AppendLine("namespace " + this.CodeNamespace);
            header.AppendLine("{");
            return header.ToString();

        }

        public Dictionary<string, string> GenerateCode(Adapter adapter)
        {
            var header = this.GetCodeHeader();
            var code = new StringBuilder(header);

            if (!string.IsNullOrWhiteSpace(ClassAttribute))
                code.AppendLine("   " + ClassAttribute);

            code.AppendLine("   public class " + this.Name + " : DomainObject");
            code.AppendLine("   {");

            var pk = "\"\"";
            if (null != this.PrimaryKey)
                pk = this.PrimaryKey.Name;

            code.AppendFormat(@"     
        public override string ToString()
        {{
            return ""{0}:"" + {1};
        }}", this.Name, pk);


            // properties for each members
            foreach (var member in this.MemberCollection)
            {
                code.AppendLinf("       //member:{0}", member.Name);
                code.AppendLine(member.GeneratedCode());
            }


            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace

            var sourceCodes = new Dictionary<string, string> { { this.Name + ".cs", code.ToString() } };

            // classes for members
            foreach (var member in this.MemberCollection.Where(m => m.Type == typeof(object) || m.Type == typeof(Array)))
            {
                var classes = member.GeneratedCustomClass(this.CodeNamespace, GetUsingNamespaces());
                foreach (var @class in classes)
                {
                    if (!sourceCodes.ContainsKey(@class.FileName))
                        sourceCodes.Add(@class.FileName, @class.GetCode());
                }
            }

            var controller = this.GenerateController(adapter);
            sourceCodes.Add(this.Name + "Controller.cs", controller);


            return sourceCodes;
        }

        public string Name { get; set; }
        public string CodeNamespace { get; set; }
        public string ClassAttribute { get; set; }

        public string WebId { get; set; }
    }
}