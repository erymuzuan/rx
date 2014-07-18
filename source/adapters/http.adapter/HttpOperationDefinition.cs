using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class HttpOperationDefinition : OperationDefinition
    {
        public HttpOperationDefinition()
        {

        }

        public HttpOperationDefinition(JToken jt)
        {
            var postData = from p in jt.SelectTokens("request.postData.params").SelectMany(x => x)
                           select new Member
                           {
                               Name = p.SelectToken("name").Value<string>(),
                               Type = typeof(string)
                           };
            this.RequestMemberCollection.AddRange(postData);
            this.RequestHeaders = new Dictionary<string, string>();


            var headers = from p in jt.SelectTokens("request.headers").SelectMany(x => x)
                           select new 
                           {
                               name = p.SelectToken("name").Value<string>(),
                               value = p.SelectToken("value").Value<string>()
                           };
            foreach (var h in headers)
            {
                this.RequestHeaders.Add(h.name, h.value);
            }
        }


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
            header.AppendLine("using System.Web.Http;");
            header.AppendLine("using System.Net;");
            header.AppendLine("using System.Net.Http;");
            header.AppendLine();

            header.AppendLine("namespace " + this.CodeNamespace);
            header.AppendLine("{");
            return header.ToString();

        }

        public override string GenerateResponseCode()
        {
            var code = new StringBuilder();
            code.AppendLine(this.GetCodeHeader());
            code.AppendLine("   public class " + this.HttpMethod + this.Name + "Response : DomainObject");
            code.AppendLine("   {");

            code.AppendLine("       public string ResponseText{ get; private set;}");


            code.AppendLinf("       public async Task LoadAsync(HttpResponseMessage response)", this.HttpMethod, this.Name);
            code.AppendLine("       {");
            code.AppendLine("           var content = response.Content as StreamContent;");
            code.AppendLine("           if(null == content) throw new Exception(\"Fail to read from response\");");
            code.AppendLine("           this.ResponseText = await content.ReadAsStringAsync();");
            code.AppendLine("       }");

            // properties for each members
            foreach (var member in this.ResponseMemberCollection)
            {
                code.AppendLinf("       //member:{0}", member.Name);
                code.AppendLine(member.GeneratedCode());
            }


            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace
            return code.ToString();
        }

        public override string GenerateRequestCode()
        {
            var code = new StringBuilder();
            code.AppendLine(this.GetCodeHeader());
            code.AppendLine("   public class " + this.HttpMethod + this.Name + "Request : DomainObject");
            code.AppendLine("   {");


            if (this.HttpMethod != "GET")
            {

                code.AppendLine("       public string PostData");
                code.AppendLine("       {");
                code.AppendLine("           get{");

                var names = string.Join("+\"&", this.RequestMemberCollection.Select(x => x.Name + "=\" + " + x.Name));
                code.AppendLine("               return \"" + names + ";");
                code.AppendLine("           }");
                code.AppendLine("       }");
            }

            // properties for each members
            foreach (var member in this.RequestMemberCollection)
            {
                code.AppendLinf("       //member:{0}", member.Name);
                code.AppendLine(member.GeneratedCode());
            }


            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace
            return code.ToString();
        }
    }
}
