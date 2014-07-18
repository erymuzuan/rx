using System;
using System.Collections.Generic;
using Bespoke.Sph.Domain;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
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

            // post data
            var postData = from p in jt.SelectTokens("request.postData.params").SelectMany(x => x)
                           select new Member
                           {
                               Name = p.SelectToken("name").Value<string>(),
                               Type = typeof(string)
                           };
            this.RequestMemberCollection.AddRange(postData);

            // for multipart/form-data
            var mimeType = jt.SelectToken("request.postData.mimeType");
            if (null != mimeType && mimeType.Value<string>() == "multipart/form-data")
            {
                var text = jt.SelectToken("request.postData.text").Value<string>();
                var formFields = from f in Strings.RegexValues(text, @"name=\""(?<fname>.*?)\""", "fname")
                                 select new Member
                           {
                               Name = f,
                               Type = typeof(string)
                           };
                this.RequestMemberCollection.AddRange(formFields);

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


                code.AppendLine("       public string PostData");
                code.AppendLine("       {");
                code.AppendLine("           get{");
                code.AppendLine(PostDataCodeGenerator.Create(this).GenerateCode(this));
                code.AppendLine("           }");
                code.AppendLine("       }");
            

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

    public class PostDataCodeGenerator
    {
        public virtual string GenerateCode(HttpOperationDefinition operation)
        {
            return "return string.Empty;";
        }

        public static PostDataCodeGenerator Create(HttpOperationDefinition operation)
        {
            if (operation.HttpMethod == "GET")
                return new PostDataForGet();
            if (operation.RequestMemberCollection.Count == 0)
                return new PostDataCodeGenerator();
            if(operation.HttpMethod == "POST" && operation.RequestHeaders["Content-Type"].Contains("multipart"))
                return new PostDataForPostMultipartEncoded();

            if(operation.HttpMethod == "POST")
                return new PostDataForPostUrlEncoded();

            return null;
        }
    }

    public class PostDataForGet : PostDataCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            return "return string.Empty;";
        }
    }
    public class PostDataForPostUrlEncoded : PostDataCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            var names = string.Join(" + \"&", operation.RequestMemberCollection.Select(x => x.Name + "=\" + " + x.Name));
            return "               return \"" + names + ";";
        }
    }
    public class PostDataForPostMultipartEncoded : PostDataCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            var contentType = operation.RequestHeaders["Content-Type"];
            var boundary = Strings.RegexSingleValue(contentType, "boundary=(?<boundary>.*?)$", "boundary");

            var code = new StringBuilder("return string.Format(@\"");
            var count = 0;
            foreach (var member in operation.RequestMemberCollection)
            {
                code.AppendLine("--" + boundary);
                code.AppendFormat("Content-Disposition: form-data; name=\"\"{0}\"\"", member.Name);
                code.AppendLine();
                code.AppendLine();
                code.AppendLinf("{{{0}}}", count++);
                
            }
            code.AppendLine("--" + boundary + "--\",");

            var names = string.Join(",", operation.RequestMemberCollection.Select(x => x.Name ));
            code.AppendLine(names);
            code.AppendLine(");");

            return code.ToString();

        }
    }
}
