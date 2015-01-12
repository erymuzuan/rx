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
    public partial class OperationDefinition
    {

        private string GetCodeHeader()
        {

            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(Int32).Namespace + ";");
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

            code.AppendLine("   public class " + this.Name + " : DomainObject");
            code.AppendLine("   {");



            code.AppendLinf("       public async Task {0}({1} request)", this.MethodName);
            code.AppendLine("       {");

            code.AppendLine("       }");


            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace

            var sourceCodes = new Dictionary<string, string> { { this.Name + ".cs", code.ToString() } };

            // classes for parameters
            
       



            return sourceCodes;
        }

    }
}
