using System;
using System.Collections.Generic;
using Bespoke.Sph.Domain;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Api;
using Humanizer;
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

            var headers = from p in jt.SelectTokens("request.headers").SelectMany(x => x)
                          let ov = p.SelectToken("value").Value<string>()
                          select new HttpHeaderDefinition
                          {
                              Name = p.SelectToken("name").Value<string>(),
                              DefaultValue = ov,
                              OriginalValue = ov,
                              Field = new ConstantField
                              {
                                  Value = ov,
                                  Type = typeof(string),
                                  Name = ov.Truncate(20, "..."),
                                  WebId = Guid.NewGuid().ToString()
                              }
                          };
            this.HeaderDefinitionCollection.ClearAndAddRange(headers);


            // post data
            var postData = from p in jt.SelectTokens("request.postData.params").SelectMany(x => x)
                           let field = p.SelectToken("name")
                           where null != field
                           && !string.IsNullOrWhiteSpace(field.Value<string>())
                           select new RegexMember
                           {
                               FieldName = field.Value<string>(),
                               Type = typeof(string)
                           };
            this.RequestMemberCollection.AddRange(postData);

            // for multipart/form-data
            var mimeType = jt.SelectToken("request.postData.mimeType");
            if (null != mimeType && mimeType.Value<string>() == "multipart/form-data")
            {
                var text = jt.SelectToken("request.postData.text").Value<string>();
                var formFields = from f in Strings.RegexValues(text, @"name=\""(?<fname>.*?)\""", "fname")
                                 select new RegexMember
                           {
                               FieldName = f,
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

        public override Dictionary<string, string> GenerateResponseCode()
        {
            var sources = new Dictionary<string, string>();

            var requestTypeName = (this.HttpMethod + "_" + this.Name).ToCsharpIdentitfier() + "Response";
            var code = new StringBuilder();
            code.AppendLine(this.GetCodeHeader());
            code.AppendLine("   public class " + requestTypeName + " : DomainObject");
            code.AppendLine("   {");

            code.AppendLine("       [XmlIgnore]");
            code.AppendLine("       [JsonIgnore]");
            code.AppendLine("       public string ResponseText{ get; private set;}");



            code.AppendLinf("       public async Task LoadAsync(HttpResponseMessage response)", this.HttpMethod, this.Name);
            code.AppendLine("       {");
            code.AppendLine("           var content = response.Content as StreamContent;");
            code.AppendLine("           if(null == content) throw new Exception(\"Fail to read from response\");");
            code.AppendLine("           this.ResponseText = await content.ReadAsStringAsync();");

            foreach (var m in this.ResponseMemberCollection.OfType<RegexMember>().Where(m => !string.IsNullOrWhiteSpace(m.Group) && !string.IsNullOrWhiteSpace(m.Pattern)))
            {
                code.AppendLine(m.GenerateParseCode("this"));
            }
            //objects
            foreach (var m in this.ResponseMemberCollection.OfType<RegexMember>().Where(m => m.Type == typeof(object)))
            {
                var m1 = m;
                code.AppendLinf("           //{0}", m1.Name);
                code.AppendLinf("           this.{0} = new {0}();", m1.Name);
                var objectsChildren = from mc in m.MemberCollection.OfType<RegexMember>()
                                      select mc.GenerateParseCode("this." + m1.Name);
                objectsChildren.ToList().ForEach(x => code.AppendLine(x));

            }

            // array
            var count = 1;
            foreach (var m in this.ResponseMemberCollection.OfType<RegexMember>().Where(m => m.Type == typeof(Array)))
            {
                var count1 = count;
                var first = m.MemberCollection.OfType<RegexMember>().FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Group) && !string.IsNullOrWhiteSpace(x.Pattern));
                if (null == first) continue;

                code.AppendLine("           // " + m.Name);
                code.AppendLinf("           var values{2} = Strings.RegexValues(this.ResponseText, {1}, \"{0}\");", first.Group, first.Pattern.ToLiteral(), count);
                code.AppendLinf("           var colls{1} = new ObjectCollection<{0}>(Enumerable.Range(0,values{1}.Length).Select(x => new {0}() ));", m.Name.Replace("Collection", ""), count);


                code.AppendLinf("           for(int i = 0 ; i < values{0}.Length; i++)", count);
                code.AppendLine("           {");

                var collectionChildrenCodes = from cm in m.MemberCollection.OfType<RegexMember>()
                                              where !string.IsNullOrWhiteSpace(cm.Group)
                                                    && !string.IsNullOrWhiteSpace(cm.Pattern)
                                              select cm.GenerateParseCode("   colls" + count1 + "[i]");
                collectionChildrenCodes.ToList().ForEach(x => code.AppendLine(x));


                code.AppendLine("           }");
                code.AppendLinf("           this.{0}.AddRange(colls{1});", m.Name, count);
                count++;
            }

            code.AppendLine("       }");

            // properties for each members
            foreach (var member in this.ResponseMemberCollection)
            {
                code.AppendLinf("       //member:{0}", member.Name);
                code.AppendLine(member.GeneratedCode());
            }


            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace

            sources.Add(requestTypeName + ".cs", code.ToString());


            // classes for members
            foreach (var member in this.ResponseMemberCollection.Where(m => m.Type == typeof(object) || m.Type == typeof(Array)))
            {
                var mc = GetCodeHeader() + member.GeneratedCustomClass() + "\r\n}";
                sources.Add(member.Name + ".cs", mc);
            }

            return sources;
        }

        public override Dictionary<string, string> GenerateRequestCode()
        {
            var sources = new Dictionary<string, string>();

            var typeName = (this.HttpMethod + "_" + this.Name).ToCsharpIdentitfier() + "Request";
            var code = new StringBuilder();
            code.AppendLine(this.GetCodeHeader());
            code.AppendLine("   public class " + typeName + " : DomainObject");
            code.AppendLine("   {");

            if (!string.IsNullOrWhiteSpace(this.RequestRouting))
            {
                code.AppendLine("       public string GenerateUrl(string url)");
                code.AppendLine("       {");
                code.AppendLine("           return \"" + this.RequestRouting.Replace("{", "\" + Uri.EscapeUriString(").Replace("}", ".ToEmptyString()) + \"") + "\";");
                code.AppendLine("       }");
            }


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

            sources.Add(typeName + ".cs", code.ToString());
            // classes for members
            foreach (var member in this.RequestMemberCollection.Where(m => m.Type == typeof(object) || m.Type == typeof(Array)))
            {
                var mc = GetCodeHeader() + member.GeneratedCustomClass() + "\r\n}";
                sources.Add(member.Name + ".cs", mc);
            }


            return sources;
        }


        public string GenerateActionCode(HttpAdapter adapter, string methodName)
        {
            var code = new StringBuilder();
            var op = this;
            code.AppendLine(CreateMethodCode(adapter, methodName, op));
            code.AppendLine(OperationLoginCode(op));
            code.AppendLine("           var client = m_client;");

            code.AppendLine(!string.IsNullOrWhiteSpace(op.RequestRouting)
                ? "           var url = request.GenerateUrl(REQUEST_URL);"
                : "           var url = REQUEST_URL;");

            var sendCode = HttpClientSendCodeGenerator.Create(op).GenerateCode(op);
        

            foreach (var c in sendCode.Split(new[] { Environment.NewLine, "\r\n", "\n" }, StringSplitOptions.None))
            {
                code.AppendLine("           " + c);
            }   
            // custom headers
            var automaticHeaders = new[] { "content-type", "content-length", "cookie", "accept-encoding" };
            foreach (var hd in this.HeaderDefinitionCollection)
            {
                if(automaticHeaders.Contains(hd.Name.ToLowerInvariant()))continue;
                code.AppendLinf("           requestMessage.Headers.Add(\"{0}\", \"{1}\");", hd.Name, hd.Field.GetValue(null));
            }


            code.AppendLine("           var response = await client.SendAsync(requestMessage);");

            code.AppendLine(CreateResponseCode(op, methodName));


            code.AppendLine("       }");

            return code.ToString();
        }


        private string CreateMethodCode(HttpAdapter adapter, string methodName, HttpOperationDefinition op)
        {
            var code = new StringBuilder();
            code.AppendLinf("       public async Task<{0}Response> {0}Async({0}Request request)",
                methodName.ToCsharpIdentitfier());
            code.AppendLine("       {");
            code.AppendLinf("           const string REQUEST_URL = \"{0}\";", op.Url.Replace(adapter.BaseAddress, ""));

            return code.ToString();
        }

        private string CreateResponseCode(HttpOperationDefinition op, string methodName)
        {
            var code = new StringBuilder();
            if (op.EnsureSuccessStatusCode)
                code.AppendLine("           response.EnsureSuccessStatusCode();");

            code.AppendLine("           if(response.IsSuccessStatusCode)");
            code.AppendLine("           {");

            if (op.IsLoginOperation)
            {
                code.AppendLine("           this.IsAuthenticated = true;// TODO - verify with the response");
            }

            code.AppendLinf("               var result =  new {0}Response();", methodName.ToCsharpIdentitfier());
            code.AppendLine("               await result.LoadAsync(response);");
            code.AppendLine("               return result;");
            code.AppendLine("           }");
            code.AppendLine("           return null;");

            return code.ToString();
        }

        private string OperationLoginCode(HttpOperationDefinition op)
        {
            if (op.IsLoginRequired)
            {
                return
                    "           " +
                    "if(!this.IsAuthenticated)" +
                    " throw new InvalidOperationException(\"You must be logged in\");";
            }
            return string.Empty;
        }
    }
}
